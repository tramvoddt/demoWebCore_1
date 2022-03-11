using demoWebCore_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using demoWebCore_1.Models.ModelViews;

namespace demoWebCore_1.IService
{
    public interface ICommentService
    {
        Comment SaveComment(Comment c);
        DataContext GetDataContext();
        List<Comment> GetListComment(int postID);
        Comment UpdateComment(Comment c);
        void Delete(int id);
        void Edit(int id, string content);
    }
}
