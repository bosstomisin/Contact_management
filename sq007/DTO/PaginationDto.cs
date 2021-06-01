using sq007.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sq007.DTO
{
    public class PaginationDto
    {
        public int count { get; set; }
        public int perPage { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<Contact> contacts { get; set; }
        public PaginationDto(int _count, int _perPage, int _CurrentPage, IEnumerable<Contact> _contact)
        {
            this.count = _count;
            this.perPage = _perPage;
            this.CurrentPage = _CurrentPage;
            this.contacts = _contact;
        }
    }
}
