namespace CCity.Model;

/// <summary>
/// Manages game mechanisms related to fire -- igniting flammable buildings, handling burning buildings and sending fire trucks.
/// </summary>
internal class FireManager
{
    #region Constants

    /// <summary>
    /// If the health of a burning building goes below this constant, fire will spread to its flammable neighbors.
    /// </summary>
    private const ushort FireSpreadThreshold = IFlammable.FlammableMaxHealth / 2;

    #endregion

    #region Properties

    /// <summary>
    /// Indicates whether there are any burning buildings in the current tick.
    /// </summary>
    internal bool FirePresent => ActiveFires.Any();

    /// <summary>
    /// Indicates whether there are any fire trucks deployed in the current tick.
    /// </summary>
    internal bool FireTrucksDeployed => DeployedFireTrucks.Any();
    
    private FieldManager FieldManager { get; }
    
    private HashSet<FireDepartment> FireDepartments { get; }

    private HashSet<Placeable> Flammables { get; }
    
    private HashSet<Fire> ActiveFires { get; }
    
    private HashSet<FireTruck> DeployedFireTrucks { get; }
    
    private Random Random { get; }
    
    #endregion
    
    #region Constructors

    internal FireManager(FieldManager fieldManager)
    {
        FieldManager = fieldManager;
        FireDepartments = new HashSet<FireDepartment>();
        Flammables = new HashSet<Placeable>();
        ActiveFires = new HashSet<Fire>();
        DeployedFireTrucks = new HashSet<FireTruck>();
        
        Random = new Random(DateTime.Now.Millisecond);
    }

    #endregion
    
    #region Internal Methods

    /// <summary>
    /// Adds a fire department to be kept track of by the fire manager.
    /// The player can only send fire trucks to extinguish a fire from fire departments that are kept track of by the model's fire manager.
    /// Should be called whenever the player places a new fire department on the map.
    /// </summary>
    internal void AddFireDepartment(FireDepartment fireDepartment) => FireDepartments.Add(fireDepartment);

    /// <summary>
    /// Removes a fire department from the list of fire departments being kept track of by the fire manager.
    /// Should be called whenever the player demolishes a fire department.
    /// </summary>
    internal void RemoveFireDepartment(FireDepartment fireDepartment) => FireDepartments.Remove(fireDepartment);
    
    /// <summary>
    /// Adds a flammable building to the list of flammables that are being kept track of by the fire manager.
    /// Should be called whenever the player places any flammable building.
    /// </summary>
    /// <exception cref="Exception">Thrown when the building that is being added is not a flammable building.</exception>
    internal void AddFlammable(Placeable placeable)
    {
        if (placeable is not IFlammable)
            throw new Exception("Internal inconsistency: Attempted to track non-flammable placeable as flammable");

        Flammables.Add(placeable);
    }

    /// <summary>
    /// Removes a flammable building from the list of flammables that are being kept track of by the fire manager.
    /// Should be called whenever the player demolishes a flammable building that is not currently burning.
    /// </summary>
    /// <exception cref="Exception">Thrown when the building being removed is on fire.</exception>
    internal void RemoveFlammable(Placeable placeable)
    {
        if (placeable is not IFlammable { Burning: false })
            throw new Exception("Internal inconsistency: Attempted to remove tracking of flammable that is burning");
        
        Flammables.Remove(placeable);
    }

    /// <summary>
    /// Adds a fire case to the list of fire cases that are managed by the fire manager.
    /// Called whenever a fire breaks out (<see cref="Fire.BreakOut"/>).
    /// </summary>
    /// <param name="fire"></param>
    internal void AddFire(Fire fire) => ActiveFires.Add(fire);

    /// <summary>
    /// Removes a fire case from the list of fire cases that are managed by the fire manager.
    /// Called whenever a fire is neutralized (<see cref="Fire.Neutralize"/>).
    /// </summary>
    /// <param name="fire"></param>
    internal void RemoveFire(Fire fire) => ActiveFires.Remove(fire);
    
    /// <summary>
    /// Returns the fire at the given placeable if it is on fire.
    /// If the placeable is not flammable or is not on fire, this method will return <c>null</c>.
    /// </summary>
    internal Fire? Fire(Placeable placeable) => ActiveFires.FirstOrDefault(f => f.Location == placeable.Owner);

    /// <summary>
    /// Returns a list containing fields representing the locations of those fire trucks that are deployed in the current tick.
    /// </summary>
    internal IEnumerable<Field> FireTruckLocations() => DeployedFireTrucks.Select(ft => ft.Location);
    
    /// <summary>
    /// Randomly ignites some buildings from the list of flammables that are being kept track of by the fire manager.
    /// The chances of ignition are based on the flammable's potential (<see cref="IFlammable.Potential"/>).
    /// If there were no buildings ignited, the method returns an empty list.
    /// </summary>
    /// <returns>A list containing the fields of the buildings that were ignited.</returns>
    internal List<Field> IgniteRandomFlammable()
    {
        var result = new List<Field>();
        
        foreach (var placeable in Flammables)
        {
            if (Random.NextSingle() >= ((IFlammable)placeable).Potential)
                continue;

            var fireLocation = Model.Fire.BreakOut(this, placeable)!.Location;
            result.Add(fireLocation);
            
            if (fireLocation.Placeable is IMultifield multifield)
                result.AddRange(multifield.Occupies.Select(f => f.Owner!));
        }

        return result;
    }
    
    /// <summary>
    /// Updates the state and health of the buildings that are being effected by the active fire cases.
    /// This method damages the burning buildings and accounts for those that are destroyed by fire.
    /// Please note that this method does not remove the wrecked buildings from the map -- that must be done separately.
    /// </summary>
    /// 
    /// <returns>
    /// A tuple of lists containing fields that were effected by the active fire cases.
    /// The first item of this tuple (named <c>Updated</c>) is a list of fields where fire is present in the current tick, i.e. fields whose buildings have been damaged by the fire in the current tick.
    /// The second item of this tuple (named <c>Wrecked</c>) is a list of fields whose buildings have been destroyed by the fire in the current tick.
    /// </returns>
    internal (List<Field> Updated, List<Field> Wrecked) UpdateFires()
    {
        (List<Field> Updated, List<Field> Wrecked) result = (new List<Field>(), new List<Field>());
        
        foreach (var fire in ActiveFires.ToList())  // We need .ToList() because fire.Neutralize() and SpreadFire() will
        {                                           // modify the iterated collection. Not the most efficient, but likely 
            switch (fire.AssignedFireTruck)         // there will not be that many buildings burning at once.
            {
                case null or { Active: true, Moving: true }:
                    // Damage the building if there is no assigned fire truck yet or it is still on its way
                    var oldHealth = fire.Flammable.Health;
            
                    fire.Damage();

                    if (oldHealth > FireSpreadThreshold && fire.Flammable.Health <= FireSpreadThreshold)
                        result.Updated.AddRange(SpreadFire(fire));

                    if (fire is { AssignedFireTruck: not null, Flammable.Health: <= 0 })
                        // Cancel the fire truck's deployment if the fire burned down the building before it could arrive
                        CancelFireTruck(fire.AssignedFireTruck);
                    
                    break;
                case { Active: false }:
                    fire.Neutralize();
                    break;
            }

            (fire.Flammable.Health > 0 ? result.Updated : result.Wrecked).Add(fire.Location);
            
            if (fire.Flammable.Health > 0 && fire.Location.Placeable is IMultifield multifield)
                result.Updated.AddRange(multifield.Occupies.Select(f => f.Owner!));
        }

        return result;
    }

    /// <summary>
    /// Deploys a fire truck to extinguish the specified fire.
    /// On calling this method, if the deployment is successful, <see cref="FireTrucksDeployed"/> becomes <c>true</c>.
    /// After deploying, <see cref="UpdateFireTrucks"/> should be called in every tick until <see cref="FireTrucksDeployed"/> becomes <c>false</c>.
    /// </summary>
    /// <param name="fire">The fire that the fire truck should extinguish.</param>
    /// <returns>A <see cref="FireTruck"/> object or <c>null</c> if there are no fire trucks available to deploy.</returns>
    internal FireTruck? DeployFireTruck(Fire fire)
    {
        // Find nearest fire truck
        var fireTruck = NearestAvailableFireTruck(fire.Location);

        if (fireTruck == null)
            // We cannot deploy any fire trucks -> the game will give an error message
            return null;

        var destination = new HashSet<Field> { fire.Location };
        
        if (fire.Location.Placeable is IMultifield multifield)
            destination.UnionWith(multifield.Occupies.Select(f => f.Owner!));
        
        // Find shortest road from the fire truck's location to the fire
        var path = Utilities.ShortestRoad(FieldManager.Fields, FieldManager.Width, FieldManager.Height, fireTruck.Location, destination);

        if (!path.Any())
            return null;
        
        // Calculate the return path to the station
        var returnPath = fireTruck.Moving 
            ? Utilities.ShortestRoad(FieldManager.Fields, FieldManager.Width, FieldManager.Height, fireTruck.Station, new HashSet<Field> { path.Last!.Value }) 
            : path;

        if (!returnPath.Any())
            return null;
        
        // Deploy the fire truck
        fireTruck.Deploy(path, returnPath);
        DeployedFireTrucks.Add(fireTruck);
        
        fire.AssignedFireTruck = fireTruck;

        return fireTruck;
    }
    
    /// <summary>
    /// Updates the locations of the deployed fire trucks on the map.
    /// Should be called in every tick when there are fire trucks deployed. <see cref="FireTrucksDeployed"/>
    /// For fire trucks that are on the way to a fire or back to the fire department, this method advances them on their path.
    /// For those that already reached the fire, it keeps the fire truck in the same place until the fire is extinguished.
    /// </summary>
    /// <returns>A list of fields representing the locations of the fire trucks in the PREVIOUS TICK. (The new/current location of the fire trucks can be retrieved by calling <see cref="FireTruckLocations"/>.)</returns>
    internal List<Field> UpdateFireTrucks()
    {
        var result = new List<Field>();

        foreach (var fireTruck in DeployedFireTrucks.ToList())  // We need .ToList() because we are modifying the iterated collection.
        {                                                       // This is not the most efficient, but likely there won't be 
            result.Add(fireTruck.Location);                     // thousands of fire trucks deployed at once.
            fireTruck.Update();
            
            if (!fireTruck.Deployed)
                DeployedFireTrucks.Remove(fireTruck);
        }

        return result;
    }
    
    #endregion
    
    #region Private Methods
    
    private IEnumerable<Field> SpreadFire(Fire fire)
    {
        var flammableNeighbors = FieldManager.GetNeighbours(fire.Location.Placeable!)
            .Where(p => p is Zone { Empty: false } or not Zone and IFlammable)
            .ToList();

        foreach (var neighbor in flammableNeighbors)
            Model.Fire.BreakOut(this, neighbor);

        return flammableNeighbors.Select(p => p.Owner!);
    }
    
    private FireTruck? NearestAvailableFireTruck(Field f)
    {
        var availableFireTrucks = AvailableFireTrucks().ToList();
        var nearestFireTruck = availableFireTrucks.FirstOrDefault();
        var smallestDistance = Utilities.AbsoluteDistance(f, nearestFireTruck?.Location);
            
        foreach (var fireTruck in availableFireTrucks)
        {
            var currentDistance = Utilities.AbsoluteDistance(f, fireTruck.Location);

            if (currentDistance < smallestDistance)
                (nearestFireTruck, smallestDistance) = (fireTruck, currentDistance);
        }

        return nearestFireTruck;
    }

    private void CancelFireTruck(FireTruck fireTruck) => fireTruck.Cancel(fireTruck.DepartedFromStation
        ? null
        : Utilities.ShortestRoad(FieldManager.Fields, FieldManager.Width, FieldManager.Height, fireTruck.Station, new HashSet<Field> { fireTruck.Location }));
    
    private IEnumerable<FireTruck> AvailableFireTrucks() => FireDepartments
        .Where(f => f is { IsElectrified: true, FireTruck.Active: false })
        .Select(f => f.FireTruck);
    
    #endregion
}