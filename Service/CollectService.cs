﻿using demoWebCore_1.IService;
using demoWebCore_1.Models;
using demoWebCore_1.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Service
{
    public class CollectService:ICollectService
    {
        private readonly DataContext ct;
        public CollectService(DataContext context)
        {
            ct = context;
        }
        public DataContext GetDataContext()
        {
            return ct;
        }
        public Collect Save(Collect u)
        {
            ct.Collect.Add(u);
            int res = ct.SaveChanges();
            if (res > 0)
            {
                return u;
            }
            else return null;
        }
        public List<Collect> GetCollectionByUserID(int id)
        {
            var q = ct.Collect.Where(x => x.user_id == id).ToList();

            return q??null;
        }
        public bool NameExists(string name)
        {
            var q = ct.Collect.FirstOrDefault(x => x.name == name&&x.user_id==AuthRequest.id);
            if (q != null)
            {
                return true;
            }
            return false;
        }
        public List<Collect> GetCollects()
        {
            var q = ct.Collect.OrderByDescending(x=>x.id).ToList();

            return q ?? null;
        }
    }
}