using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUaClient.Exceptions
{
    /// <summary>
    /// Error reading value
    /// </summary>
    public class ReadException : Exception
    {

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="message">
        /// Error message
        /// </param>
        public ReadException(string? message) : base(message)
        {
        }
    }
}
