using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.BusinessPattern;
using demoWebCore_1.Models.ModelViews;
using demoWebCore_1.Service;
using demoWebCore_1.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace demoWebCore_1.Controllers
{
    public class AuthController : Controller
    {
        /* 
         * CLIENT
         */
        IUserService userService = null;
        ICollectService _collectService = null;
        IPostService _postService = null;
        IPostOtherService _postOtherService = null;
        public static string pages ="index";
        public static int page = 1;
        public AuthController(IUserService db, ICollectService c,IPostService p, IPostOtherService po)
        {

            userService = db;
            _collectService = c;
            _postOtherService = po;
            _postService = p;
        }

        public IActionResult Auth(string type, string page) //login and sign-up
        {
            pages = page;
            if (AuthRequest.id != 0)
            {

                return RedirectToAction("Index", "Home");

            }
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
               userService.Save(new Models.ModelViews.Users() { name = u.name, code = u.code, phone = u.phone, created_at = u.created_at, status = true, password = u.password, email = u.email, role_id = 2 });

                TempData["Message"] = "Successfully registered. Login now!";
            }

            ModelState.Clear();


            return RedirectToAction("Auth", "Auth", new { type = "login" });
        }
        //MANAGER ACCOUNT
        public IActionResult MyAccount()
        {
            if (AuthRequest.id == 0)
            {
                return RedirectToAction("Index", "Home");

            }
            ViewBag.post = _postService;
            return View(_collectService.GetCollectionByUserID(AuthRequest.id));
        }
        public IActionResult EditCollection()
        {
            ViewBag.data = _collectService.GetCollectionByUserID(AuthRequest.id);
            return View();
        }
      
        //LOGIN
        [HttpPost]
        public IActionResult ClientLogin()
        {
            if (ModelState.IsValid)
            {
                string uName = Request.Form["username"];
                string psw = Request.Form["psw"];
                Users u = userService.LoginAction(uName, psw);
                if (u is null)
                {
                    TempData["ErrorLogin"] = "Invalid login, please try again";
                    TempData["user"] = uName;
                    TempData["pass"] = psw;
                    return RedirectToAction("Auth", "Auth", new { type = "login" });
                }
                else
                {
                    var str = JsonConvert.SerializeObject(u);
                    HttpContext.Session.SetString("auth", str);
                    var str1 = HttpContext.Session.GetString("auth");
                    var obj = JsonConvert.DeserializeObject<Users>(str1);
                    AuthRequest.SetCurrent(obj.id, (int)obj.role_id, obj.name);
                    if (AuthRequest.roleId == 1)
                    {
                        return RedirectToAction("Dashboard", "Auth");
                    }
                }
              
            }
            ModelState.Clear();

            if (pages == "post")
            {
                pages = "";
                return RedirectToAction("Index", "Post");
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Dashboard()
        {
            ViewBag.data = LoadData(page);
            ViewBag.pagi = RowEvent(_postService.GetPosts().Count);
            ViewBag.currentPage = page;
            return View();
        }
        private static readonly Regex _regex = new Regex("[^0-9]+"); 
        public static bool IsNumber(string text)
        {
            return _regex.IsMatch(text);
        }
        public int RowEvent(int i)
        {
            
            double pagi = i/10.0;
            if (IsNumber(pagi.ToString()))
            {
                pagi = (int)pagi;

                pagi += 1;

            }
            return (int)pagi;
        }
        public List<Post> LoadData(int p)
        {
            int currentSkip = 10 * (p - 1);
            
            return _postService.GetPosts().OrderByDescending(x => x.id).Skip(currentSkip).Take(10).ToList();
            
        }
        //CHECK DUP
        public JsonResult PhoneExists(Users model)
        {
            bool phoneExists = !userService.GetDataContext().Users.Any(x => x.phone == model.phone);
            return Json(phoneExists);

        }
        public JsonResult EmailExists(Users model)
        {
            bool emailExists = !userService.GetDataContext().Users.Any(x => x.email == model.email);
            return Json(emailExists);
        }
      

        //LOGOUT
        public IActionResult ClientLogout()
        {
            HttpContext.Session.SetString("auth", "");
            AuthRequest.ClearSession();
            return RedirectToAction("Index", "Home");
        }
        public string GetUserName(int id)
        {
            return userService.GetUserName(id);
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
