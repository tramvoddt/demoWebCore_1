using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models.ModelViews
{
    [Table("reports")]
    public class Reports
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int id { get; set; }
        [Column("cmt_id", TypeName = "int")]
        public int cmt_id { get; set; }
        [Column("list_user", TypeName = "varchar")]
        public string list_user { get; set; }
        [Column("total", TypeName = "int")]
        public int total { get; set; }

        [Column("status", TypeName = "int")]
        public int status { get; set; }
    }
}
