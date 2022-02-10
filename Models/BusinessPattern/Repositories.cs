using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models.BusinessPattern
{
    public class Repositories
    {
       
        public static bool NewUser(ModelViews.Users u, DataContext dt)
        {
           
            return UserSingle.Instance.CreateUser(u, dt);
        }
    }
}
