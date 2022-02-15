using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.IService
{
    public  interface IUserService
    {
        bool Save(Users u);
        Users LoginAction(string user, string pass);
        DataContext GetDataContext();
        string GetUserName(int id);
    }
}
