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
        public bool SaveComment(Comment c)
        {
            ct.Comment.Add(c);
            ct.SaveChanges();
            return true;
        }
       public  List<Comment> GetListComment(int postID)
        {
            return ct.Comment.Where(x => x.post_id == postID).ToList() ?? null;
        }

    }
}
