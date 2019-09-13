using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.ServiceLayer.Utils
{
    public class WBSCodeNumericComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            var rpc1 = s1.Split('-');
            var rpc2 = s2.Split('-');
            if (rpc1.Count() == rpc2.Count())
            {
                int result = -1;
                for(int i=0;i<rpc2.Count();i++)
                {
                    if (Convert.ToInt32(rpc1[i]) < Convert.ToInt32(rpc2[i]))
                    {
                        result = -1;
                    }

                    if (Convert.ToInt32(rpc1[i]) > Convert.ToInt32(rpc2[i]))
                    {
                        result = 1;
                    }

                    if (Convert.ToInt32(rpc1[i]) == Convert.ToInt32(rpc2[i]))
                    {
                        result = 0;
                    }
                }
                return result;
            }
            else
            {
                if (rpc1.Count() > rpc2.Count())
                {
                    return 1;
                }

                if (rpc1.Count() < rpc2.Count())
                {
                    return -1;
                }
            }

            //if (IsNumeric(s1) && IsNumeric(s2))
            //{
            //    if (Convert.ToInt32(s1) > Convert.ToInt32(s2)) return 1;
            //    if (Convert.ToInt32(s1) < Convert.ToInt32(s2)) return -1;
            //    if (Convert.ToInt32(s1) == Convert.ToInt32(s2)) return 0;
            //}

            //if (IsNumeric(s1) && !IsNumeric(s2))
            //    return -1;

            //if (!IsNumeric(s1) && IsNumeric(s2))
            //    return 1;

            return string.Compare(s1, s2, false);
        }

        public static bool IsNumeric(object value)
        {
            try
            {
                int i = Convert.ToInt32(value.ToString());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
