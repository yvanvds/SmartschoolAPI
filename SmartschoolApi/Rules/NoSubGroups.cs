using AbstractAccountApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AbstractAccountApi.ObservableProperties;

namespace SmartschoolApi.Rules
{
    public class NoSubGroups : IRule
    {
        private Rule rule;
        public Rule Rule => rule;

        private RuleType ruleType;
        public RuleType RuleType => ruleType;

        private RuleAction ruleAction;
        public RuleAction RuleAction => ruleAction;

        private string header;
        public string Header => header;

        private string description;
        public string Description => description;

        public Prop<string> ShortInfo { get; set; } = new Prop<string> { Value = "Groep: " };

        private string groupName;

        public bool Enabled { get; set; }

        public NoSubGroups()
        {
            setDefaults();
        }

        public NoSubGroups(JObject obj)
        {
            setDefaults();
            groupName = obj.ContainsKey("GroupName") ? obj["GroupName"].ToString() : "";
            ShortInfo.Value = "Groep: " + groupName;
        }

        private void setDefaults()
        {
            rule = Rule.SS_NoSubGroups;
            ruleType = RuleType.SS_Import;
            ruleAction = RuleAction.Discard;
            header = "Negeer Subgroepen";
            description = "Zorgt er voor dat de subgroepen van deze groep niet geimporteerd zal worden.";
        }

        public bool ShouldApply(object obj)
        {
            if((obj as Group).HasParent(groupName))  return true;
            return false;
        }

        public string getConfig(int ID)
        {
            return groupName;
        }

        public void Modify(object obj)
        {
            // not needed
        }

        public void setConfig(int ID, string data)
        {
            // we need only one piece of data here: the name of the group
            groupName = data;
            ShortInfo.Value = "Groep: " + groupName;
        }

        public JObject ToJson()
        {
            var result = new JObject();
            result["Rule"] = rule.ToString();
            result["GroupName"] = groupName;
            return result;
        }
    }
}
