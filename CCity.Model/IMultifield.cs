using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCity.Model
{
    public interface IMultifield
    {

        #region Properties

        /// <summary>
        /// The Width of the IMultifield placeable
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The Height of the IMultifield placeable
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Store the Fillers
        /// </summary>
        public List<Filler> Occupies { get; internal set; }

        #endregion

    }
}
