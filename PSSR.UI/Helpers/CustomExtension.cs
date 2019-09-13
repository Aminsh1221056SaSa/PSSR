using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers
{
    public static class CustomExtension
    {
        public static IEnumerable<Type> GetApplicationTypes()
        {
            return typeof(CustomExtension).Assembly.GetTypes().Where(t => !t.IsAbstract).ToList();
        }

        public static string ActivityBarCodeGeneration(string activityId, string projectdId, string formCode)
        {
            var gCode = "";
            int wLength = activityId.Length;

            if (wLength >= 1 && wLength<3)
            {
                string sbNation = projectdId.Substring(projectdId.Length - 2);
                string sbPhone = formCode.Substring(formCode.Length - 2);
                gCode = $"TS50{activityId}{sbNation}{sbPhone}";
            }
            else if (wLength >= 3 && wLength < 5)
            {
                string sbNation = projectdId.Substring(projectdId.Length - 2);
                string sbPhone = formCode.Substring(formCode.Length - 1);
                gCode = $"TS60{activityId}{sbNation}{sbPhone}";
            }
            else if (wLength >= 5 && wLength < 7)
            {
                string sbNation = projectdId.Substring(projectdId.Length - 1);
                string sbPhone = formCode.Substring(formCode.Length - 1);
                gCode = $"Ts70{activityId}{sbNation}{sbPhone}";
            }
            else if (wLength >= 7 && wLength < 9)
            {
                string sbNation = projectdId.Substring(projectdId.Length - 1);
                gCode = $"Ts80{activityId}{sbNation}";
            }
            else
            {
                gCode = $"TS90{activityId}";
            }

            return gCode;
        }

    }
}
