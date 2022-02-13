using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Controllers
{
    public class CollectController : Controller
    {
        ICollectService collectService = null;
        public CollectController(ICollectService db)
        {

            collectService = db;
        }
        [HttpPost]
        public int SaveCollect(Collect f)
        {
        
                f.created_at = DateTime.Now;
                f.user_id = AuthRequest.id;
                collectService.Save(f);
            var w = collectService.GetDataContext().Collect.OrderByDescending(x=>x.id).FirstOrDefault(x => x.user_id == AuthRequest.id);
         
            return  w.id;

        }
        //CHECKDUP
        public bool NameExists(string name)
        {
            return collectService.NameExists(name);

        }

    }
}
