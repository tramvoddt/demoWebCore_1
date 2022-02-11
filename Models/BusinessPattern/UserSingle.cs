using demoWebCore_1.Models.ModelViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models.BusinessPattern
{
    public class UserSingle // AuthController
    {
        private static UserSingle _UserSingle = null;

        private UserSingle()

        {
        }

        public static UserSingle Instance
        {
            get
            {
                if (_UserSingle == null)
                {
                    _UserSingle = new UserSingle();

                }
                return _UserSingle;
            }
        }
        
        public bool CreateUser(Users newTtem, DataContext dt)
        {
            dt.Users.Add(newTtem);
            int res = dt.SaveChanges();
            if (res > 0)
            {
                return true;
            }
            else return false;
        }

        public List<ModelViews.Users> GetAllUsers(DataContext dt)
        {
            var u = dt.Users.FromSqlRaw("select * from users").ToList();
            return u;
        }
        public static Users LoginAction(string user, string pass, DataContext dt)
        {

            Users d = dt.Users.FirstOrDefault(x => (x.phone == user || x.email == user) && x.status == true);

            if (d != null && BCrypt.Net.BCrypt.Verify(pass, d.password) == true)
            {
                Users item = new Users { id = d.id, name = d.name, code = d.code, email = d.email, password = d.password, phone = d.phone, role_id = (int)d.role_id, created_at = (DateTime)d.created_at, status = (bool)d.status };
                AuthRequest.id = item.id;
                AuthRequest.name = item.name;
                AuthRequest.roleId = (int)item.role_id;
                return item;
            }

            return null;
        }


    }
}
