using System;
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
            await Groups.Load();
            Assert.IsNotNull(Groups.Root);
            Assert.IsTrue(Groups.Root.Children.Count > 0);

            // asume that the root group is not a class, is visible and not official
            Assert.IsTrue(Groups.Root.Type == GroupType.Group);
            Assert.IsTrue(Groups.Root.Visible);
            Assert.IsFalse(Groups.Root.Official);
        }

        [TestMethod]
        public async Task AddAndDeleteGroup()
        {
            await Groups.Load();
            IGroup leerlingen = Groups.Root.Find("Leerlingen");
            Assert.IsTrue(leerlingen != null);

            Group newGroup = new Group(leerlingen);
            newGroup.Name = "unittestgroup";
            newGroup.Description = "may be deleted";
            newGroup.Type = GroupType.Group;
            newGroup.Code = "UTGRP";
            bool result = await Groups.Save(newGroup);
            Assert.IsTrue(result);

            await Groups.Reload();
            IGroup testGroup = Groups.Root.Find("unittestgroup");
            Assert.IsNotNull(testGroup);
            Assert.IsNotNull(testGroup.Parent);
            Assert.IsTrue(testGroup.Parent.Name == "Leerlingen");

            await Groups.Delete(testGroup);
            await Groups.Reload();
            testGroup = Groups.Root.Find("unittestgroup");
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
            result = await Groups.AddUserToGroup(testaccount, testgroup);
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

            result = await Groups.RemoveUserFromGroup(testaccount, testgroup);
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
    }
}

