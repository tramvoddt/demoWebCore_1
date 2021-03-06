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
        bool CheckChooseCollection(int postID, int collectID);
        List<Post> GetPostByUserID(int id, int post_id);
        void RemovePost(int id);
        void UpdatePost(List<Post> l, bool sts);
        void UpdateAllPost(bool val);
        bool ReportComment( int cmt_id);
        bool CheckUserReport(int cmtID);
        List<Reports> GetReports();
        Users GetUserByCmtID(int id);
        Post GetPostByCmtID(int id);
        void UpdateReport(List<Reports> l, int sts);

        List<Reports> GetListReportByListInt(List<int> ls);
        string CheckSelected(int postID);


    }
}
