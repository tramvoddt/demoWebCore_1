using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models.ModelViews
{
    [Table("collection")]

    public class Collect
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int id { get; set; }

        [Column("name", TypeName = "nvarchar")]
        [Required(ErrorMessage = "* required")]

        public string name { get; set; }

        [Column("user_id", TypeName = "int")]
        public int user_id { get; set; }
    
        [Column("created_at", TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column("status", TypeName = "bit")]
        public bool status { get; set; }
    }
}
