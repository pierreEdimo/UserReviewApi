using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using userVoice.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using userVoice.DBContext;
using userVoice.DTo;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace userVoice.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly DatabaseContext _context; 

        public UserController(UserManager<UserEntity> userManager, 
                              SignInManager<UserEntity> signInManager, 
                              IConfiguration configuration, 
                              DatabaseContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;

            _context.Database.EnsureCreated(); 
        }

        [HttpGet]
        public async Task<ActionResult<UserEntity>> GetUser()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var user = await _userManager.Users.Include(a => a.getReviews)
                                               .Include(a => a.getComments)
                                               .SingleOrDefaultAsync(x => x.Email == email); 
            return user;
        }

        [HttpGet(Name = nameof(GetAllUsers) ) ]
        public async Task<List<UserEntity>> GetAllUsers()
        {
            using (var Context = new DatabaseContext())
            {
                return await _userManager.Users.Include(a => a.getComments)
                                               .Include(a => a.getReviews)
                                               .ToListAsync();
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Register( [FromBody] RegisterDTo model )
        {
            var user = new UserEntity
            {
                UserName = model.UserName, 
                Email = model.Email, 
               

            };

            var result = await _userManager.CreateAsync(user, model.passWord); 

            if(result.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(user, model.passWord, false, false);
                return GenerateJwtToken(model.Email, user); 
            }

            throw new ApplicationException("UNKNOWN_ERROR"); 
        }

        [AllowAnonymous]
        public async Task<object> Login([FromBody] LoginDTo modelLogin)
        {
            var user = await _userManager.FindByEmailAsync(modelLogin.Email); 

            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, modelLogin.passWord, false, false);

                if (result.Succeeded)
                {
                    var loggedUser = _userManager.Users.SingleOrDefault(u => u.Email == modelLogin.Email);

                    return GenerateJwtToken(modelLogin.Email, loggedUser); 
                }
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }
        

      
        private object GenerateJwtToken(string email, UserEntity user)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
           

            var token = new JwtSecurityToken(
                 _configuration["JwtIssuer"],
                 _configuration["JwtIssuer"],
                 claims,
                 signingCredentials: credentials
                 );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }



    }
}
