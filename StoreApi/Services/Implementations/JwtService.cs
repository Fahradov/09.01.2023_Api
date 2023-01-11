using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Store.Core.Entities;
using StoreApi.Services.Interfaces;

namespace StoreApi.Services.Implementations
{

    public class JwtService : IJwtService 
    {
        public string GenerateToken(AppUser user, IList<string> roles,IConfiguration config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim("FullName",user.FullName),
            };

            
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Name, x));
            claims.AddRange(roleClaims);

            string secret = config.GetSection("JWT:secret").Value;

            var symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
                (
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddHours(5),
                issuer: config.GetSection("JWT:issuer").Value,
                audience: config.GetSection("JWT:audience").Value
                );

            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenStr;
        }
    }
}

