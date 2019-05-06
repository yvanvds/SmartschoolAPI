using AbstractAccountApi;
using SmartschoolApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolTests
{
    class TestAccounts
    {
        public static Account Teacher()
        {
            Account result = new Account();
            result.UID = "UnitTeacher";
            result.AccountID = "UNITTEACHER";
            result.RegisterID = "1111111111";
            result.StemID = 0;//1001234;
            result.Role = AccountRole.Teacher;

            result.GivenName = "Unit";
            result.SurName = "Test";
            result.ExtraNames = "One Two";
            result.Initials = "A. B.";
            result.Gender = GenderType.Female;

            result.BirthCountry = "Belgie";
            result.BirthPlace = "Leuven";
            result.Birthday = new DateTime(1990, 9, 1);

            result.Street = "My Street";
            result.HouseNumber = "1";
            result.HouseNumberAdd = "a";
            result.City = "Aarschot";
            result.PostalCode = "3200";
            result.Country = "Belgie";
            result.Mail = "unittest@gmail.com";

            result.MobilePhone = "0494/456789";
            result.HomePhone = "016/123456";
            result.Fax = "";
            return result;
        }

        public static Account Student()
        {
            Account result = new Account();
            result.UID = "UnitStudent";
            result.AccountID = "UNITSTUDENT";
            result.RegisterID = "1111111111";
            result.StemID = 1001234;
            result.Role = AccountRole.Student;

            result.GivenName = "Unit";
            result.SurName = "Test";
            result.ExtraNames = "One Two";
            result.Initials = "A. B.";
            result.Gender = GenderType.Male;

            result.BirthCountry = "Belgie";
            result.BirthPlace = "Leuven";
            result.Birthday = new DateTime(1990, 9, 1);

            result.Street = "My Street";
            result.HouseNumber = "1";
            result.HouseNumberAdd = "a";
            result.City = "Aarschot";
            result.PostalCode = "3200";
            result.Country = "Belgie";
            result.Mail = "unittest@gmail.com";

            result.MobilePhone = "0494/456789";
            result.HomePhone = "016/123456";
            result.Fax = "";
            return result;
        }
    }
}

