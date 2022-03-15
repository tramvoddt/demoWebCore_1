using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace demoWebCore_1.Service
{
    public class PostService : IPostService
    {
        private readonly DataContext ct;
        public PostService(DataContext context)
        {
            ct = context;
        }
        public Post Save(Post u)
        {
            ct.Post.Add(u);
            int res = ct.SaveChanges();
            if (res > 0)
            {
                return u;
            }
            else return null;
        }
        public DataContext GetDataContext()
        {
            return ct;
        }
        public Post GetPostByID(int id)
        {
            var post = ct.Post.FirstOrDefault(x => x.id == id);

            return post ?? null;
        }
        public List<Post> GetPosts()
        {
            List<Post> p = new List<Post>();
            var post = ct.Post.OrderByDescending(x => x.id).ToList();
            foreach (var item in post)
            {
                var collect = ct.Collect.FirstOrDefault(x => x.id == item.collection_id);
                if (collect.status == false)
                {
                    p.Add(item);
                }


            }

            return p ?? null;
        }
        public bool CheckStatusCollection(int collect_id)
        {
            var post = ct.Collect.FirstOrDefault(x => x.id == collect_id);

            return post == null ? true : false;
        }

        public List<int> GetPostByCollection(int collectID)
        {

            return ct.PostOther.OrderByDescending(x => x.id).Where(x => x.collection_id == collectID).Select(x => x.post_id).ToList();


        }
        public List<Post> GetListPostByListID(List<int> ls)
        {
            List<Post> p = new List<Post>();
            foreach (var item in ls)
            {
                var post = ct.Post.FirstOrDefault(x => x.id == item);
                p.Add(post);
            }
            return p;
        }
        public bool CheckChooseCollection(int postID, int collectionID)
        {
            var q = ct.PostOther.FirstOrDefault(x => x.post_id == postID && x.user_id == AuthRequest.id && x.collection_id == collectionID);
            return q == null ? false : true;

        }
        public List<Post> GetPostByUserID(int id, int postID)
        {
            var q = ct.Post.Where(x => x.user_id == id && x.id != postID).ToList();
            return q ?? null;
        }
        public void RemovePost(int id)
        {
            var w = ct.Post.FirstOrDefault(x => x.id == id && x.user_id == AuthRequest.id);
            if (w == null)
            {
                var r = ct.PostOther.FirstOrDefault(x => x.post_id == id && x.user_id == AuthRequest.id);
                ct.Remove(r);
            }
            else
            {
                ct.Remove(w);

                List<PostOther> r = ct.PostOther.Where(x => x.post_id == id).ToList();
                ct.RemoveRange(r);
            }
            ct.SaveChanges();
        }
        public void UpdatePost(List<Post> l, bool sts)
        {
            foreach (var item in l)
            {
                item.status = sts;
                ct.SaveChanges();
            }

        }
        public void UpdateAllPost(bool val)
        {
            foreach (var item in GetPosts())
            {
                if (item.status != val)
                {
                    item.status = val;
                    ct.SaveChanges();
                }

            }
        }
        public bool ReportComment(int cmt_id)
        {
            if (CheckUserReport(cmt_id))
            {
                return false;
            }
            else
            {
                var w = ct.Reports.FirstOrDefault(x => x.cmt_id == cmt_id);
                if (w == null)
                {
                    ct.Reports.Add(new Reports() { id = AuthRequest.id, cmt_id = cmt_id, list_user = "[]", total = 1 });

                }
                else {
                    List<UserReport> l = new List<UserReport>();
                    l = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserReport>>(w.list_user);
                    l.Add(new UserReport() { id = AuthRequest.id, created_at = DateTime.Now });
                    w.total += 1;
                    w.list_user = Newtonsoft.Json.JsonConvert.SerializeObject(l);

                }
                ct.SaveChanges();

                return true;

            }


        }
        public bool CheckUserReport(int cmtID)
        {
            var w = ct.Reports.FirstOrDefault(x => x.cmt_id == cmtID);
            if (w != null)
            {
                List<UserReport> ls = new List<UserReport>();
                ls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserReport>>(w.list_user);
                foreach (var item in ls)
                {
                    if (item.id == AuthRequest.id)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

    }
}
