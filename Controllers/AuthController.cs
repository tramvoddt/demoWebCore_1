using demoWebCore_1.Models;
using demoWebCore_1.Models.BusinessPattern;
using demoWebCore_1.Models.ModelViews;
using demoWebCore_1.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Controllers
{
    public class AuthController : Controller
    {
        /* 
         * CLIENT
         */
        public readonly DataContext dt;
        public AuthController(DataContext db)
        {
            
            dt = db;
        }
       
        public IActionResult Auth(string type) //login and sign-up
        {
            TempData["type-auth"] = type;
            if (type == "sign-up")
            {
                TempData["user"] = "";
                TempData["pass"] = "";
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUpAction(Users u)
        {
            if (ModelState.IsValid)
            {
                u.code = Helpers.RandomCode();
                u.created_at = DateTime.Now;
                u.password = BCrypt.Net.BCrypt.HashPassword(u.password);
                Repositories.NewUser(new Models.ModelViews.Users() { name = u.name, code = u.code, phone = u.phone, created_at = u.created_at, status = true, password = u.password, email = u.email, role_id = 2 },dt) ;
               
                TempData["Message"] = "Successfully registered. Login now!";
            }

            ModelState.Clear();


            return RedirectToAction("Auth", "Auth", new { type="login" });
        }
        //LOGIN
        [HttpPost]
        public IActionResult ClientLogin()
        {
            if (ModelState.IsValid)
            {
                string uName = Request.Form["username"];
                string psw = Request.Form["psw"];
                var str = JsonConvert.SerializeObject(UserSingle.LoginAction(uName, psw, dt));
                HttpContext.Session.SetString("auth", str);  
                if (UserSingle.LoginAction(uName, psw, dt) is null)
                {
                    TempData["ErrorLogin"] = "Invalid login, please try again";
                    TempData["user"] = uName;
                   TempData["pass"] = psw;
                    return RedirectToAction("Auth","Auth",new { type="login"});
                }
                var str1 = HttpContext.Session.GetString("auth");
                var obj = JsonConvert.DeserializeObject<Users>(str1);
                AuthRequest.id = obj.id;
                AuthRequest.name = obj.name;
                AuthRequest.roleId = (int)obj.role_id;

            }
            ModelState.Clear();
            return RedirectToAction("Index", "Home");

        }
        //CHECK DUP
        public JsonResult PhoneExists(Users model)
        {
            bool phoneExists = !dt.Users.Any(x => x.phone == model.phone);
            return Json(phoneExists);

        }
        public JsonResult EmailExists(Users model)
        {
            bool emailExists = !dt.Users.Any(x => x.email == model.email);
            return Json(emailExists);
        }

    
    //LOGOUT
    public IActionResult ClientLogout()
        {
            HttpContext.Session.SetString("auth", ""); 
            AuthRequest.ClearSession();
            return RedirectToAction("Index", "Home");
        }

            /*
             * ADMIN
             */
            public IActionResult LoginAdmin()
        {
            return View();
        }
    }
}
