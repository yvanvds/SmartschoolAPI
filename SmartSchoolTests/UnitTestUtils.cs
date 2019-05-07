using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmartSchoolTests
{
    [TestClass]
    public class UnitTestUtils
    {
        [TestMethod]
        public void TestDateToString()
        {
            var date = new DateTime(2019, 11, 9);
            var result = SmartschoolApi.Utils.DateToString(date);
            Assert.IsNotNull(result);
            Assert.IsTrue(result == "2019-11-9");
        }

        [TestMethod]
        public void TestStringToDate()
        {
            string goodDate = "2019-11-9";
            string badDate = "2019-13-40";

            var result = SmartschoolApi.Utils.StringToDate(badDate);
            Assert.AreEqual(result, DateTime.MinValue);

            result = SmartschoolApi.Utils.StringToDate(goodDate);
            Assert.AreEqual(result.Day, 9);
            Assert.AreEqual(result.Month, 11);
            Assert.AreEqual(result.Year, 2019);
        }

        [TestMethod]
        public void TestSameDay()
        {
            var date = new DateTime(2019, 4, 18);
            var wrongDay = new DateTime(2019, 4, 8);
            var wrongMonth = new DateTime(2019, 7, 18);
            var wrongYear = new DateTime(19, 4, 18);

            Assert.IsTrue(SmartschoolApi.Utils.SameDay(date, date));
            Assert.IsFalse(SmartschoolApi.Utils.SameDay(date, wrongDay));
            Assert.IsFalse(SmartschoolApi.Utils.SameDay(date, wrongMonth));
            Assert.IsFalse(SmartschoolApi.Utils.SameDay(date, wrongYear));
        }
    }
}
