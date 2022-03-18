using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Controllers
{
    public class HomeController : Controller
    {
        IPostService _postService = null;
        ICollectService _collectService = null;
        IUserService _userService = null;
        public HomeController(IPostService c,ICollectService co,IUserService u)
        {
            _postService = c;
            _collectService = co;
            _userService = u;
           /* Services.ServiceClient cli = new Services.ServiceClient();
            var val = cli.GetListPostAsync();
            var res = val.Result;
            List<Post> list = new List<Post>();
            list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Post>>(res);*/
        }

        public IActionResult Index()
        {
            TempData["type"] = "nav1";
            TempData["listCollect"] = _collectService.GetCollectionByUserID(AuthRequest.id);
            ViewBag.data = _postService.GetPosts().Where(x=>x.status==true).ToList();
            ViewBag.user = _userService;
            ViewBag.collect = _collectService;
            ViewBag.post = _postService;
            
            return View();
        }

        public IActionResult Contact()
        {
            TempData["type"] = "nav2";
             BCrypt.Net.BCrypt.HashPassword("123456");
            TempData["aaa"] = BCrypt.Net.BCrypt.Verify("123456", "$2a$11$GOXvcuJUAhq0rcN.5dzhauG0pOqIgd3OlYgKGKmjRq68Kw1Wq/1AG");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
