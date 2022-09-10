using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUaClient.Objects
{
    public class Device
    {
        public String Name { get; set; }
        public String Address { get; set; }
        public List<Group> Groups { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
