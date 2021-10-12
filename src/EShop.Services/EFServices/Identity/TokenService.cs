using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EShop.Services.Contracts.Identity;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Users.WebApi;
using Microsoft.IdentityModel.Tokens;

namespace EShop.Services.EFServices.Identity
{
    public class TokenService : ITokenService
    {
        private const double ExpireTimeInMinute = 30;

        public string BuildToken(string key, string issuer, UserToBuildJwtTokenViewModel user, bool rememberMe)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            foreach (var role in user.Roles)
            {
                claims.Add(
                    new Claim(ClaimTypes.Role, role)
                );
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: rememberMe ? DateTime.Now.AddDays(90) : DateTime.Now.AddMinutes(ExpireTimeInMinute),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public bool IsTokenValid(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = issuer,
                        ValidAudience = issuer,
                        IssuerSigningKey = mySecurityKey,
                    }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

}
