using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sq007.Models
{
    public class Address
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]

        public string City { get; set; }
        [Required]

        public string State { get; set; }
        [Required]

        public string Country { get; set; }



    }
}
