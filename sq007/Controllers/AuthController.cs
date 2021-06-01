using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sq007.Data;
using sq007.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sq007.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<Contact> _signinManager;
        private readonly UserManager<Contact> _userManager;
        private readonly DataContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(IConfiguration config, RoleManager<IdentityRole> roleManager, SignInManager<Contact> signInManager, UserManager<Contact> userManager, DataContext  dataContext)
        {
            _config = config;
            _signinManager = signInManager;
            _userManager = userManager;
            _db = dataContext;
            _roleManager = roleManager;
        }
        [HttpPost("login")]
         public async Task<IActionResult> Login([FromForm] LoginDto model)
         {
            // a var user = await _userManager.FindByEmailAsync(model.Email);
            var getByEmail = await _db.Contacts.Where(x => x.Email == model.Email).Include(a => a.Address).FirstOrDefaultAsync();
            if (getByEmail == null)
                return BadRequest();
            
            string[] roles = { "Admin"  };
            var token = UtilityClass.GenerateToken(getByEmail.UserName, getByEmail.Id, getByEmail.Email, _config, roles);
            return Ok(token);

            // a if (user == null)
            // a return BadRequest();

           // a var loggedInUser = await _signinManager.PasswordSignInAsync(model.Email,model.Password,false,false);

            // a if (!loggedInUser.Succeeded)
               // a return BadRequest();

           // a  string[] roles = { "Admin" };
           // try
            //{


              // a  var token = UtilityClass.GenerateToken(user.UserName, user.Id, user.Email, _config, roles);
               // a return Ok(token);
           // }
           // catch(Exception e)
            //{
               // return BadRequest($"Error:{e.ToString()}");
            //}
        } 
    }
}
