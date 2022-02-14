﻿using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace demoWebCore_1.Service
{
    public class PostService:IPostService
    {
        private readonly DataContext ct;
        public PostService(DataContext context)
        {
            ct = context;
        }
        public Post Save(Post u)
        {
            ct.Post.Add(u);
            int res = ct.SaveChanges();
            if (res > 0)
            {
                return u;
            }
            else return null;
        }
        public DataContext GetDataContext()
        {
            return ct;
        }
        public Post GetPostByID(int id)
        {
            var post = ct.Post.FirstOrDefault(x => x.id == id);
            
             return post??null;
        }
        public List<Post> GetPosts()
        {
            List<Post> p = new List<Post>();
            var post = ct.Post.OrderByDescending(x => x.id).ToList();
            foreach (var item in post)
            {
                var collect = ct.Collect.FirstOrDefault(x => x.id == item.collection_id);
                if (collect.status == false)
                {
                    p.Add(item);
                }


            }

            return p??null;
        }
        public bool CheckStatusCollection(int collect_id)
        {
            var post = ct.Collect.FirstOrDefault(x=>x.id==collect_id);
            
            return post==null ?true:false;
        }
        

    }
}