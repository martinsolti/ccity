namespace CCity.Model;

/// <summary>
/// Encapsulates properties of a fire case.
/// </summary>
internal class Fire
{
    /// <summary>
    /// The field representing the location of the fire.
    /// </summary>
    internal Field Location { get; }
    
    /// <summary>
    /// Convenience property for the building that is on fire, as a flammable.
    /// </summary>
    internal IFlammable Flammable => (IFlammable)Location.Placeable!;

    /// <summary>
    /// The fire truck that is assigned to the fire case.
    /// Set by <see cref="FireManager"/>.
    /// </summary>
    /// <seealso cref="FireManager"/>
    internal FireTruck? AssignedFireTruck { get; set; }
    
    /// <summary>
    /// The <see cref="FireManager"/> that is responsible for handing the fire case.
    /// Fires cannot be broken out without a fire manager being assigned to handle them.
    /// </summary>
    /// <seealso cref="FireManager"/>.
    private FireManager Manager { get; }

    private Fire(FireManager manager, Field location)
    {
        if (location.Placeable is not IFlammable { Burning: true })
            throw new Exception("Internal inconsistency: Attempted to assign a field as the destination of a fire emergency whose placeable is not a burning building");

        Manager = manager;
        Location = location;
    }

    /// <summary>
    /// Decreases the health of burning building.
    /// If the health of the burning building is <c>0</c>, the fire is neutralized (as the building is wrecked).
    /// </summary>
    internal void Damage()
    {
        Flammable.Health -= 1;

        if (Flammable.Health <= 0)
            Neutralize();
    }

    /// <summary>
    /// Neutralizes the fire.
    /// </summary>
    internal void Neutralize()
    {
        Flammable.Burning = false;
        Manager.RemoveFire(this);
    }

    /// <summary>
    /// Breaks out a fire at a given building.
    /// </summary>
    /// <param name="manager">The fire manager that is responsible for handling the fire case.</param>
    /// <param name="placeable">The building that caught on fire.</param>
    /// <returns>A fire object or <c>null</c> if the building is not a flammable whose health is greater than <c>0</c>.</returns>
    internal static Fire? BreakOut(FireManager manager, Placeable placeable)
    {
        if (placeable is not IFlammable { Burning: false, Health: > 0 } flammable)
            return null;
            
        flammable.Burning = true;
        flammable.Health = IFlammable.FlammableMaxHealth; // Reset the building's health upon ignition

        var result = new Fire(manager, placeable.Owner!);
        manager.AddFire(result);

        return result;
    }
}