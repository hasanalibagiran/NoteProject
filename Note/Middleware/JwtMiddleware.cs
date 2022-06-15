using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Business.Extensions;
using Dto.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Note.Middleware
{
    public class JwtMiddleware
    {
        private readonly AppSettings _appSettings;
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                await CheckToken(context, token);
            }
                await _next(context);
        }

        private async Task CheckToken(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var roleId = jwtToken.Claims.First(a => a.Type == "roleId").Value;
                var name = jwtToken.Claims.First(a => a.Type == "name").Value;
                var id = jwtToken.Claims.First(a => a.Type == "id").Value;
                var contextModel = new ContextUser(){
                    Id = Convert.ToInt32(id),
                    Name = name,
                    RoleId = Convert.ToInt32(roleId)
                };
                context.Items.Add("User", contextModel);
                
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)StatusCodes.Status401Unauthorized;
                await context.Response.StartAsync();
            }
        }
    }
}