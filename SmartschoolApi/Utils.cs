using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartschoolApi
{
    static internal class Utils
    {
        static public string DateToString(DateTime value)
        {
            if (value == null) return "";
            return value.Year.ToString() + "-" + value.Month.ToString() + "-" + value.Day.ToString();
        }

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

        static public bool SameDay(DateTime a, DateTime b)
        {
            if (a.Year != b.Year) return false;
            if (a.Month != b.Month) return false;
            if (a.Day != b.Day) return false;
            return true;
        }
    }
}
