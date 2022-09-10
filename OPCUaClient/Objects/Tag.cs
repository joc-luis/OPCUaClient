using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUaClient.Objects
{
    public class Tag
    {
        public String Name
        {
            get
            {
                return this.Address.Substring(this.Address.LastIndexOf("."));
            }
        }
        public String Address { get; set; }
        public Object Value { get; set; }

        public Boolean Quality { get; set; }
    }
}
