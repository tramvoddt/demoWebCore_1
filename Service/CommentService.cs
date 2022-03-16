using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Service
{
    public class CommentService : ICommentService
    {
        private readonly DataContext ct;
        public CommentService(DataContext context)
        {
            ct = context;
        }
        public DataContext GetDataContext()
        {
            return ct;
        }
        public Comment SaveComment(Comment c)
        {
            c.hide = false;
            ct.Comment.Add(c);
            ct.SaveChanges();
            return c;
        }
        public Comment UpdateComment(Comment c)
        {
            var q = ct.Comment.FirstOrDefault(x => x.id == c.id);
            if (q != null)
            {
                q.hide = c.hide;
                if (!string.IsNullOrEmpty(c.content))
                {
                    q.content = c.content;
                }
                ct.SaveChanges();
            }
            return q;
        }
       public  List<Comment> GetListComment(int postID)
        {
            List<Comment> ls = new List<Comment>();
             var l = ct.Comment.Where(x => x.post_id == postID&&x.status==false).ToList();
            foreach (var item in l)
            {
                if (item.hide == true) {
                    var p = ct.Post.FirstOrDefault(x => x.id == item.post_id);
                    if (item.user_id == AuthRequest.id || p.user_id == AuthRequest.id)
                    {
                        ls.Add(item);
                    }
               }
                else
                {
                    ls.Add(item);
                }
            }
            return ls;
        }
        public void Delete(int id)
        {
            var w = ct.Comment.FirstOrDefault(x => x.id == id);
            ct.Remove(w);
            ct.SaveChanges();
        }
        public void Edit(int id, string content)
        {
            var w = ct.Comment.FirstOrDefault(x => x.id == id);
            w.content = content;
            ct.SaveChanges();
        }

    }
}
