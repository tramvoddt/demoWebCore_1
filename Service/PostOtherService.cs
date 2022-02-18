using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Service
{
    public class PostOtherService:IPostOtherService
    {
        private readonly DataContext ct;
        public PostOtherService(DataContext context)
        {
            ct = context;
        }
        
        public DataContext GetDataContext()
        {
            return ct;
        }
        public bool SavePost(PostOther p)
        {

            ct.PostOther.Add(p);
            int res = ct.SaveChanges();
            if (res > 0)
            {
                return true;
            }
            else return false;
        }
        public List<PostOther> GetListPostOther(int collection)
        {
            return ct.PostOther.Where(x => x.collection_id == collection && x.user_id == AuthRequest.id).ToList();
        }
    }
}
