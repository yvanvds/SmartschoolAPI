using AbstractAccountApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartschoolApi
{

    public class Account : IAccount
    {
        /// <summary>
        /// The User ID, this is smartschool's username or login
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// This is used as smartschool's internal number
        /// </summary>
        public string AccountID { get; set; }

        /// <summary>
        /// The state registry number
        /// </summary>
        public string RegisterID { get; set; }

        /// <summary>
        /// Smartschool field for a Untis ID. Smartschool does not support saving this right now.
        /// </summary>
        public string UntisID { get; set; }

        /// <summary>
        /// Stamboeknummer...
        /// </summary>
        public int StemID { get; set; }

        /// <summary>
        /// The account role can be student, teacher or director. IT or Support are supported internally,
        /// but will be converted to teacher when saved to smartschool.
        /// </summary>
        public AccountRole Role { get; set; }

        /// <summary>
        /// The given/first name of the account holder
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// The last name of the account holder
        /// </summary>
        public string SurName { get; set; }

        /// <summary>
        /// Middle names of the account holder (optional)
        /// </summary>
        public string ExtraNames { get; set; }

        /// <summary>
        /// The account holder's initials (optional)
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// The account holder's gender. (Smartschool only supports male/female. Other options will be converted
        /// to female.)
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// The account holder's date of birth
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// The account holder's place of birth
        /// </summary>
        public string BirthPlace { get; set; }

        /// <summary>
        /// The account holder's country of birth
        /// </summary>
        public string BirthCountry { get; set; }

        /// <summary>
        /// The account holder's street address. In smartschool, this will be merged with house number and bus number
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// The account holder's street address number. In smartschool, this will be merged with street and bus number.
        /// </summary>
        public string HouseNumber { get; set; }

        /// <summary>
        /// The account holder's street address bus number.
        /// </summary>
        public string HouseNumberAdd { get; set; }

        /// <summary>
        /// The account holder's current address postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// The account holder's current city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The account holder's current country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The account holder's mobile phone number
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// The account holder's home phone number
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// The account holder's fax number
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// The account holder's email adress
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// The account holder's email alias (not currently used by smartschool)
        /// </summary>
        public string MailAlias { get; set; }

        /// <summary>
        /// Primary group for this account holder. (Not used by smartschool)
        /// </summary>
        public string Group { get; set; }
    }
}
