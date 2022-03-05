using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Service
{
    public class ReactService :IReactService
    {
        private readonly DataContext ct;
        public ReactService(DataContext context)
        {
            ct = context;
        }
        public DataContext GetDataContext()
        {
            return ct;
        }
        public bool CreateReact(React r)
        {
            ct.React.Add(r);
            int res = ct.SaveChanges();
            if (res > 0)
                return true;
            return false;

        }
        public void ReactAction(int cID, int val)
        {
            var w = ct.React.FirstOrDefault(x => x.cmt_id == cID);
            if (w != null)
            {
                List<int> ls = new List<int>();
                ls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(w.listUser);
                switch (val)
                {
                    case 0:
                        ls.Remove(AuthRequest.id);
                        w.total -= 1;
                        break;
                    case 1:
                        ls.Add(AuthRequest.id);
                        w.total += 1;
                        break;
                }
                w.listUser = Newtonsoft.Json.JsonConvert.SerializeObject(ls);
                ct.SaveChanges();
            }

        }
        public bool CheckUserReact(int cmtID)
        {
            var w = ct.React.FirstOrDefault(x => x.cmt_id == cmtID);
            if (w != null)
            {
                List<int> ls = new List<int>();
                ls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(w.listUser);
                if (ls.Contains(AuthRequest.id))
                {
                    return true;
                }
            }
            return false;

        }
        public React GetReactByCmt(int cmt)
        {
            var w = ct.React.FirstOrDefault(x => x.cmt_id == cmt);
            return w ?? null;
        }
    }
}
