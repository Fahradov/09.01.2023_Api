using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.Core.Entities;
using StoreApi.Admin.Dtos.AccountDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreApi.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AccountsController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("Member"));

            return Ok();

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            AppUser user =await  _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null || user.IsMember)
                return BadRequest(new { error = new { field = "UserName", message = "Username is incorrect!!!" } });

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return BadRequest(new { error = new { field = "Password", message = "Password is incorrect!!!" } });


            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim("FullName",user.FullName),
            };

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Name,x));
            claims.AddRange(roleClaims);

            string secret = _config.GetSection("JWT:secret").Value;

            var symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
                (
                claims:claims,
                signingCredentials:creds,
                expires:DateTime.UtcNow.AddHours(5),
                issuer:_config.GetSection("JWT:issuer").Value,
                audience:_config.GetSection("JWT:audience").Value
                );

            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new {token=tokenStr});
        }

        //[HttpPost("createAdmin")]
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser user = new AppUser
        //    {
        //        FullName = "Seymur Fahradov",
        //        UserName = "Master",
        //    };

        //    await _userManager.CreateAsync(user, "Salam123");
        //    await _userManager.AddToRoleAsync(user, "SuperAdmin");

        //    return Ok();
        //}
        [Authorize(Roles ="SuperAdmin")]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            return Ok(new { username = User.Identity.Name });
        }
    }
}

