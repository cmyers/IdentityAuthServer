using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
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
        [Authorize]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
            return Ok(_userManager.Users);
        }

        [HttpPost]
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
                await _signInManager.SignInAsync(user, false);
                return Ok(user);
            }

            return BadRequest();
        }

        [HttpPost]
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
        [Authorize]
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
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    }
}