using AbstractAccountApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartschoolApi
{
    public class Group : IGroup
    {
        public Group(IGroup parent)
        {
            this.Parent = parent;
        }

        string name = string.Empty;
        public string Name { get => name; set => name = value; }

        string description = string.Empty;
        public string Description { get => description; set => description = value; }

        GroupType type = GroupType.Invalid;
        public GroupType Type { get => type; set => type = value; }

        string code = string.Empty;
        public string Code { get => code; set => code = value; }

        string untis = string.Empty;
        public string Untis { get => untis; set => untis = value; }

        bool visible = false;
        public bool Visible { get => visible; set => visible = value; }

        bool official = false;
        public bool Official { get => official; set => official = value; }

        string coAccountLabel = string.Empty;
        public string CoAccountLabel { get => coAccountLabel; set => coAccountLabel = value; }

        int adminNumber = 0;
        public int AdminNumber { get => adminNumber; set => adminNumber = value; }

        string instituteNumber = string.Empty;
        public string InstituteNumber { get => instituteNumber; set => instituteNumber = value; }

        List<string> titulars;
        public List<string> Titulars { get => titulars; set => titulars = value; }

        List<IGroup> children;
        public List<IGroup> Children { get => children; set => children = value; }

        List<IAccount> accounts = new List<IAccount>();
        public List<IAccount> Accounts { get => accounts; set => accounts = value; }

        public IGroup Parent { get; set; } = null;

        public IGroup Find(string name)
        {
            if (Name.Equals(name)) return this;

            if (children == null) return null;

            if (Connector.DiscardSubgroups.Contains(Name)) return null;

            foreach (var group in children)
            {

                var result = group.Find(name);
                if (result != null) return result;
            }
            return null;
        }

        public int CountClassGroupsOnly
        {
            get
            {
                int count = 0;
                if (Connector.DiscardSubgroups.Contains(Name)) return count;

                if (children != null) foreach (var group in children)
                    {
                        count += group.CountClassGroupsOnly;
                    }
                if (Official)
                {
                    count++;
                }

                return count;
            }
        }

        public int Count
        {
            get
            {
                int count = 0;
                if (Connector.DiscardSubgroups.Contains(Name)) return count;

                if (children != null) foreach (var group in children)
                    {
                        count += group.Count;
                    }
                count++;


                return count;
            }
        }

        public int NumAccounts()
        {
            int count = 0;
            if (Connector.DiscardSubgroups.Contains(Name)) return count;

            if (children != null)
            {
                foreach (var group in children)
                {
                    try
                    {
                        count += group.NumAccounts();
                    }
                    catch (Exception e)
                    {
                        Connector.log.AddError("Smartschool API Group Error: " + e.Message + "in group " + group.Name);
                    }
                }
            }
            if (Accounts != null)
            {
                count += Accounts.Count;
            }

            return count;
        }

        public void Sort()
        {
            if (children == null) return;
            children.Sort((x, y) => x.Name.CompareTo(y.Name));
            foreach (var child in children)
            {
                child.Sort();
            }
        }

        public void GetTreeAsList(List<IGroup> list)
        {

            if (children != null && !Connector.DiscardSubgroups.Contains(Name))
            {
                foreach (var child in children)
                {
                    child.GetTreeAsList(list);
                }
            }

            list.Add(this);
        }

        public async Task LoadAccounts()
        {
            var jsonResult = await Task.Run(
             () => Connector.service.getAllAccountsExtended(Connector.password, Name, "0")
            );

            if (jsonResult is int)
            {
                // probably just a group without direct accounts
                if ((int)jsonResult == 19)
                {
                    return;
                }
                else
                {
                    Error.AddError((int)jsonResult);
                    return;
                }
            }

            try
            {
                List<JSONAccount> details = JsonConvert.DeserializeObject<List<JSONAccount>>(jsonResult.ToString());

                Accounts.Clear();
                foreach (var account in details)
                {
                    Accounts.Add(new Account());
                    Accounts.Last().Group = Name;
           
                    SmartschoolApi.Accounts.LoadFromJSON(Accounts.Last(), account);
                }

                Error.AddMessage("Added " + Accounts.Count.ToString() + " Accounts to " + Name);
            }
            catch (Exception e)
            {
                Error.AddError(e.Message);
            }
        }
    }
}
