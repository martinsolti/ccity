using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCity.Model
{
    public class GameErrorException : Exception
    {
        /// <summary>
        /// The cause of the error
        /// </summary>
        public GameErrorType ErrorType { get; private set;}

        public GameErrorException(GameErrorType error)
        {
            ErrorType = error;
        }

    }
}
