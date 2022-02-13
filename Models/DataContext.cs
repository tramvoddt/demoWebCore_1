using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using demoWebCore_1.Models.ModelViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace demoWebCore_1.Models
{
    public class DataContext : DbContext
    {
        private string _ConnectionString;
        public string ConnectionString { get => _ConnectionString; set => _ConnectionString = value; }
        public static DataContext _instance;

        public static DataContext Instance
        {
            get
            {
               if(_instance is null)
                {
                    _instance = new DataContext();
                }
                return _instance;
            }
        }
        public DataContext GetConnection() { return _instance; }
       
        public DataContext()
        {

        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Post> Post { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Collect> Collect { get; set; }
    }
}
