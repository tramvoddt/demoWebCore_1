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
using System.IO;
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
            TempData["title"] = "Approval of posts";
            TempData["cate"] = "approval";
            ViewBag.data = LoadData(page,-1);
            ViewBag.pagi = RowEvent(_postService.GetPosts().Count);
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
        public List<Post> LoadData(int p, int status)
        {
            int currentSkip = 10 * (p - 1);
            if (status == -1)
            {
                return _postService.GetPosts().OrderByDescending(x => x.id).Skip(currentSkip).Take(10).ToList();

            }
            else if (status == 0)
            {
                return _postService.GetPosts().OrderByDescending(x => x.id).Where(x=>x.status==false).Skip(currentSkip).Take(10).ToList();

            }
            return _postService.GetPosts().OrderByDescending(x => x.id).Where(x => x.status == true).Skip(currentSkip).Take(10).ToList();


        }
        public int GetCount(int status)
        {
            if (status == -1)
            {
                return _postService.GetPosts().ToList().Count;

            }
            else if (status == 0)
            {
                return _postService.GetPosts().Where(x => x.status == false).ToList().Count;

            }
            return _postService.GetPosts().Where(x => x.status == true).ToList().Count;


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
            public IActionResult ChangeAvt()
            {
            if (AuthRequest.id == 0)
            {
                return RedirectToAction("Auth", "Auth", new { type = "login" });
            }
           
            return View();
            }
        public List<Post> GetListPost()
        {
            Services.ServiceClient cli = new Services.ServiceClient();
            var val = cli.GetListPostAsync(AuthRequest.id);
            var res = val.Result;
            List<Post> list = new List<Post>();
            list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Post>>(res);
            return list;
        }
        public void SaveAvt(IFormFile f, int postID)
        {
            var u = userService.GetDataContext().Users.FirstOrDefault(x => x.id == AuthRequest.id);
            if (postID == 0)
            {
                
                    using (var ms = new MemoryStream())
                    {
                        f.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        u.avt = fileBytes;
                        
                    }

            }
            else
            {
                var p = _postService.GetPostByID(postID);
                u.avt = p.img;

            }
            AuthRequest.avt = u.avt;
            Services.ServiceClient cli = new Services.ServiceClient();
            cli.SaveAvtFileAsync(AuthRequest.id, AuthRequest.avt);

        }
        public FileContentResult GetAvtFile()
        {
           
            return new FileContentResult(GetImage(Convert.ToBase64String(AuthRequest.avt)), "image/jpg");

        }


        public byte[] GetImage(string base64)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(base64))
            {
                bytes = Convert.FromBase64String(base64);
            }
            return bytes;
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
        public IActionResult ReportComment()
        {
            TempData["title"] = "Report Comment";
            ViewBag.data = _postService.GetReports();
            ViewBag.psrv = _postService;
            TempData["cate"] = "reports";
            ViewBag.data = LoadDataReport(page, -1, -1);
            ViewBag.pagi = RowEvent(_postService.GetReports().Count);
            return View();
        }
        public List<Reports> LoadDataReport(int p, int status, int sort)
        {
            int currentSkip = 10 * (p - 1);
            List<Reports> ls = new List<Reports>();
            switch (status)
            {
                case -1:
                    ls = _postService.GetReports().Skip(currentSkip).Take(10).ToList();
                    break;
                case 0:
                    ls = _postService.GetReports().Where(x => x.status == 0).Skip(currentSkip).Take(10).ToList();
                    break;
                case 2:
                    ls = _postService.GetReports().Where(x => x.status == 2).Skip(currentSkip).Take(10).ToList();
                    break;
                default:
                    ls = _postService.GetReports().Where(x => x.status == 1).Skip(currentSkip).Take(10).ToList();
                    break;

            }
            switch (sort)
            {
                case 1:
                    return ls.OrderBy(x => x.total).ToList();
                case 0:
                    return ls.OrderByDescending(x => x.total).ToList();
                default:
                    return ls.OrderByDescending(x => x.id).ToList();
            }

        }
        public int GetCountReport(int status, int sort)
        {
            List<Reports> ls = new List<Reports>();
            switch (status)
            {
                case -1:
                    ls = _postService.GetReports().ToList();
                    break;
                case 0:
                    ls = _postService.GetReports().Where(x => x.status == 0).ToList();
                    break;
                case 2:
                    ls = _postService.GetReports().Where(x => x.status == 2).ToList();
                    break;
                default:
                    ls = _postService.GetReports().Where(x => x.status == 1).ToList();
                    break;

            }
            switch (sort)
            {
                case 1:
                    return ls.OrderBy(x => x.total).ToList().Count;
                case 0:
                    return ls.OrderByDescending(x => x.total).ToList().Count;
                default:
                    return ls.OrderByDescending(x => x.id).ToList().Count;
            }

        }
        public string GetUserByCmtID(int id, string type)
        {
            switch (type)
            {
                case "code":
                    return _postService.GetUserByCmtID(id).code;

                default:
                    return _postService.GetUserByCmtID(id).name;

            }
        }
        public string GetContentComment(int id)
        {
            return _postService.GetDataContext().Comment.FirstOrDefault(x => x.id == id).content;
        }
        public int GetPostID(int id)
        {
            return _postService.GetPostByCmtID(id).id;
        }
    }
}
