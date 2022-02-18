using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Controllers
{
    public class CollectController : Controller
    {
        ICollectService collectService = null;
        IPostService postService = null;
        IPostOtherService postOtherService = null;
        public CollectController(ICollectService db, IPostService p, IPostOtherService po)
        {

            collectService = db;
            postService = p;
            postOtherService = po;
        }
        [HttpPost]
        public int SaveCollect(Collect f)
        {
        
                f.created_at = DateTime.Now;
                f.user_id = AuthRequest.id;
                collectService.Save(f);
            var w = collectService.GetDataContext().Collect.OrderByDescending(x=>x.id).FirstOrDefault(x => x.user_id == AuthRequest.id);
         
            return  w.id;

        }
        public IActionResult CollectionDetail(int cID)
        {
            TempData["listCollect"] = collectService.GetCollectionByUserID(AuthRequest.id);
            ViewBag.collectName = collectService.GetCollectionName(cID);
            ViewBag.post = postService.GetListPostByListID(postService.GetPostByCollection(cID));
            ViewBag.postService =postService;
            return View();
        }
        //CHECKDUP
        public bool NameExists(string name)
        {
            return collectService.NameExists(name);

        }

    }
}
