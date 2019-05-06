using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartschoolApi
{
    internal class JSONGroup
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public bool isKlas { get; set; }
        public bool isOfficial { get; set; }
    }
}
