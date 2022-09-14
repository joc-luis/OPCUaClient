using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUaClient.Objects
{

    /// <summary>
    /// Group of tags.
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
        /// Groups into the group <see cref="Group"/>
        /// </summary>
        public List<Group> Groups { get; set; } = new List<Group>();

        /// <summary>
        /// Tags into the group <see cref="Tag"/>
        /// </summary>
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
