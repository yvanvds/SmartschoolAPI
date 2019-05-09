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
    public class UnitTestAccounts
    {
        public UnitTestAccounts()
        {
            Connector.Init(Settings.Default.school, Settings.Default.passphrase, new TestLogger());
        }

        ~UnitTestAccounts()
        {
            Connector.Disconnect();
        }

        [TestMethod]
        public async Task DontSaveEmptyUser()
        {
            // saving an empty account to smartschool should fail
            Account empty = new Account();
            bool result = await Accounts.Save(empty, "");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateAndDeleteUser()
        {
            Account test = TestAccounts.Student();
            bool result = await Accounts.Save(test, "Abc123D!");
            Assert.IsTrue(result);

            result = await Accounts.Delete(test);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CreateLoadAndDeleteUser()
        {
            Account local = TestAccounts.Student();
            Account remote = new Account();
            bool result = await Accounts.Save(local, "Abc123D!");
            Assert.IsTrue(result);

            remote.UID = local.UID;
            result = await Accounts.Load(remote);
            Assert.IsTrue(result);

            Assert.IsTrue(local.UID == remote.UID, "UID is incorrect");
            Assert.IsTrue(local.AccountID == remote.AccountID, "AccountID is incorrect");
            Assert.IsTrue(local.RegisterID == remote.RegisterID, "RegisterID is incorrect");
            Assert.IsTrue(local.Role == remote.Role, "Role is incorrect");
            Assert.IsTrue(local.GivenName == remote.GivenName, "GivenName is incorrect");
            Assert.IsTrue(local.SurName == remote.SurName, "SurName is incorrect");
            Assert.IsTrue(local.ExtraNames == remote.ExtraNames, "ExtraNames is incorrect");
            Assert.IsTrue(local.Initials == remote.Initials, "Initials is incorrect");
            Assert.IsTrue(local.Gender == remote.Gender, "Gender is incorrect");
            Assert.IsTrue(Utils.SameDay(local.Birthday, remote.Birthday), "Birthday is incorrect");
            Assert.IsTrue(local.BirthPlace.Equals(remote.BirthPlace), "Birthplace is incorrect");
            Assert.IsTrue(local.BirthCountry.Equals(remote.BirthCountry), "BirthCountry is incorrect");
            Assert.IsTrue(local.Street.Equals(remote.Street), "Street is incorrect");
            Assert.IsTrue(local.HouseNumber.Equals(remote.HouseNumber), "HouseNumber is incorrect");
            Assert.IsTrue(local.HouseNumberAdd.Equals(remote.HouseNumberAdd), "HouseNumberAdd is incorrect");
            Assert.IsTrue(local.PostalCode.Equals(remote.PostalCode), "PostalCode is incorrect");
            Assert.IsTrue(local.City.Equals(remote.City), "City is incorrect");
            Assert.IsTrue(local.MobilePhone.Equals(remote.MobilePhone), "Mobilephone is incorrect");
            Assert.IsTrue(local.HomePhone.Equals(remote.HomePhone), "Homephone is incorrect");
            Assert.IsTrue(local.Fax.Equals(remote.Fax), "Fax is incorrect");
            Assert.IsTrue(local.Mail.Equals(remote.Mail), "Mail is incorrect");

            result = await Accounts.Delete(local);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ChangeUID()
        {
            Account test = TestAccounts.Student();
            bool result = await Accounts.Save(test, "Abc123D!");
            Assert.IsTrue(result);

            test.UID = "changedUID";
            result = await Accounts.ChangeUID(test);
            Assert.IsTrue(result);

            // loading is done on UID, so the account should be the same
            // and we can compare any field except UID
            Account remote = new Account();
            remote.UID = "changedUID";
            result = await Accounts.Load(remote);
            Assert.IsTrue(remote.RegisterID.Equals(test.RegisterID));

            result = await Accounts.Delete(test);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ChangeAccountID()
        {
            Account test = TestAccounts.Student();
            bool result = await Accounts.Save(test, "Abc123D!");
            Assert.IsTrue(result);

            test.AccountID = "ACCOUNTID";
            result = await Accounts.ChangeAccountID(test);
            Assert.IsTrue(result);

            Account remote = new Account();
            remote.UID = test.UID;
            result = await Accounts.Load(remote);
            Assert.IsTrue(remote.AccountID.Equals(test.AccountID));

            result = await Accounts.Delete(test);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task AccountStatus()
        {
            Account test = TestAccounts.Student();
            bool result = await Accounts.Save(test, "Abc123D!");
            Assert.IsTrue(result);

            AccountState state = await Accounts.GetStatus(test);
            Assert.IsTrue(state != AccountState.Invalid);

            result = await Accounts.SetStatus(test, AccountState.Active);
            Assert.IsTrue(result);

            state = await Accounts.GetStatus(test);
            Assert.IsTrue(state == AccountState.Active);

            result = await Accounts.SetStatus(test, AccountState.Inactive);
            Assert.IsTrue(result);

            state = await Accounts.GetStatus(test);
            Assert.IsTrue(state == AccountState.Inactive);

            result = await Accounts.SetStatus(test, AccountState.Administrative);
            Assert.IsTrue(result);

            state = await Accounts.GetStatus(test);
            Assert.IsTrue(state == AccountState.Administrative);

            result = await Accounts.Delete(test);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetListOfAccounts()
        {
            await GroupManager.Reload();

            IGroup students = GroupManager.Root.Find("Leerlingen");

            await Accounts.LoadAccounts(students);
            Assert.IsTrue(students.NumAccounts() > 50);
        }

        [TestMethod]
        public async Task TestJSon()
        {
            await GroupManager.Load();

            IGroup personnel = GroupManager.Root.Find("Secretariaat");

            await Accounts.LoadAccounts(personnel);
            var obj = personnel.ToJson(true);
            File.WriteAllText("SmartschoolGroupsAndAccounts.json", obj.ToString());

            Assert.IsTrue(File.Exists("SmartschoolGroupsAndAccounts.json"), "json file does not exist");

            string content = File.ReadAllText("SmartschoolGroupsAndAccounts.json");
            var newObj = Newtonsoft.Json.Linq.JObject.Parse(content);
            var newGroup = new Group(null, newObj);

            Assert.IsTrue(newGroup.Equals(personnel, true, true));
        }
    }
}
