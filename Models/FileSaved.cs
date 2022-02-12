using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoWebCore_1.Models
{
    public class FileUpload
    {
        public IFormFile file { get; set; }
        public string Post { get; set; }
    }
}
