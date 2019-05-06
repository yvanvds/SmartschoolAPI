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
        public string UID { get; set; }
        public string AccountID { get; set; }
        public string RegisterID { get; set; }
        public string UntisID { get; set; }
        public int StemID { get; set; }
        public AccountRole Role { get; set; }
        public string GivenName { get; set; }
        public string SurName { get; set; }
        public string ExtraNames { get; set; }
        public string Initials { get; set; }
        public GenderType Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string BirthPlace { get; set; }
        public string BirthCountry { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string HouseNumberAdd { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string MailAlias { get; set; }

        public string Group { get; set; }
    }
}
