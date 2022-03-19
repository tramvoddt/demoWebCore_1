using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models.ModelViews
{
    [Table("users")]

    public class Users
    {
        //id
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int id { get; set; }

        //code

        [Column("code", TypeName = "varchar")]
        public string code { get; set; }

        //name
        [Column("name", TypeName = "nvarchar")]
        [Required(ErrorMessage = "* required")]
        public string name { get; set; }

        //phone
        [Column("phone", TypeName = "varchar")]
        [Required(ErrorMessage = "* required")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Invalid phone number")]
        [Remote("PhoneExists", "Auth", ErrorMessage = "This phone number already exist.")]
        public string phone { get; set; }

        //email
        [Column("email", TypeName = "nvarchar")]
        [Required(ErrorMessage = "* required")]
        [RegularExpression("[a-zA-Z0-9][a-zA-Z0-9._]*@[a-zA-Z0-9-]+([.][a-zA-Z]+)+", ErrorMessage = "Invalid email address")]
        [Remote("EmailExists", "Auth", ErrorMessage = "This email already exist.")]
        public string email { get; set; }

        //password
        [Required(ErrorMessage = "* required")]
        [Column("password", TypeName = "nvarchar")]
        [MinLength(6, ErrorMessage = "password needs more than 5 characters")]
        public string password { get; set; }

        //role_id
        [Column("role_id", TypeName = "int")]
        public int? role_id { get; set; }

        //created_at
        [Column("created_at", TypeName = "datetime")]
        public DateTime created_at { get; set; }

        //status
        [Column("status", TypeName = "bit")]
        public bool status { get; set; }
        //avt
        [Column("avt", TypeName = "varbinary")]
        public byte[] avt { get; set; }
    }
}
