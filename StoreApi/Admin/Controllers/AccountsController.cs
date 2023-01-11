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
using StoreApi.Services;
using StoreApi.Services.Interfaces;

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
        private readonly IJwtService _jwtServ;

        public AccountsController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,IConfiguration config,IJwtService jwtServ)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _jwtServ = jwtServ;
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

            var roles =await  _userManager.GetRolesAsync(user);


            var token=_jwtServ.GenerateToken(user, roles, _config);

            
            

            return Ok(token);
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

