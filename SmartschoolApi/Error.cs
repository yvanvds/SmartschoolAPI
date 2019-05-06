using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartschoolApi
{
    internal static class Error
    {
        private static JObject errorCodes;

        internal static void GetCodes()
        {
            if (errorCodes != null) return;

            try
            {
                var result = Connector.service.returnJsonErrorCodes();
                errorCodes = JObject.Parse(result);

            }
            catch (Exception e)
            {
                AddError(e.Message);
            }
        }

        internal static string ToString(int error)
        {
            try
            {
                return errorCodes[error.ToString()].ToString();
            }
            catch (Exception e)
            {
                AddError(e.Message);
            }
            return "Invalid Error";
        }

        internal static void AddError(int error)
        {
            AddError(ToString(error));
        }

        internal static void AddError(string error)
        {
            if (Connector.log != null)
            {
                Connector.log.AddError("Smartschool Error: " + error);
            }
        }

        internal static void AddMessage(string message)
        {
            if (Connector.log != null)
            {
                Connector.log.AddError("Smartschool Message: " + message);
            }
        }
    }
}
