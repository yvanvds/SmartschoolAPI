using AbstractAccountApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartschoolApi
{
    public static class Groups
    {
        private static IGroup root;
        // the root itself is just an empty hook
        public static IGroup Root { get => root.Children != null ? root.Children[0] : null; }

        public static async Task Load()
        {
            if (root != null) return;
            await Reload();
        }

        public static int Count(bool onlyClassGroups)
        {
            if (root == null) return 0;
            if (onlyClassGroups) return root.CountClassGroupsOnly;
            return root.Count;
        }

        public static async Task Reload()
        {
            var result = await Task.Run(
              () => Connector.service.getAllGroupsAndClasses(Connector.password)
            );

            byte[] data = Convert.FromBase64String(result as string);
            string decoded = Encoding.UTF8.GetString(data);

            root = new Group(null);
            Group current = root as Group;
            using (XmlReader reader = XmlReader.Create(new StringReader(decoded)))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "group":
                                if (current.Children == null) current.Children = new List<IGroup>();
                                current.Children.Add(new Group(current));
                                current = current.Children.Last() as Group;
                                //Debug.WriteLine("new group");
                                break;
                            case "name":
                                if (reader.Read()) current.Name = reader.Value;
                                //Debug.WriteLine(current.Name);
                                break;
                            case "desc":
                                if (reader.Read()) current.Description = reader.Value;
                                //Debug.WriteLine(current.Description);
                                break;
                            case "type":
                                if (reader.Read()) switch (reader.Value)
                                    {
                                        case "G": current.Type = GroupType.Group; break;
                                        case "K": current.Type = GroupType.Class; break;
                                        default: current.Type = GroupType.Invalid; break;
                                    }
                                //Debug.WriteLine(current.Type);
                                break;
                            case "code":
                                if (reader.Read()) current.Code = reader.Value;
                                //Debug.WriteLine(current.Code);
                                break;
                            case "untis":
                                if (reader.Read()) current.Untis = reader.Value;
                                //Debug.WriteLine(current.Untis);
                                break;
                            case "visible":
                                if (reader.Read()) current.Visible = reader.Value.Equals("1") ? true : false;
                                //Debug.WriteLine(current.Visible);
                                break;
                            case "isOfficial":
                                if (reader.Read()) current.Official = reader.Value.Equals("1") ? true : false;
                                //Debug.WriteLine(current.Official);
                                break;
                            case "coAccountLabel":
                                if (reader.Read()) current.CoAccountLabel = reader.Value;
                                //Debug.WriteLine(current.CoAccountLabel);
                                break;
                            case "adminNumber":
                                if (reader.Read())
                                {
                                    int var = 0;
                                    int.TryParse(reader.Value, out var);
                                    current.AdminNumber = var;
                                    //Debug.WriteLine(current.AdminNumber);
                                }
                                break;
                            case "instituteNumber":
                                if (reader.Read())
                                {
                                    current.InstituteNumber = reader.Value;
                                    //Debug.WriteLine(current.InstituteNumber);
                                }
                                break;
                            case "username":
                                if (reader.Read())
                                {
                                    if (current.Titulars == null) current.Titulars = new List<string>();
                                    current.Titulars.Add(reader.Value);
                                }
                                //Debug.WriteLine(current.Titulars.Last());
                                break;
                        }

                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == "group") // end of group
                        {
                            current = current.Parent as Group;
                            //Debug.WriteLine("switched to parent");
                        }
                    }
                }
            }

            if (root != null) Root.Sort();
        }

        // get the logical parent for this group, even it does not exist yet
        public static string GetLogicalParent(string classgroup)
        {
            string number = classgroup.Substring(0, 1);
            int year = 0;
            try
            {
                year = Convert.ToInt32(number);
            }
            catch (Exception e)
            {
                Error.AddError(e.Message);
                return "";
            }

            if (year < 1 || year > 7) return "";
            if (Connector.StudentYear.Count() == 7)
            {
                return Connector.StudentYear[year - 1];
            }
            else if (Connector.StudentGrade.Count() == 3)
            {
                switch (year)
                {
                    case 1:
                    case 2:
                        return Connector.StudentGrade[0];
                    case 3:
                    case 4:
                        return Connector.StudentGrade[1];
                    default:
                        return Connector.StudentGrade[2];
                }
            }
            return Connector.StudentPath;
        }

        public static async Task<bool> Save(IGroup group)
        {
            int result = 0;

            if (group.Type == GroupType.Class)
            {
                var r = await Task.Run(() => Connector.service.saveClass(
                  Connector.password,
                  group.Name,
                  group.Description, // TODO: add warning (empty description is not accepted by smartschool)
                  group.Code,
                  group.Parent.Code,
                  group.Untis,
                  group.InstituteNumber,
                  group.AdminNumber.ToString(),
                  ""
                ));
                result = Convert.ToInt32(r);
            }
            else if (group.Type == GroupType.Group)
            {
                var r = await Task.Run(() => Connector.service.saveGroup(
                  Connector.password,
                  group.Name,
                  group.Description,
                  group.Code,
                  group.Parent.Code,
                  group.Untis
                ));
                result = Convert.ToInt32(r);
            }

            if (result != 0)
            {
                Error.AddError(result);
                return false;
            }
            return true;
        }

        public static async Task<bool> Delete(IGroup group)
        {
            var result = await Task.Run(() => Connector.service.delClass(
              Connector.password,
              group.Code
            ));

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }
            return true;
        }

        public static async Task<bool> MoveUserToClass(IAccount account, IGroup group, DateTime date)
        {
            if (group.Type != GroupType.Class || !group.Official)
            {
                Connector.log.AddError("Smartschool Groups API: You can only move users to official classes");
                return false;
            }
            var result = await Task.Run(() => Connector.service.saveUserToClass(
              Connector.password,
              account.UID,
              group.Name,
              Utils.DateToString(date)
            ));

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }
            return true;
        }

        public static async Task<bool> AddUserToGroup(IAccount account, IGroup group)
        {
            if (group.Official)
            {
                Connector.log.AddError("Smartschool Groups API: Users cannot be added to official classes. Use the MoveUserToClass method instead.");
                return false;
            }
            var result = await Task.Run(() => Connector.service.saveUserToClassesAndGroups(
              Connector.password,
              account.UID,
              group.Name,
              1
            ));

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }
            return true;
        }

        public static async Task<bool> RemoveUserFromGroup(IAccount account, IGroup group)
        {
            if (group.Official)
            {
                Connector.log.AddError("Smartschool Groups API: Users cannot be removed from official classes.");
                return false;
            }

            var result = await Task.Run(() => Connector.service.removeUserFromGroup(
              Connector.password,
              account.UID,
              group.Name,
              Utils.DateToString(DateTime.Now)
            ));

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }
            return true;
        }
    }
}
