using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.IService
{
    public interface IPostService
    {
        Post Save(Post u);
        DataContext GetDataContext();
        Post GetPostByID(int ID);
        List<Post> GetPosts();
        List<int> GetPostByCollection(int collectID);
        List<Post> GetListPostByListID(List<int> ls);
    }
}
