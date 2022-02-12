﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models
{
    [Table("posts")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int id { get; set; }
        [Column("collection_id", TypeName = "int")]
        public int colection_id { get; set; }
        [Column("subject", TypeName = "nvarchar")]
        public string subject { get; set; }
        [Column("content", TypeName = "text")]

        public string content { get; set; }
        [Column("img", TypeName = "nvarchar")]

        public string img { get; set; }
        [Column("url", TypeName = "nvarchar")]

        public string url { get; set; }
        [Column("created_at", TypeName = "datetime")]

        public DateTime created_at { get; set; }
        [Column("status", TypeName = "bit")]

        public bool status { get; set; }

    }
}