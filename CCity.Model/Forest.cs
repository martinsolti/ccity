using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCity.Model
{
    public class Forest : Placeable
    {
        /// <summary>
        /// The radius of forest effect
        /// </summary>
        const int effectRadius = 3;

        /// <summary>
        /// The value of max age, the forest does`t grow anymore if it reach it
        /// </summary>
        public int MaxAge => 10;

        #region Properties 

        public override int PlacementCost => 100;

        public override int MaintenanceCost => CanGrow ? 10 : 0;

        public override bool EffectSpreadingCondition => !IsDemolished;

        public override bool ListingCondition => !IsDemolished && CanGrow;

        /// <summary>
        /// the amount of growth month
        /// </summary>
        public int GrowthMonts { get; private set; }

        /// <summary>
        /// true if the age will change in the next month
        /// </summary>
        public bool WillAge => GrowthMonts % 12 == 11;

        /// <summary>
        /// The value of effect according to age
        /// </summary>
        public double EffectRate => Math.Max((double)Age/MaxAge,(double)1/MaxAge); 

        /// <summary>
        /// True if the forest is under max age
        /// </summary>
        public bool CanGrow => Age < MaxAge;

        /// <summary>
        /// The age of the forest
        /// </summary>
        public int Age => GrowthMonts / 12;

        public override bool IsPublic => true;

        #endregion

        #region Constructor

        public Forest(bool starter=false)
        {
            if (!starter) GrowthMonts = 0;
            else GrowthMonts = MaxAge * 12;
        }

        #endregion

        #region public methods

        public override List<Field> Effect(Func<Placeable, bool, Action<Field,int>,int,List<Field> > spreadingFunction, bool add)
        {
            if (EffectSpreaded == add) return new();
            EffectSpreaded = add;
            return spreadingFunction(this, add, (f, i) => f.ChangeForestEffect(i), effectRadius);
        }

        /// <summary>
        /// grow the forest, if it is not max aged
        /// </summary>
        public void Grow()
        {
            if(CanGrow)
            {
                ++GrowthMonts;
            }
        }

        #endregion
    }

}
