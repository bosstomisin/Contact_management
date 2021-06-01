using Microsoft.AspNetCore.Mvc;
using sq007.DTO;
using sq007.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sq007.Data
{
    public interface IContactRepository
    {
        public Task<IEnumerable<ContactDto>> Get([FromQuery] PaginationFilter filter);
        public Task<IEnumerable<ContactDto>> GetBySearchTerm(string searchTerm, PaginationFilter filter);
        public Task<ContactDto> GetById(string id);
        public Task<ContactDto> GetByEmail(string email);
        public Task<Contact> Create(ContactDto contact);
        public Task Update(ContactDto contact);
        public Task UpdatePhoto(EditPhotoDto contact);

        public Task Delete(string id);
    }
}
