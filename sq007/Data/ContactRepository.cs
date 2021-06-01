using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sq007.DTO;
using sq007.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sq007.Data
{
    public class ContactRepository : IContactRepository
    {
        private readonly DataContext _db;
        private readonly UserManager<Contact> _userManager;
        //UserManager<Contact> userManager
        public ContactRepository(DataContext db, UserManager<Contact> userManager)
        {
            _db = db;
           
            _userManager = userManager;
        }

        /// <summary>
        /// get contact
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ContactDto>> GetBySearchTerm(string searchTerm, PaginationFilter filter)
        {
            var validPageFilter = new PaginationFilter(filter.CurrentPage);
            List<ContactDto> contactsToGet = new List<ContactDto>();
            var contacts = await _db.Contacts.Where(t => t.Email.Contains(searchTerm) ||

                                                    t.FirstName.Contains(searchTerm) ||
                                                    t.LastName.Contains(searchTerm) 
                                                    
                                                    ).Include(a => a.Address)
                                                     .Skip((validPageFilter.CurrentPage - 1) * 4)
                                                    .Take(4).ToListAsync();
            var num = contacts.Count();
            for (int i = 0; i < num; i++)
            {
                ContactDto contactDto = new ContactDto();
                contactDto.Id = contacts[i].Id;
                contactDto.FirstName = contacts[i].FirstName;
                contactDto.LastName = contacts[i].LastName;
                contactDto.PhoneNumber = contacts[i].PhoneNumber;
                contactDto.Email = contacts[i].Email;
                contactDto.Address = contacts[i].Address;
                contactDto.PhotoUrl = contacts[i].PhotoUrl ;
                contactsToGet.Add(contactDto);
            }
            return contactsToGet;
        }


        public async Task<Contact> Create(ContactDto contact)
        {

            Contact contact1 = new Contact
            {
                
                //Id = contact.Id,
                UserName = contact.Email,
                PhotoUrl = contact.PhotoUrl,
                Email = contact.Email,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                PhoneNumber = contact.PhoneNumber,
                Address = contact.Address

            };

            _db.Add(contact1);
            await _db.SaveChangesAsync();

            return contact1;

        }

        public async Task Delete(string id)
        {
            var deleteUser = await _db.Contacts.Where(x => x.Id == id).Include(x => x.Address).FirstOrDefaultAsync();
            var deleteAddress = await _db.Addresses.Where(x => x.Id == Convert.ToString(id)).FirstOrDefaultAsync();
             _db.Contacts.Remove(deleteUser);
            _db.Addresses.Remove(deleteAddress);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactDto>> Get([FromQuery] PaginationFilter filter)
        {
            var paginationFilter = new PaginationFilter(filter.CurrentPage);
             var getContact = await _db.Contacts.Include(x => x.Address)
                .Skip((paginationFilter.CurrentPage -1 ) * 3)
                .Take(3).ToListAsync();
            
            List<ContactDto> newContact = new List<ContactDto>();
            foreach (var contact in getContact)
            {
                ContactDto contactDto = new ContactDto();
                contactDto.Id = contact.Id;
                contactDto.FirstName = contact.FirstName;
                contactDto.LastName = contact.LastName;
                contactDto.Email = contact.Email;
                contactDto.PhotoUrl = contact.PhotoUrl;
                contactDto.PhoneNumber = contact.PhoneNumber;
                    contactDto.Address = contact.Address;
                    
                newContact.Add(contactDto);

            }
            return newContact;

        }

        public async Task<ContactDto> GetByEmail(string email)
        {
            var getByEmail = await _db.Contacts.Where(x => x.Email == email).Include(a => a.Address).FirstOrDefaultAsync();
            ContactDto contactdto = new ContactDto()
            {
                Id = getByEmail.Id,
                FirstName = getByEmail.FirstName,
                LastName = getByEmail.LastName,
                Email = getByEmail.Email,
                PhoneNumber = getByEmail.PhoneNumber,
                PhotoUrl = getByEmail.PhotoUrl,
                Address = getByEmail.Address
            };
            return contactdto;

        }

        public async Task<ContactDto> GetById(string id)
        {
            var getByEmail = await _db.Contacts.Where(x => x.Id == id).Include(a => a.Address).FirstOrDefaultAsync();
            ContactDto contactdto = new ContactDto()
            {
                Id = getByEmail.Id,
                FirstName = getByEmail.FirstName,
                LastName = getByEmail.LastName,
                Email = getByEmail.Email,
                PhoneNumber = getByEmail.PhoneNumber,
                PhotoUrl = getByEmail.PhotoUrl,
                Address = getByEmail.Address
            };
            return contactdto;
        }

        public async Task Update(ContactDto user)
        {
            var getByid = await _db.Contacts.Where(x => x.Id == user.Id).Include(a => a.Address).FirstOrDefaultAsync();
            var address = await _db.Addresses.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
            getByid.FirstName = user.FirstName;
            getByid.LastName = user.LastName;
            getByid.Email = user.Email;
            getByid.PhotoUrl = user.PhotoUrl;
            //getByid.Id = user.Id;
            address.City = user.Address.City;
            getByid.PhoneNumber = user.PhoneNumber;
            _db.Update(getByid);
            _db.Update(address);
            await _db.SaveChangesAsync();




            ////_db.Entry(getByid).State = EntityState.Modified;
            //await _db.SaveChangesAsync();

            //Contact contact = new Contact()
            //{   Id = user.Id,
            //    FirstName = user.FirstName, 
            //    LastName = user.LastName, 
            //    Email = user.Email, 
            //    PhoneNumber = user.PhoneNumber, 
            //    PhotoUrl = user.PhotoUrl,
            //    Address = user.Address
            //};
            //_db.Entry(user).State = EntityState.Modified;
            //await _db.SaveChangesAsync();
        }



        public async Task UpdatePhoto(EditPhotoDto contact)
        {
            var getByid = await _db.Contacts.Where(x => x.Id == contact.id).Include(a => a.Address).FirstOrDefaultAsync();

            getByid.PhotoUrl = contact.photourl;

            
            _db.Update(getByid);
            await _db.SaveChangesAsync();

        }
    }
}
