using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace demoWebCore_1.Service
{
    public class UserService:IUserService
    {
        private readonly DataContext ct;
        public UserService(DataContext context)
        {
            ct = context;
        }
        public Users LoginAction(string user, string pass)
        {
            Users d = ct.Users.FirstOrDefault(x => (x.phone == user || x.email == user) && x.status == true);

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
        public string GetUserName(int id)
        {
            return ct.Users.FirstOrDefault(x => x.id == id).name;
        }
        public bool Save(Users u)
        {
            ct.Users.Add(u);
            int res = ct.SaveChanges();
            if (res > 0)
            {
                return true;
            }
            else return false;
        }
        public DataContext GetDataContext()
        {
            return ct;
        }
    }

}
