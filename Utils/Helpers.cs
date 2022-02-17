using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Utils
{
    public class Helpers
    {
        public static DataContext dt;
        public Helpers(DataContext db)
        {
            dt = db;
        }
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
        public static string GetUserName(int id)
        {
            return dt.Users.FirstOrDefault(x => x.id == id).name;
        }
    

    }
}
