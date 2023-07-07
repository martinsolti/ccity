namespace CCity.Model;

/// <summary>
/// Encapsulates all properties necessary for extinguishing fire cases. 
/// </summary>
public class FireTruck
{
    #region Constants
    
    /// <summary>
    /// The amount of time a fire truck stays stationary next to a fire when it is extinguishing it.
    /// This "simulates" the actual extinguishing process.
    /// </summary>
    private const int RescueCounterMax = MainModel.TicksPerSecond * 2; // 2 seconds
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// The field representing the current location of the fire truck.
    /// </summary>
    public Field Location => PathCurrentNode?.Value ?? Station;
    
    /// <summary>
    /// The field representing the fire department that the fire truck belongs to.
    /// </summary>
    /// <seealso cref="FireDepartment.FireTruck"/>
    internal Field Station { get; }
    
    /// <summary>
    /// Indicates whether a fire truck is currently assigned a fire case to extinguish.
    ///
    /// If the value of this property is <c>true</c> and <see cref="Moving"/> is also <c>true</c>, that means the fire truck is currently on its way to the fire case.
    ///
    /// If the value of this property is <c>true</c> but <see cref="Moving"/> is <c>false</c>, that means the fire truck is currently standing next to the fire and extinguishing it.
    ///
    /// If the value of this property is <c>false</c> but <see cref="Moving"/> is <c>true</c>, that means the fire truck is on its way back to its station (<see cref="Station"/>).
    ///
    /// If the value of this property is <c>false</c> and <see cref="Moving"/> is <c>false</c>, that means that the fire truck is standing by at its station.
    /// </summary>
    /// <seealso cref="Moving"/>
    internal bool Active { get; private set; }
    
    /// <summary>
    /// Indicates whether the fire truck is currently moving (changing its location in the current tick).
    ///
    /// See the documentation for <see cref="Active"/> to know more about
    /// </summary>
    /// <seealso cref="Active"/>
    internal bool Moving { get; private set; }

    /// <summary>
    /// Helper property indicating whether the fire truck is deployed. <c>true</c> if the fire truck is either active or moving.
    /// </summary>
    /// <seealso cref="Active"/>
    /// <seealso cref="Moving"/>
    internal bool Deployed => Active || Moving;

    /// <summary>
    /// Indicates whether in its current deployment the fire truck was deployed from its station.
    /// If <c>false</c>, the return route must be calculated separately from the outbound route.
    /// </summary>
    internal bool DepartedFromStation { get; private set; }
    private LinkedListNode<Field>? PathCurrentNode { get; set; }

    // For convenience reasons, the return path should be a list whose **FIRST** item is the fire truck's home department,
    // and last item is the location of the fire emergency that the fire truck is assigned to.
    //
    // This is so because if the fire truck is ordered to a fire emergency from its home department, the return path
    // is the same list as the fire truck's path to the fire emergency and PathCurrentNode 
    //
    // Using this, the last node of the return path is sufficient to be stored
    private LinkedListNode<Field>? ReturnPathLastNode { get; set; }

    private int ExtinguishCounter { get; set; }

    #endregion
    
    internal FireTruck(Field station)
    {
        Station = station;
        
        Active = false;
        Moving = false;
        DepartedFromStation = true;
        
        ExtinguishCounter = 0;
    }

    /// <summary>
    /// Updates the location of the fire truck in the current tick.
    /// </summary>
    /// <exception cref="Exception">Thrown if attempted to update the location of a fire truck that is standing by at its station.</exception>
    internal void Update()
    {
        if (!Active && !Moving)
            throw new Exception("Internal inconsistency: Attempted to update a fire truck that is neither active nor moving");

        switch (Moving, Active, RescueCounter: ExtinguishCounter)
        {
            case (true, true, _) when PathCurrentNode?.Next == null:
                // Arrived at the fire
                Moving = false;
                break;
            case (true, false, _) when PathCurrentNode?.Previous?.Value == Station:
                // Arrived at the station
                Moving = false;
                
                // Reset paths
                PathCurrentNode = null;
                ReturnPathLastNode = null;
                break;
            case (true, _, _):
                // On its way, either to the fire or back to the station
                // Step onto the next field
                // The next field is the next node in the path if the truck is on its way
                //  to the fire, or the previous node if it's on its way back
                PathCurrentNode = Active ? PathCurrentNode?.Next : PathCurrentNode?.Previous;
                break;
            case (false, true, < RescueCounterMax):
                // At the fire, still extinguishing
                ExtinguishCounter++;
                break;
            case (false, true, >= RescueCounterMax):
                // At the fire, extinguished
                Active = false;
                Moving = true;
                    
                // Turn back to the station
                PathCurrentNode = ReturnPathLastNode;
                break;
        }
    }

    /// <summary>
    /// Deploys a fire truck to a fire case.
    /// </summary>
    /// <param name="path">A linked list of fields representing the path the fire truck should take to get to its assigned fire case.</param>
    /// <param name="returnPath">A linked list of fields representing the path the fire truck should take to get back to its station after it has extinguished the fire.</param>
    /// <exception cref="Exception">Thrown if attempted to deploy a fire truck that is active or if either of the paths are invalid.</exception>
    internal void Deploy(LinkedList<Field> path, LinkedList<Field> returnPath)
    {
        if (Active)
            throw new Exception("Internal inconsistency: Attempted to assign a fire emergency to a fire truck that is currently active.");
        
        if (returnPath.Last?.Value != path.Last?.Value && returnPath.First?.Value != Station)
            throw new Exception("Internal inconsistency: Attempted to assign a path to a fire truck whose last stop is not the first stop of the return path. The return path should be a list whose **FIRST** item is the fire truck's home department, and last item is the location of the fire emergency that the fire truck is assigned to.");

        if (!Moving)
            DepartedFromStation = true;
        
        Active = true;
        Moving = true;
        
        ExtinguishCounter = 0;

        PathCurrentNode = path.First;
        ReturnPathLastNode = returnPath.Last;
    }

    /// <summary>
    /// Cancels the deployment of the fire truck.
    /// After canceling, the fire truck will head back to its station using the given return path.
    /// If no return path is specified, the fire truck will simply reverse on the path it used to travel towards its assigned fire case.
    /// </summary>
    /// <param name="returnPath">The path on which the fire truck should return to its station.</param>
    /// <exception cref="Exception">Thrown if attempted to cancel a fire truck that is not active.</exception>
    internal void Cancel(LinkedList<Field>? returnPath)
    {
        if (!Active)
            throw new Exception("Internal inconsistency: Attempted to cancel a fire truck that is not active");
        
        if (returnPath != null)
            ReturnPathLastNode = returnPath.Last;

        Active = false;
        Moving = true;
    }
}