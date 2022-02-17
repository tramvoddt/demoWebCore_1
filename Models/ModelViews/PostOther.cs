using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models.ModelViews
{
    [Table("save_post")]
    public class PostOther
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int id { get; set; }

        [Column("user_id", TypeName = "int")]
        public int user_id { get; set; }
        [Column("collection_id", TypeName = "int")]
        public int collection_id { get; set; }
        [Column("post_id", TypeName = "int")]
        public int post_id { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime created_at { get; set; }

    }
}
