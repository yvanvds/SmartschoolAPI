using System;
using System.IO;
using System.Threading.Tasks;
using AbstractAccountApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartschoolApi;
using SmartSchoolTests.Properties;

namespace SmartSchoolTests
{
    [TestClass]
    public class UnitTestGroups
    {
        public UnitTestGroups()
        {
            Connector.Init(Settings.Default.school, Settings.Default.passphrase, new TestLogger());
        }

        ~UnitTestGroups()
        {
            Connector.Disconnect();
        }

        [TestMethod]
        public async Task LoadGroups()
        {
            await GroupManager.Load();
            Assert.IsNotNull(GroupManager.Root);
            Assert.IsTrue(GroupManager.Root.Children.Count > 0);

            // asume that the root group is not a class, is visible and not official
            Assert.IsTrue(GroupManager.Root.Type == GroupType.Group);
            Assert.IsTrue(GroupManager.Root.Visible);
            Assert.IsFalse(GroupManager.Root.Official);
        }

        [TestMethod]
        public async Task AddAndDeleteGroup()
        {
            await GroupManager.Load();
            IGroup leerlingen = GroupManager.Root.Find("Leerlingen");
            Assert.IsTrue(leerlingen != null);

            Group newGroup = new Group(leerlingen);
            newGroup.Name = "unittestgroup";
            newGroup.Description = "may be deleted";
            newGroup.Type = GroupType.Group;
            newGroup.Code = "UTGRP";
            bool result = await GroupManager.Save(newGroup);
            Assert.IsTrue(result);

            await GroupManager.Reload();
            IGroup testGroup = GroupManager.Root.Find("unittestgroup");
            Assert.IsNotNull(testGroup);
            Assert.IsNotNull(testGroup.Parent);
            Assert.IsTrue(testGroup.Parent.Name == "Leerlingen");

            await GroupManager.Delete(testGroup);
            await GroupManager.Reload();
            testGroup = GroupManager.Root.Find("unittestgroup");
            Assert.IsNull(testGroup);
        }

        [TestMethod]
        public async Task AddTeacherToGroup()
        {
            Account testaccount = TestAccounts.Teacher();
            bool result = await Accounts.Save(testaccount, "Abc123D!");
            Assert.IsTrue(result);

            Group testgroup = new Group(null);
            testgroup.Name = Settings.Default.teacherGroup;
            result = await GroupManager.AddUserToGroup(testaccount, testgroup);
            Assert.IsTrue(result);

            await Accounts.LoadAccounts(testgroup);

            bool found = false;
            foreach (var account in testgroup.Accounts)
            {
                if (account.UID.Equals("UnitTeacher"))
                {
                    found = true;
                    break;
                }
            }
            Assert.IsTrue(found);

            result = await GroupManager.RemoveUserFromGroup(testaccount, testgroup);
            Assert.IsTrue(result);

            await Accounts.LoadAccounts(testgroup);

            found = false;
            foreach (var account in testgroup.Accounts)
            {
                if (account.UID.Equals("UnitTeacher"))
                {
                    found = true;
                    break;
                }
            }
            Assert.IsFalse(found);
        }

        [TestMethod]
        public async Task TestJSon()
        {
            await GroupManager.Load();
            Assert.IsNotNull(GroupManager.Root, "Group Root should not be null");
            var obj = GroupManager.Root.ToJson();
            File.WriteAllText("SmartschoolGroups.json", obj.ToString());

            Assert.IsTrue(File.Exists("SmartschoolGroups.json"), "json file does not exist");

            string content = File.ReadAllText("SmartschoolGroups.json");
            var newObj = Newtonsoft.Json.Linq.JObject.Parse(content);
            var newGroups = new Group(null, newObj);

            Assert.IsTrue(newGroups.Equals(GroupManager.Root, true), "The group tree is not the same");
        }
    }
}

