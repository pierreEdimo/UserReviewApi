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
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context; 

        public UserController(UserManager<UserEntity> userManager, 
                              SignInManager<UserEntity> signInManager, 
                              IConfiguration configuration, 
                              DatabaseContext context,
                              IMapper mapper
                           
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
            _context = context; 

            
        }

        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetUser()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == email);

            return _mapper.Map<UserDTO>(user) ;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = "Admin" )]
        [HttpGet(Name = nameof(GetAllUsers) ) ]
        public async Task<List<UserDTO>> GetAllUsers()
        {
            using (var Context = new DatabaseContext())
            {
                var users = await _userManager.Users.ToListAsync();

                return _mapper.Map<List<UserDTO>>(users); 
            }
        }

        public async Task<object> updateEmail([FromBody] UpdateEmailDTO emailDTO)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var user = await _userManager.FindByEmailAsync(email); 

            if(user == null)
            {
                return NotFound(); 
            }

            user.Email = emailDTO.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var loggedUser = await _userManager.FindByEmailAsync(emailDTO.Email);

                return GenerateJwtToken(loggedUser.Email, loggedUser); 
            }

            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Register( [FromBody] RegisterInfo model )
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

            return BadRequest(result.Errors); 
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login([FromBody] LoginInfo modelLogin)
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
           

            var token = new JwtSecurityToken(
                 issuer: null, 
                 audience:null,
                 claims:claims,
                 signingCredentials: credentials
                 );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<String>>> GetRoles()
        {
            return await _context.Roles.Select(x => x.Name).ToListAsync();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> AssignRole(EditRoleDTO editRole)
        {
            var user = await _userManager.FindByIdAsync(editRole.UserId);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editRole.RoleName));

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<ActionResult> RemoveRole(EditRoleDTO editRole)
        {
            var user = await _userManager.FindByIdAsync(editRole.UserId);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editRole.RoleName));

            return NoContent();
        }



    }
}
