using demoWebCore_1.Models;
using demoWebCore_1.Models.BusinessPattern;
using demoWebCore_1.Models.ModelViews;
using demoWebCore_1.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public IActionResult ClientLogin()
        {
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
