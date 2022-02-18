using demoWebCore_1.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.IService
{
    public interface IPostOtherService
    {
        bool SavePost(PostOther p);
        List<PostOther> GetListPostOther(int postID);
    }
}
