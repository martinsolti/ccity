using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CCity.Model
{
    public abstract class Placeable
    {
        #region Properties

        /// <summary>
        /// The field the placeable is placed on
        /// </summary>
        public virtual Field? Owner { get; internal set; }

        /// <summary>
        /// The cost of the placeable's placement
        /// </summary>
        public abstract int PlacementCost { get; }

        /// <summary>
        /// The cost of the placeable's maintenance
        /// </summary>
        public abstract int MaintenanceCost { get; }

        /// <summary>
        /// The amount of the placeable's needed electricity
        /// </summary>
        public virtual int NeededElectricity => 0;

        /// <summary>
        /// Indicates whether the effect of the placeable is already spreaded or not
        /// </summary>
        public bool EffectSpreaded { get; internal set; }

        /// <summary>
        /// Indicates whether the placeable is demolished or not
        /// </summary>
        public bool IsDemolished { get => Owner != null && Owner.Placeable == null; }

        /// <summary>
        /// The root of the placeable (if the placeable is a filler, it returns its main, returns itself otherwise)
        /// </summary>
        public Placeable Root => this is Filler filler ? (Placeable)filler.Main : this;

        /// <summary>
        /// Indicates whether the conditions for effect spreading are present
        /// </summary>
        public virtual bool EffectSpreadingCondition => IsPublic && IsElectrified && !IsDemolished;

        /// <summary>
        /// Indicates whether the conditions for listing are present
        /// </summary>
        public virtual bool ListingCondition => IsPublic && !IsDemolished;

        //SPREADING

        /// <summary>
        /// Stores which placeable is the root of the spread source for each spread type (Placeable is the root of an S spread, it GetsSpreadFrom[S].root = itself)
        /// </summary>
        public Dictionary<SpreadType, (Placeable? direct, Placeable? root)> GetsSpreadFrom { get; set; }

        /// <summary>
        /// Stores the max amount of spread value for each spread type (if Placeable is root, then MaxSpread is the value of its capacity, the needed value otherwise)
        /// </summary>
        public Dictionary<SpreadType, Func<int>> MaxSpreadValue { get; set; }

        /// <summary>
        /// Stores the current amount of spread value for each spread type (if Placeable is root, then CurrentSpreadValue is the value of how many values are used out of its capacity, the current value otherwise)
        /// </summary>
        public Dictionary<SpreadType, int> CurrentSpreadValue { get; set; }

        /// <summary>
        /// Indicates whether the placeable is accessible from the main road or not
        /// </summary>
        public virtual bool IsPublic => GetsSpreadFrom[SpreadType.Publicity].root != null;

        /// <summary>
        /// Indicates whether the placeable is electrified or not
        /// </summary>
        public bool IsElectrified => CurrentSpreadValue[SpreadType.Electricity] == MaxSpreadValue[SpreadType.Electricity]() && GetsSpreadFrom[SpreadType.Electricity].root != null || GetsSpreadFrom[SpreadType.Electricity].root == this;

        /// <summary>
        /// Indicates whether the placeable is partly electrified or not
        /// </summary>
        public bool IsPartlyElectrified => CurrentSpreadValue[SpreadType.Electricity] > 0 && !IsElectrified;

        #endregion

        #region Constructors

        public Placeable()
        {
            EffectSpreaded = false;

            GetsSpreadFrom = new();
            CurrentSpreadValue = new();
            MaxSpreadValue = new();

            InitSpread(SpreadType.Publicity);
            InitSpread(SpreadType.Electricity);
            MaxSpreadValue[SpreadType.Electricity] = () => NeededElectricity;
        }

        #endregion

        #region Private methods

        private void InitSpread(SpreadType spreadType)
        {
            GetsSpreadFrom.Add(spreadType, (null, null));
            CurrentSpreadValue.Add(spreadType, 0);
            MaxSpreadValue.Add(spreadType, () => 0);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Makes the placeable a root of a spread
        /// </summary>
        /// <param name="spreadType">The type of the spread</param>
        public virtual void MakeRoot(SpreadType spreadType)
        {
            GetsSpreadFrom[spreadType] = (null, this);
        }

        /// <summary>
        /// Places the placeable on a field
        /// </summary>
        /// <param name="field">The field to place the placeable on</param>
        public void PlaceAt(Field field)
        {
            Owner = field;
        }

        /// <summary>
        /// Spreads or revokes the effect of the placeable
        /// </summary>
        /// <param name="f">The effect spreading function that implements the spreading's logic. It has 3 parameters (the placeable, a bool (spread or revoke), an action that changes the field's proper effect value, an integer (the value of the effect change)) and it returns a list of field that has changed during the spreading/revoking</param>
        /// <param name="b">Spreading (true) or revocation (false)</param>
        /// <returns></returns>
        public virtual List<Field> Effect(Func<Placeable, bool, Action<Field, int>, int, List<Field>> f, bool b) => new();


        /// <summary>
        /// Indicates whether the placeable could give publicity to an other placeable (based on placeable type)
        /// </summary>
        /// <param name="_">The other placeable</param>
        /// <returns></returns>
        public virtual bool CouldGivePublicityTo(Placeable _) => false;

        /// <summary>
        /// Indicates whether the placeable could give electricity to an other placeable (based on placeable type)
        /// </summary>
        /// <param name="_">The other placeable</param>
        /// <returns></returns>
        public virtual bool CouldGiveElectricityTo(Placeable placeable)
        {
            return placeable switch
            {
                Zone => true,
                FireDepartment => true,
                PoliceDepartment => true,
                Stadium => true,
                Pole => true,
                Road => true,
                _ => false
            };
        }

        #endregion
    }
}
