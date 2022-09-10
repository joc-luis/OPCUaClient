using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUaClient.Objects
{
    public class Group
    {
        public String Name
        {
            get
            {
                return this.Address.Substring(this.Address.LastIndexOf(".") + 1);
            }
        }
        public String Address { get; set; }
        public List<Group> Groups { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
