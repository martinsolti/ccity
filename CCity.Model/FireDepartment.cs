namespace CCity.Model;

/// <summary>
/// A building that can send fire trucks to extinguish fires and effects fields in its near proximity, decreasing chances of them catching fire.
/// </summary>
public class FireDepartment : Placeable
{
    #region Constants

    /// <summary>
    /// The radius of the fire department effect.
    /// </summary>
    private const int effectRadius = 6;

    #endregion
    
    #region Properties

    public override int PlacementCost => 1000;

    public override int MaintenanceCost => 200;

    public override int NeededElectricity => 20;

    public override Field? Owner
    {
        get => base.Owner;
            
        internal set
        {
            if (value != null)
                FireTruck = new FireTruck(value);
                
            base.Owner = value;
        }
    }

    /// <summary>
    /// The fire truck of the fire department.
    /// Initially <c>null</c>, but set when the fire department is actually placed.
    /// </summary>
    public FireTruck FireTruck { get; private set; } = null!;

    /// <summary>
    /// Indicates whether the fire truck of the fire department is deployed.
    /// Note: the fire department cannot be demolished if its fire truck is deployed.
    /// </summary>
    internal bool FireTruckDeployed => FireTruck.Deployed;

    #endregion
        
    #region Public methods

    public override List<Field> Effect(Func<Placeable, bool, Action<Field, int>, int, List<Field>> spreadingFunction, bool add)
    {
        if (EffectSpreaded == add) return new();
        EffectSpreaded = add;
        return spreadingFunction(this, add, (f, i) => f.ChangeFireDepartmentEffect(i), effectRadius);
    }

    #endregion
}