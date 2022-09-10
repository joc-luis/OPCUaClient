using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUaClient.Exceptions
{
    public class ReadException : Exception
    {
        public ReadException(string? message) : base(message)
        {
        }
    }
}
