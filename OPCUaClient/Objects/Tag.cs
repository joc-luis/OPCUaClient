using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OPCUaClient.Objects
{

    /// <summary>
    /// Representation in class of a the tag of OPC UA Server
    /// </summary>
    public class Tag
    {

        /// <summary>
        /// Name of the tag
        /// </summary>
        public String Name
        {
            get
            {
                return this.Address.Substring(this.Address.LastIndexOf(".") + 1);
            }
        }

        /// <summary>
        /// Address of the tag
        /// </summary>
        public String Address { get; set; }

        /// <summary>
        /// Value of the tag
        /// </summary>
        public Object Value { get; set; }


        /// <summary>
        /// Quality of the tag
        /// </summary>
        public Boolean Quality { get; set; }
    }
}
