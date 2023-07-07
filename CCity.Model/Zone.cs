using System.Buffers.Text;

namespace CCity.Model
{
    public abstract class Zone : Placeable, IFlammable, IUpgradeable
    {
        #region Fields

        private Level _level;
        private const int upgradeCost = 100;

        #endregion

        #region Constants
        
        private const int BasicUpgradeCost= 5000;
        private const int BeginnerCapacity = 10;
        private const int IntermediateCapacity = 30;
        private const int AdvancedCapacity = 100;

        #endregion
       
        #region Properties

        /// <summary>
        /// The value of the zone's needed electricity
        /// </summary>
        public override int NeededElectricity => Count;

        /// <summary>
        /// Number of citizens living/working in the zone
        /// </summary>
        public int Count => Citizens.Count;
        
        /// <summary>
        /// Max number of citizens living/working in the zone
        /// </summary>
        public int Capacity => _level switch
        {
            Level.Beginner => BeginnerCapacity,
            Level.Intermediate => IntermediateCapacity,
            Level.Advanced => AdvancedCapacity,
            _ => throw new System.NotImplementedException()
        };

        /// <summary>
        /// List of Citizens living in the zone
        /// </summary>
        public List<Citizen> Citizens { get; }

        /// <summary>
        /// The potential to ignite
        /// </summary>
        public abstract float Potential { get; }

        /// <summary>
        /// Returns if the zone is full or not (if the number of citizens living/working in the zone is equal to the capacity)
        /// </summary>
        public bool Full => Count == Capacity;

        /// <summary>
        /// Returns if the zone is empty or not (if the number of citizens living/working in the zone is zero)
        /// </summary>
        public bool Empty => Count == 0;

        /// <summary>
        /// Returns if the number of citizens is below the half of the capacity
        /// </summary>
        public bool BelowHalfPopulation => Count * 2 < Capacity;

        /// <summary>
        /// The citizen's desire to move in
        /// </summary>
        public double DesireToMoveIn { get; set; }

        /// <summary>
        /// The effect based on the average distance between citizens' homes and workplaces
        /// </summary>
        public double DistanceEffect { get; set; }

        bool IFlammable.Burning { get; set; }

        Level IUpgradeable.Level { get => _level; set => _level = value; }

        int IUpgradeable.NextUpgradeCost => _level != Level.Advanced ? ((int)_level + 1) * BasicUpgradeCost : 0;

        bool IUpgradeable.CanUpgrade => _level != Level.Advanced;

        ushort IFlammable.Health { get; set; } = IFlammable.FlammableMaxHealth;

        #endregion

        #region Constructors

        internal Zone()
        {
            Citizens = new List<Citizen>();
            _level = Level.Beginner;
        }
        
        #endregion
        
        #region Public methods

        /// <summary>
        /// Adds a citizen to the zone
        /// </summary>
        /// <param name="citizen"> The citizen to be added</param>
        /// <returns> True if the citizen was added, false if the zone is full</returns>
        public bool AddCitizen(Citizen citizen)
        {
            if (Count + 1 > Capacity) 
                return false;
            
            Citizens.Add(citizen);
            return true;
        }

        /// <summary>
        /// Drops a citizen from the zone
        /// </summary>
        /// <param name="citizen"> The citizen to be dropped</param>
        /// <returns> True if the citizen was dropped, false if the citizen was not in the zone</returns>
        public bool DropCitizen(Citizen citizen) => Citizens.Remove(citizen);

        /// <summary>
        /// Upgrades the zone
        /// </summary>
        public void Upgrade()
        {
            if (_level == Level.Advanced) return;
            _level++;
        }
        #endregion
    }
}
