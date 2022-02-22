﻿using Microsoft.AspNetCore.Mvc;
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
using System.Text;

namespace demoWebCore_1.Controllers
{
    public class PostController : Controller
    {
        IPostService _postService =null;
        ICollectService _collectService =null;
        IPostOtherService _postOtherService =null;
        public PostController(IPostService p,ICollectService c,IPostOtherService po)
        {
            _postService = p;
            _postOtherService = po;
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
                    Debug.WriteLine(obj.url);
                    obj = _postService.Save(obj);
                    _postOtherService.SavePost(new PostOther() { post_id = obj.id, user_id = obj.user_id, collection_id = obj.collection_id, created_at = obj.created_at });

                }
            }
        }
        public IActionResult PostDetail(int id)
        {
            ViewBag.postService = _postService;
            if (AuthRequest.id == 0)
            {
                TempData["listCollect"] = null;
            }
            else
            {
                TempData["listCollect"] = _collectService.GetCollectionByUserID(AuthRequest.id);
            }
            var w = _postService.GetPostByID(id);
            ViewBag.data =_postService.GetPostByUserID(w.user_id, id);
            TempData["bgc"] = "#d3e3dc";
            ViewBag.model = w;

            return View();
        }
        public bool SavePostOther(int post_id, int collect_id)
        {
            var q = _postService.GetDataContext().Post.FirstOrDefault(x => x.id == post_id && x.user_id == AuthRequest.id);
            var r = _postService.GetDataContext().PostOther.FirstOrDefault(x => x.post_id == post_id && x.user_id == AuthRequest.id);
            if (q != null)
            {
                if (q.collection_id != collect_id)
                {
                    q.collection_id = collect_id;
                    r.collection_id = collect_id;

                    _postService.GetDataContext().SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }

            }
             _postOtherService.SavePost(new PostOther() { user_id = AuthRequest.id, post_id = post_id, collection_id = collect_id, created_at=DateTime.Now });
            return true;

        }
        [HttpPost]
        public string GetUrl(int id)
        {
            return _postService.GetPostByID(id).url;
        }
        [HttpGet]
        public JsonResult GetPost(int id)
        {
            var p = _postService.GetPostByID(id);
            p.img = GetImage(Convert.ToBase64String(p.img));
           
            return Json(p);

        }
        public FileResult DownloadFile(int filename)
        {
            return File("/Files/File Result.pdf", "text/plain", "File Result.pdf");
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
