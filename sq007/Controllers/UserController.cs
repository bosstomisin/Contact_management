using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sq007.Data;
using sq007.DTO;
using sq007.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace sq007.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly UserManager<Contact> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IContactRepository _contactRepo;

        public UserController(IConfiguration config, IContactRepository contactRepo, RoleManager<IdentityRole> roleManager, UserManager<Contact> usermanager)
        {
            _userManager = usermanager;
            _roleManager = roleManager;
            
            _contactRepo = contactRepo;
            Account account = new Account
            {
                Cloud = config.GetSection("CloudinarySettings:CloudName").Value,
                ApiKey = config.GetSection("CloudinarySettings:ApiKey").Value,
                ApiSecret = config.GetSection("CloudinarySet                                                                                                                                                    tings:ApiSecret").Value,
            };
            _cloudinary = new Cloudinary(account);
        }
        [HttpPost("photo/{id}")]
        public IActionResult AddUserPhoto(int id, [FromForm] PhotoToAddDto model)
        {
            // if logged user id does not match with id passes, return unauthorizes logged in
            //if (id != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)) 
               // return Unauthorized();
            var file = model.PhotoFile;
            if (file.Length <= 0)
                return BadRequest("Invalid file size");

            var imageUploadResult = new ImageUploadResult();
            using (var fs = file.OpenReadStream())
            {
                var imageUploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, fs),
                    Transformation = new Transformation().Width(200).Height(200)
                    .Crop("fill").Gravity("face")
                };
                imageUploadResult = _cloudinary.Upload(imageUploadParams);
            }
            var publicId = imageUploadResult.PublicId;
            var Url = imageUploadResult.Url.ToString();
            // send url to database here
            return Ok(new { id = publicId, Url });
        }




        [HttpPost]
        [Route("mkadmin/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer" , Roles ="Admin")]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            var exist = await _userManager.FindByIdAsync(id);
            if (exist == null)
            {
                return NotFound("User does not exist");
            }
            if (await _roleManager.FindByNameAsync("Admin") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            var attempt = await _userManager.AddToRoleAsync(exist, "Admin");
            if (!attempt.Succeeded)
                return StatusCode(500, "Attempt not successful");
            return Ok("Registration successful");
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("all-users")]
        public async Task<IEnumerable<ContactDto>> GetAllUsers([FromQuery] PaginationFilter filter)
        {
            return await _contactRepo.Get(filter);
        }
       
        [HttpGet("Id/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ContactDto>> GetUserById(string id)
        {
            return await _contactRepo.GetById(id);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Email{email}")]
        public async Task<ActionResult<ContactDto>> GetUserEmail(string email)
        {
            return await _contactRepo.GetByEmail(email);
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddUser([FromForm] ContactDto contact)
        {
            var addUser = await _contactRepo.Create(contact);
            return CreatedAtAction(nameof(GetAllUsers), new { id = addUser.Id }, addUser);
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] ContactDto contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }
            await _contactRepo.Update(contact);
            return Content("Profile Successfully Updated");
        }



        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var deleteUser = await _contactRepo.GetById(id);
            if (deleteUser == null)
            {
                return NotFound();
            }
            await _contactRepo.Delete(id);
            return Content("Contact Successfully Deleted");
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPatch("photo/id")]
        public async Task<ActionResult> UpdatePhoto(string id, [FromForm] EditPhotoDto contact)
        {
            await _contactRepo.UpdatePhoto(contact);
            return Content("Profile Successfully Updated");
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContactsBySearch([FromQuery] string searchTerm, [FromQuery] PaginationFilter filter)
        {
            var users = await _contactRepo.GetBySearchTerm(searchTerm, filter);
            if (users.Count() <= 0)
            {
                return Content("No user with said search term exists");
            }
            else
            {
                return Ok(users);
            }


        }
    }
}
