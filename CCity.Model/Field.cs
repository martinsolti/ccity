using Priority_Queue;

namespace CCity.Model
{
    public class Field
    {
        #region Constants
        /// <summary>
        /// Maxium effect that the police departments can have on the field
        /// </summary>
        internal const int MAX_POLICE_DEPARTMENT_EFFECT = 10;

        /// <summary>
        /// Maximum effect that the stadiums can have on the field
        /// </summary>
        internal const int MAX_STADIUM_EFFECT = 10;

        /// <summary>
        /// Maximum effect that the fire departments can have on the field
        /// </summary>
        internal const int MAX_FIRE_DEPARTMENT_EFFECT = 10;

        /// <summary>
        /// Maximum effect that the forests can have on the field
        /// </summary>
        internal const int MAX_FOREST_EFFECT = 20;

        /// <summary>
        /// Maximum effect that the undustrial zones can have on the field
        /// </summary>
        internal const int MAX_INDUSTRIAL_EFFECT = 30;
        #endregion

        #region Fields
        /// <summary>
        /// Reference of the field route placeable (it is different, if it is multifield)
        /// </summary>
        public Placeable? Placeable { get => _placeable == null ? null : _placeable.Root; }

        /// <summary>
        /// Reference of the field actual placeable
        /// </summary>
        public Placeable? ActualPlaceable { get => _placeable; }
        
        private Placeable? _placeable;
        private int _policeDepartmentEffect;
        private int _stadiumEffect;
        private int _fireDepartmentEffect;
        private int _forestEffect;
        private int _industrialEffect;

        /// <summary>
        /// the value of x cordinate 
        /// </summary>
        public int X { get; }


        public int Y { get; }


        public bool HasPlaceable { get => Placeable != null; }

        /// <summary>
        /// The value of the effects of police departments
        /// </summary>
        public double PoliceDepartmentEffect => Math.Min(_policeDepartmentEffect, MAX_POLICE_DEPARTMENT_EFFECT) / (double)MAX_POLICE_DEPARTMENT_EFFECT;

        /// <summary>
        /// The value of the effects of stadiums
        /// </summary>
        public double StadiumEffect => Math.Min(_stadiumEffect, MAX_STADIUM_EFFECT) / (double)MAX_STADIUM_EFFECT;

        /// <summary>
        /// The value of the effects of fire departments
        /// </summary>
        public double FireDepartmentEffect => Math.Min(_fireDepartmentEffect, MAX_FIRE_DEPARTMENT_EFFECT) / (double)MAX_FIRE_DEPARTMENT_EFFECT;

        /// <summary>
        /// The value of the effects of forests
        /// </summary>
        public double ForestEffect => Math.Min(_forestEffect, MAX_FOREST_EFFECT) / (double)MAX_FOREST_EFFECT;

        /// <summary>
        /// The value of the effects of industrial zones
        /// </summary>
        public double IndustrialEffect => Math.Max(Math.Min(_industrialEffect, MAX_INDUSTRIAL_EFFECT) / (double)MAX_INDUSTRIAL_EFFECT,0);

        #endregion

        #region Constructors

        public Field(int x, int y)
        {
            _placeable = null;
            X = x;
            Y = y;
            _policeDepartmentEffect = 0;
            _stadiumEffect = 0;
            _fireDepartmentEffect = 0;
            _forestEffect = 0;
            _industrialEffect = 0;
        }

        #endregion

        #region Public methods}

        /// <summary>
        /// Change the value of PoliceDepartmentEffect
        /// </summary>
        /// <param name="n">the amount of the change</param>
        public void ChangePoliceDepartmentEffect(int n)
        {
            _policeDepartmentEffect += n;
        }

        /// <summary>
        /// Change the value of StadiumEffect
        /// </summary>
        /// <param name="n">the amount of the change</param>
        public void ChangeStadiumEffect(int n)
        {
            _stadiumEffect += n;
        }

        /// <summary>
        /// Change the value of FireDepartmentEffect
        /// </summary>
        /// <param name="n">the amount of the change</param>
        public void ChangeFireDepartmentEffect(int n)
        {
            _fireDepartmentEffect += n;
        }

        /// <summary>
        /// Change the value of ForestEffect
        /// </summary>
        /// <param name="n">the amount of the change</param>
        public void ChangeForestEffect(int n)
        {
            _forestEffect += n;
        }


        /// <summary>
        /// Change the value of IndustrialEffect
        /// </summary>
        /// <param name="n">the amount of the change</param>
        public void ChangeIndustrialEffect(int n)
        {
            _industrialEffect += n;
        }

        /// <summary>
        /// place the placeable at the field
        /// </summary>
        /// <param name="placeable">the placed placeable</param>
        /// <returns>True, if the it was succesfull</returns>
        internal bool Place(Placeable placeable)
        {
            if (_placeable != null) return false;
            _placeable = placeable;
            placeable.PlaceAt(this);
            return true;
        }

        /// <summary>
        /// Demolish the placeable from the field
        /// </summary>
        /// <returns>True, if the it was succesfull</returns>
        internal bool Demolish()
        {
            if (_placeable == null) return false;
            _placeable = null;
            return true;
        }

        #endregion

    }
}
