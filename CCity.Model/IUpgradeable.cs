namespace CCity.Model
{
    public interface IUpgradeable
    {
        
        #region Properties

        /// <summary>
        /// The level of the zone
        /// </summary>
        public  Level Level { get; set; }

        /// <summary>
        /// The cost of the next upgrade
        /// </summary>
        public  int NextUpgradeCost { get; }

        /// <summary>
        /// True if can upgrade
        /// </summary>
        public  bool CanUpgrade { get;}

        #endregion

        #region Public methods

        public void Upgrade()
        {
            if (CanUpgrade)
                Level++;

        }

        #endregion
    }
}