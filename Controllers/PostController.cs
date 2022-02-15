using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using demoWebCore_1.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using demoWebCore_1.IService;
using demoWebCore_1.Models.ModelViews;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace demoWebCore_1.Controllers
{
    public class PostController : Controller
    {
        IPostService _postService =null;
        ICollectService _collectService =null;
        public PostController(IPostService p,ICollectService c)
        {
            _postService = p;
            _collectService = c;
        }
        public IActionResult Index()
        {
            if (AuthRequest.id == 0)
            {
                return RedirectToAction("Auth", "Auth",new {type="login",page="post" });
            }
            TempData["bgc"] = "#17bb7e";
            TempData["listCollect"] = _collectService.GetCollectionByUserID(AuthRequest.id);
            return View();
        }
      
        [HttpPost]
        public void SavePost(FileUpload f)
        {
           Post obj = JsonConvert.DeserializeObject<Post>(f.Post);
            if (f.file.Length > 0)
            {
                using (var ms=new MemoryStream())
                {
                    f.file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    obj.img = fileBytes;
                    obj.created_at = DateTime.Now;
                    obj.user_id = AuthRequest.id;
                    obj = _postService.Save(obj);
                    


                }
            }
        }

        [HttpGet]
        public JsonResult GetPost(int id)
        {
            var p = _postService.GetPostByID(id);
            p.img = GetImage(Convert.ToBase64String(p.img));
           
            return Json(p);

        }
        public FileContentResult GetImageFile(int id)
        {
            var p = _postService.GetPostByID(id);
            p.img = GetImage(Convert.ToBase64String(p.img));
            return new FileContentResult(p.img, "image/jpg");

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
       public List<Collect> GetCollectionByUserID()
        {
            return _collectService.GetCollectionByUserID(AuthRequest.id); 
        }
    }
}
