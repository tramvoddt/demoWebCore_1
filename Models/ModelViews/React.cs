using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models.ModelViews
{
    public class React
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int id { get; set; }

        [Column("cmt_id", TypeName = "int")]
        public int cmt_id { get; set; }
        [Column("list_user", TypeName = "varchar")]
        public string listUser { get; set; }
        //Newtonsoft.Json.JsonConvert.SerializeObject(w); 
       // list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CollectContract>>(res);

        [Column("total", TypeName = "int")]
        public int total { get; set; }
        
    }
}
