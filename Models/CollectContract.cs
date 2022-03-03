using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models
{
    public class CollectContract
    {
        public int id { get; set; }
        public string name { get; set; }
        public int user_id { get; set; }
        public DateTime created_at { get; set; }
        public bool status { get; set; }
    }
}
