using AbstractAccountApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartschoolApi
{
    public static class Accounts
    {
        /// <summary>
        /// Saves an account to smartschool. This can be used to create a new user as well as update an
        /// existing user, but the account values must be complete. Also a new password must be provided. There are other methods to do a partial
        /// user update.
        /// </summary>
        /// <param name="account">The account to send to smartschool.</param>
        /// <param name="pw1">The new password for the user.</param>
        /// <param name="pw2">The password for the first co-account. This is optional and will not change the password if empty.</param>
        /// <param name="pw3">The password for the second co-account. This is option ald will not change the password if empty.</param>
        /// <returns>Returns true on success. False when the transaction fails. When false, an error will be logged.</returns>
        public static async Task<bool> Save(IAccount account, string pw1, string pw2 = "", string pw3 = "")
        {
            // first convert values to smartschool format

            // Account role needs to be in string format
            string role = "";
            switch (account.Role)
            {
                case AccountRole.Student: role = "Leerling"; break;
                case AccountRole.Support:
                case AccountRole.IT:
                case AccountRole.Teacher: role = "Leerkracht"; break;
                case AccountRole.Director: role = "Directie"; break;
                default: return false; // other accounts should be no part of smartschool
            }

            // Smartschool gender only has m/f values
            string gender = "f";
            if (account.Gender == GenderType.Male) gender = "m";
            else if (account.Gender == GenderType.Transgender) gender = "f"; // Smartschool only knows about male or female. Hopefully they'll discover the 21st century soon!

            // Birthday needs to be a string (year-month-day)
            string birthday = Utils.DateToString(account.Birthday);

            // stamboeknummer is a string value. If equal to zero, it should be an empty string. If less than 1.000.000, a string needs
            // to be added upfront.
            string stemID = account.StemID.ToString();
            if (account.StemID == 0) stemID = "";
            else if (account.StemID < 1000000) stemID = "0" + stemID;

            // The street address must be passed as one string, but will be split on the server side.
            // Splitting occurs on the last space, to separate the house number.
            // If a / is included after that house number, it will be a buss number.
            string StreetAddress = account.Street;
            if (account.HouseNumber != string.Empty) StreetAddress += " " + account.HouseNumber;
            if (account.HouseNumberAdd != string.Empty) StreetAddress += "/" + account.HouseNumberAdd;

            // add account
            var result = await Task.Run(() => Connector.service.saveUser(
              Connector.password,
              account.AccountID,
              account.UID,
              pw1, pw2, pw3,
              account.GivenName,
              account.SurName,
              account.ExtraNames,
              account.Initials,
              gender,
              birthday,
              account.BirthPlace,
              account.BirthCountry,
              StreetAddress,
              account.PostalCode,
              account.City,
              account.Country,
              account.Mail,
              account.MobilePhone,
              account.HomePhone,
              account.Fax,
              account.RegisterID,
              stemID,
              role,
              account.UntisID
              )
            );

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Load account details from smartschool. The account must have a valid UID to begin with.
        /// All other values will be overwritten.
        /// </summary>
        /// <param name="account">The account to load.</param>
        /// <returns>Returns false when failed. Errors will be added to the log object.</returns>
        public static async Task<bool> Load(IAccount account)
        {
            var result = await Task.Run(
              () => Connector.service.getUserDetails(Connector.password, account.UID)
            );

            try
            {
                JSONAccount details = JsonConvert.DeserializeObject<JSONAccount>(result);
                LoadFromJSON(account, details);
                return true;
            }
            catch (Exception e)
            {
                Error.AddError(e.Message);

                int iResult = Convert.ToInt32(result);
                if (iResult != 0)
                {
                    Error.AddError(iResult);
                    return false;
                }
                return false;
            }
        }

        public static async Task LoadAccounts(IGroup group)
        {
            List<IGroup> list = new List<IGroup>();
            group.GetTreeAsList(list);

            List<Task> TaskList = new List<Task>();

            for (int i = 0; i < list.Count; i++)
            {
                TaskList.Add(list[i].LoadAccounts());
            }
            await Task.Run(
              () => Task.WaitAll(TaskList.ToArray())
            );


        }



        public static async Task<bool> SetPassword(IAccount account, string password, AccountType type)
        {
            var result = await Task.Run(
              () => Connector.service.savePassword(Connector.password, account.UID, password, (int)type)
            );

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }

            return true;
        }

        public static async Task<bool> ForcePasswordReset(IAccount account, AccountType type)
        {
            var result = await Task.Run(
              () => Connector.service.forcePasswordReset(Connector.password, account.UID, (int)type)
            );

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }

            return true;
        }

        public static async Task<bool> UnregisterStudent(IAccount account, DateTime dateOfChange)
        {
            string changedate = Utils.DateToString(dateOfChange);

            var result = await Task.Run(() =>
              Connector.service.unregisterStudent(Connector.password, account.UID, changedate)
            );
            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }

            return true;
        }

        public static async Task<bool> ChangeUID(IAccount account)
        {
            var result = await Task.Run(
              () => Connector.service.changeUsername(Connector.password, account.AccountID, account.UID)
            );

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }

            return true;
        }

        public static async Task<bool> ChangeAccountID(IAccount account)
        {
            var result = await Task.Run(
              () => Connector.service.changeInternNumber(Connector.password, account.UID, account.AccountID)
            );

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }

            return true;
        }

        public static async Task<bool> SetStatus(IAccount account, AccountState state)
        {
            string status;
            switch (state)
            {
                case AccountState.Active:
                    status = "active";
                    break;
                case AccountState.Inactive:
                    status = "inactive";
                    break;
                case AccountState.Administrative:
                    status = "administrative";
                    break;
                default:
                    status = "invalid";
                    break;
            }

            var result = await Task.Run(
              () => Connector.service.setAccountStatus(Connector.password, account.UID, status)
            );

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }

            return true;
        }

        public static async Task<AccountState> GetStatus(IAccount account)
        {
            var result = await Task.Run(
              () => Connector.service.getUserDetails(Connector.password, account.UID)
            );

            try
            {
                JSONAccount details = JsonConvert.DeserializeObject<JSONAccount>(result);
                switch (details.status)
                {
                    case "actief":
                    case "active":
                    case "enabled":
                        return AccountState.Active;
                    case "uitgeschakeld": // yes, this correct. Even though you use inactief, inactive or disabled to set this status!
                        return AccountState.Inactive;
                    case "administrative":
                    case "administratief":
                        return AccountState.Administrative;
                    default:
                        return AccountState.Invalid;
                }

            }
            catch (Exception e)
            {
                Error.AddError(e.Message);

                int iResult = Convert.ToInt32(result);
                if (iResult != 0)
                {
                    Error.AddError(iResult);
                    return AccountState.Invalid;
                }
                return AccountState.Invalid;
            }
        }

        public static async Task<bool> Delete(IAccount account)
        {
            return await Delete(account, DateTime.MinValue);
        }

        public static async Task<bool> Delete(IAccount account, DateTime dateOfChange)
        {
            string changeDate = "1-1-1";
            if (dateOfChange != DateTime.MinValue)
            {
                changeDate = Utils.DateToString(dateOfChange);
            }

            var result = await Task.Run(
                () => Connector.service.delUser(Connector.password, account.UID, changeDate)
            );

            int iResult = Convert.ToInt32(result);
            if (iResult != 0)
            {
                Error.AddError(iResult);
                return false;
            }
            return true;
        }

        internal static void LoadFromJSON(IAccount account, JSONAccount json)
        {
            account.UID = json.gebruikersnaam;
            account.AccountID = json.internnummer;
            account.RegisterID = json.rijksregisternummer;

            if (json.basisrol == "1")
            {
                account.Role = AccountRole.Student;
            }
            else if (json.basisrol == "2")
            {
                account.Role = AccountRole.Teacher;
            }
            else if (json.basisrol == "3")
            {
                account.Role = AccountRole.Director;
            }

            account.GivenName = json.voornaam;
            account.SurName = json.naam;
            account.ExtraNames = json.extravoornamen;
            account.Initials = json.initialen;

            if (json.geslacht.Equals("m"))
            {
                account.Gender = GenderType.Male;
            }
            else if (json.geslacht.Equals("f"))
            {
                account.Gender = GenderType.Female;
            }
            else
            {
                account.Gender = GenderType.Transgender;
            }

            account.Birthday = Utils.StringToDate(json.geboortedatum);
            account.BirthPlace = json.geboorteplaats;
            account.BirthCountry = json.geboorteland;

            account.Street = json.straat;
            account.HouseNumber = json.huisnummer;
            account.HouseNumberAdd = json.busnummer;
            account.PostalCode = json.postcode;
            account.City = json.woonplaats;

            account.MobilePhone = json.mobielnummer;
            account.HomePhone = json.telefoonnummer;
            account.Fax = json.fax;
            account.Mail = json.emailadres;
        }
    }
}
