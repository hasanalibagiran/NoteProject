using Business.Extensions;
using Dto.Models;
using Microsoft.Extensions.Options;
using Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Business.Services.Token
{
    public class TokenHandler : ITokenHandler
    {

        private readonly AppSettings _appSettings;

        public TokenHandler(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }



        public TokenModel CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            DateTime expireDate = DateTime.Now.AddMinutes(_appSettings.TokenExpiresAddMinutes);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim("id", user.Id.ToString()),
                    new Claim("name", user.UserName),
                    new Claim("password", user.Password),
                    new Claim("roleId", user.RoleId.ToString())
                }),
                Expires = expireDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var userToken = new TokenModel() {
                Token = tokenHandler.WriteToken(token),
                Expire = expireDate
            };
            return userToken;
        }
    }
}