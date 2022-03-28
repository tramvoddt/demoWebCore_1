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

                    List<int> ls_user = new List<int>();
                    ls_user.Add(AuthRequest.id);
                    ct.Reports.Add(new Reports() { cmt_id = cmt_id, list_user = Newtonsoft.Json.JsonConvert.SerializeObject(ls_user), total = 1 });

                }
                else {
                    List<int> l = new List<int>();
                    l = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(w.list_user);
                    l.Add(AuthRequest.id);
                    w.total += 1;
                    w.list_user = Newtonsoft.Json.JsonConvert.SerializeObject(l);
                    if (w.status == 2)
                    {
                        w.status = 0;
                    }

                    

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
                List<int> ls = new List<int>();
                ls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(w.list_user);
                if (ls.Contains(AuthRequest.id))
                {
                    return true;
                }
            }
            return false;

        }
        public List<Reports> GetReports()
        {
            return ct.Reports.OrderByDescending(x => x.id).ToList()??null;
        }
        public Users GetUserByCmtID(int cmtID)
        {
            var w = ct.Users.FirstOrDefault(x => x.id == ct.Comment.FirstOrDefault(x => x.id == cmtID).user_id);
            return w;
        }
        public Post GetPostByCmtID(int cmtID)
        {
            return ct.Post.FirstOrDefault(x => x.id == (ct.Comment.FirstOrDefault(x => x.id == cmtID).post_id));
        }
        public void UpdateReport(List<Reports> l, int sts)
        {
            foreach (var item in l)
            {
                if (item.status != 2 && item.status != 1)
                {
                    item.status = sts;
                }
                if (sts == 1)
                {
                    var cmt = ct.Comment.FirstOrDefault(x => x.id == item.cmt_id);
                    if (cmt != null)
                    {
                        cmt.status = true;
                    }
                }
                ct.SaveChanges();
            }

        }
        public List<Reports> GetListReportByListInt(List<int> ls)
        {
            List<Reports> p = new List<Reports>();
            foreach (var item in ls)
            {
                var post = ct.Reports.FirstOrDefault(x => x.id == item);
                p.Add(post);
            }
            return p;

        }
        public string CheckSelected(int postID)
        {
            var q = ct.Collect.Where(x => x.user_id == AuthRequest.id).ToList();
            var p = ct.Post.FirstOrDefault(x => x.id == postID);
            string res = "<option style='color:black' value='-1'>Choose</option> <option style='color:black' value='0'>Create</option>";
            foreach (var item in q)
            {
                if((p.collection_id == item.id && p.user_id == AuthRequest.id) || CheckChooseCollection(p.id, item.id))
                {
                   res+="<option style='color:black' selected='selected' value="+item.id+">"+item.name +"-"+(item.status == true ? "Private" : "Public")+"</option>'";

                }
                else
                {
                    res += "<option style='color:black' value=" + item.id + ">" + item.name + "-" + (item.status == true ? "Private" : "Public") + "</option>'"; 


                }
            }
            return res+="</select>";
              
        }
    }
}
