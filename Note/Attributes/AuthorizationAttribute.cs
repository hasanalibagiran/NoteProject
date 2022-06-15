using System.Net;
using Dto.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Note.Attributes
{
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public int RoleID { get; set; }
        
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            ContextUser contextUser = context.HttpContext.Items["User"] as ContextUser;
            
            if ( contextUser != null && contextUser.RoleId == RoleID)
            {
                return;
            }
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            await context.HttpContext.Response.StartAsync();
            return;
        }
    }
}