using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartschoolApi
{
    /// <summary>
    /// This class contains a few utilities which are used for convenience.
    /// </summary>
    static public class Utils
    {
        /// <summary>
        /// Converts a Date to the String format expected by smartschool.
        /// </summary>
        /// <param name="value">The DateTime object to convert</param>
        /// <returns>A date string with format year-month-day</returns>
        static public string DateToString(DateTime value)
        {
            if (value == null) return "";
            return value.Year.ToString() + "-" + value.Month.ToString() + "-" + value.Day.ToString();
        }

        /// <summary>
        /// Converts a smartschool date string to a DateTime object.
        /// </summary>
        /// <param name="value">The string, with format year-month-day</param>
        /// <returns>A DateTime object representing the same day.</returns>
        static public DateTime StringToDate(string value)
        {
            string[] values = value.Split('-');
            if (values.Count() != 3)
            {
                return DateTime.MinValue;
            }

            try
            {
                return new DateTime(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), Convert.ToInt32(values[2]));
            }
            catch (Exception e)
            {
                Error.AddError(e.Message);
                return DateTime.MinValue;
            }


        }

        /// <summary>
        /// Checks if two DateTime objects refer to the same day. Year, Month and Day are
        /// evaluated, but differences in Hours and lower units are discarded.
        /// </summary>
        /// <param name="a">The reference Date</param>
        /// <param name="b">The date to compare</param>
        /// <returns>True, if Year, Month and Day are the same. Otherwise false.</returns>
        static public bool SameDay(DateTime a, DateTime b)
        {
            if (a.Year != b.Year) return false;
            if (a.Month != b.Month) return false;
            if (a.Day != b.Day) return false;
            return true;
        }
    }
}
