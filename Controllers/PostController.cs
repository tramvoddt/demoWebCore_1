using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using demoWebCore_1.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace demoWebCore_1.Controllers
{
    public class PostController : Controller
    {
        private readonly DataContext dt;
        public PostController(DataContext db)
        {
            dt = db;
        }
        public IActionResult Index()
        {
            var post = dt.Post.FromSqlRaw("select * from posts").ToList();
            ViewData["Title"] = "Posts";
            return View(post);
        }
      
        }
}
