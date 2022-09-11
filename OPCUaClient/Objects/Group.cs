using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUaClient.Objects
{

    /// <summary>
    /// Group of tags. Not implemented yet.
    /// </summary>
    public class Group
    {

        /// <summary>
        /// Name of the group
        /// </summary>
        public String Name
        {
            get
            {
                return this.Address.Substring(this.Address.LastIndexOf(".") + 1);
            }
        }

        /// <summary>
        /// Address of the group
        /// </summary>
        public String Address { get; set; }
        /// <summary>
        /// Groups into the group
        /// </summary>
        public List<Group> Groups { get; set; }
        /// <summary>
        /// Tags into the group
        /// </summary>
        public List<Tag> Tags { get; set; }
    }
}
