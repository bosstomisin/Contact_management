using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sq007.DTO
{
    public class PhotoToAddDto
    {
        public IFormFile PhotoFile { get; set; }
       // public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
