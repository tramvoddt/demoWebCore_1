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

namespace demoWebCore_1.Controllers
{
    public class PostController : Controller
    {
        IPostService _postService =null;
        public PostController(IPostService p)
        {
            _postService = p;
        }
        public IActionResult Index()
        {
            @TempData["bgc"] = "#17bb7e";
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
        public byte[] GetImage(string base64)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(base64))
            {
                bytes = Convert.FromBase64String(base64);
            }
            return bytes;
        }
    }
}
