using AbstractAccountApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartschoolApi.Rules
{
    public static class Rules
    {
        public static Dictionary<Rule, string> ImportRules = new Dictionary<Rule, string>()
        {
            { Rule.SS_DiscardGroup, "Negeer Groep" },
            { Rule.SS_NoSubGroups, "Negeer SubGroepen" },
        };
    }
}
