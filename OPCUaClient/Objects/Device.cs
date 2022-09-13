using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUaClient.Objects
{

    /// <summary>
    /// Device on the OPC UA Server
    /// </summary>
    public class Device
    {

        /// <summary>
        /// Name of the device
        /// </summary>
        public String Name { 
            get
            {
                return this.Address.Substring(this.Address.LastIndexOf(".") + 1);
            }
        }
        /// <summary>
        /// Address of the device
        /// </summary>
        public String Address { get; set; }

        /// <summary>
        /// Groups into the device
        /// </summary>
        public List<Group> Groups { get; set; } = new List<Group>();

        /// <summary>
        /// Tags into the device
        /// </summary>
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
