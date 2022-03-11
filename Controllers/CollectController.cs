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
        public static int cid;
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
            TempData["MessageCollect"] = "Create successfully!";

            return w.id;

        }
        public Collect GetCollection(int id)
        {
            cid = id;
            var q = collectService.GetCollect(id);
            return q;
        }
        public void DeleteCollection(int id)
        {
            collectService.DeleteCollect(id);
            TempData["MessageCollect"] = "Delete successfully!";
          
        }
        public int SaveCollectUpdate(Collect f)
        {
            f.id = cid;
            var q = collectService.GetCollect(f.id);
            if (q != null)
            {
                if (q.name == f.name && q.status == f.status)
                {
                    return 0;
                }
                q.name = f.name;
                q.status = f.status;
                collectService.GetDataContext().SaveChanges();
                return q.id;
            }
            return 0;
        }
        public IActionResult CollectionDetail(int cID)
        {
            cid = cID;
            TempData["listCollect"] = collectService.GetCollectionByUserID(AuthRequest.id);
            ViewBag.collectName = collectService.GetCollectionName(cID);
            ViewBag.post = postService.GetListPostByListID(postService.GetPostByCollection(cID));
            ViewBag.postService =postService;
            return View();
        }
        public int RemovePost(int id)
        {
            postService.RemovePost(id);
            return cid;

        }
        //CHECKDUP
        public bool NameExists(string name)
        {
            return collectService.NameExists(name);

        }
        public bool NameExistsUpdate(string name, int id)
        {
            id = cid;
            return collectService.NameExists(name,id);

        }

    }
}
