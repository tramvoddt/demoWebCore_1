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
using System.Text;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using demoWebCore_1.Utils;

namespace demoWebCore_1.Controllers
{
    public class PostController : Controller
    {
        IPostService _postService =null;
        ICollectService _collectService =null;
        IPostOtherService _postOtherService =null;
        ICommentService _commentService =null;
        IReactService _reactService =null;
        public static int k,j;
        public static List<Post>[] count= { };
        public static int c= 1, check=1;
        public PostController(IPostService p,ICollectService c,IPostOtherService po, ICommentService cmt, IReactService r)
        {
            _postService = p;
            _postOtherService = po;
            _collectService = c;
            _commentService = cmt;
            _reactService = r;
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
            ViewBag.reactService = _reactService;
            if (AuthRequest.id == 0)
            {
                TempData["listCollect"] = null;
            }
            else
            {
                TempData["listCollect"] = _collectService.GetCollectionByUserID(AuthRequest.id);
            }
            var w = _postService.GetPostByID(id);
            ViewBag.data =_postService.GetPostByUserID(w.user_id,id);
            TempData["bgc"] = "#d3e3dc";
            ViewBag.listComment = _commentService.GetListComment(id);
            ViewBag.model = w;

            return View();
        }
        
        public List<Comment> LoadMoreCmt(int postID, int value)
        {
            var skip = _commentService.GetListComment(postID).Count - value*3;
            int take = 3;
            if (skip < 0)
            {
                if (k == 0)
                {
                    k++;
                    take = skip + 3;
                }
                else if(k==1) { return null; }
                
            }
           
            return _commentService.GetListComment(postID).Skip(skip).Take(take).ToList();
        }
        public int GetReactByCmt(int cID)
        {
            return _reactService.GetReactByCmt(cID).total;
        }
        public bool CheckUserReact(int cID)
        {
            return _reactService.CheckUserReact(cID);
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
                    _postService.GetDataContext().SaveChanges();
                    

                }

            }
            else if (r != null)
            {
                if (r.collection_id != collect_id)
                {
                    r.collection_id = collect_id;
                    _postService.GetDataContext().SaveChanges();
                   

                }
            }
            if (r == null && q == null)
            {
                _postOtherService.SavePost(new PostOther() { user_id = AuthRequest.id, post_id = post_id, collection_id = collect_id, created_at = DateTime.Now });
            }
            return true;

        }
        
        public Comment CommentAction(int postID,string content)
        {
            Comment c = _commentService.SaveComment(new Comment() { post_id=postID,user_id=AuthRequest.id,content=content,created_at=DateTime.Now });
            _reactService.CreateReact(new React() { cmt_id = c.id, listUser = "[]", total = 0 });
            return c;
        }
        public void ReactAction(int cmtID, int value)
        {
            _reactService.ReactAction(cmtID, value);
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
        public void DownloadImage(int id)
        {
            using (WebClient webClient = new WebClient())
            {
                string val = "http://localhost:20194/Post/GetImageFile?id="+id;
                var w = _postService.GetPostByID(id).subject.Replace(" ","-")+"-";

                byte[] data = webClient.DownloadData(val);

                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var yourImage = Image.FromStream(mem))
                    {
                        string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads" + @"\" +w+ Helpers.RandomCode() + ".jpg";
                        yourImage.Save(download, ImageFormat.Jpeg);
                    }
                }
            }
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
        public bool HideAndUnhideComment(int cid, int status)
        {
            try
            {
                _commentService.UpdateComment(new Comment (){ id = cid, hide = status == 1 ? true : false });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }
        public bool DeleteComment(int id)
        {
            try
            {
                var c=_commentService.GetDataContext().Comment.FirstOrDefault(x=>x.id==id);
                c.status = true;
                _commentService.GetDataContext().SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EditComment(int id, string content)
        {
            try
            {
                _commentService.Edit(id, content);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public void UpdatePost(List<int> ls,int sts)
        {
            List<Post> l = _postService.GetListPostByListID(ls);
            _postService.UpdatePost(l, sts == 0 ? false : true);
        }
        public void UpdateAllPost(int val)
        {
            _postService.UpdateAllPost(val==1?true:false);
        }
        public Post GetPostByID(int pid)
        {
            return _postService.GetPostByID(pid)??null;
        }
        public bool ReportComment( int cmt_id)
        {
            return _postService.ReportComment(cmt_id);
        }
        public void UpdateReport(List<int> ls, int sts)
        {
            List<Reports> l = _postService.GetListReportByListInt(ls);
            _postService.UpdateReport(l, sts);
        }
        public List<Post> LazyLoadPost()
        {
           
                var skip =  c * 20;
                int take = 20;
            c++;
                var ls= _postService.GetPosts().Where(x => x.status == true).Skip(skip).Take(take).ToList();
            return ls;

        }
        public bool CheckLoad(List<Post> ls)
        {
            
            foreach (var item in count)
            {
                if (item == ls)
                {
                    return true;
                   
                }

            }
            return false;
        }
    }
}
