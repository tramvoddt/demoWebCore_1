using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.IService
{
    public interface ICollectService
    {
        List<Collect> GetCollectionByUserID(int user_id);
        DataContext GetDataContext();
        Collect Save(Collect c);
        bool NameExists(string value);
        bool NameExists(string value,int val);
        List<Collect> GetCollects();
        string GetCollectionName(int id);
        Collect GetCollect(int id);
    }
}
