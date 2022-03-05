using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.IService
{
    public interface IReactService
    {
        DataContext GetDataContext();
        bool CreateReact(React r);

        void ReactAction(int cmtID, int value);
        bool CheckUserReact(int cmtID);
        React GetReactByCmt(int cmt);
    }
}
