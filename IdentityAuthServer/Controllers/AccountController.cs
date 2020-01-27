using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityAuthServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            return Ok(_userManager.Users);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]Register register)
        {
            var user = new AppUser
            {
                UserName = register.UserName,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                return Ok(user);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.UserName);
                return Ok(new { Token = GenerateJwtToken(appUser) });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UserDetails userDetails)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.Email = userDetails.Email;

            var result = await _userManager.UpdateAsync(user);
            return Ok(result);
        }

        private string GenerateJwtToken(AppUser user)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            });

            var tokenHandler = new JwtSecurityTokenHandler();
            DateTime expiry = DateTime.UtcNow.AddMinutes(5);

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Subject = claimsIdentity,
                IssuedAt = DateTime.UtcNow,
                Expires = expiry,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.Default.GetBytes(_configuration["JWT:Key"])),
                        SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
    }
}