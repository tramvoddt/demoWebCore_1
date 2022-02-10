using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Utils
{
    public class Helpers
    {
        public static string RandomCode()
        {
            return DateTime.Now.Ticks.ToString();
        }
        public static string ProductName(string value)
        {
            if (value.Length <= 25)
            {
                return value;
            }
            char[] a = value.ToArray();
            string result = "";
            for (int i = 0; i < a.Length; i++)
            {
                result += a[i];
                if (i == 24)
                {
                    result = result + "...";
                    break;
                }
            }
            return result;

        }
    }
}
