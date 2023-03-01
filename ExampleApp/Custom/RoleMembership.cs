using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ExampleApp.Custom
{
    public class RoleMemberships
    {
        private RequestDelegate next;

        public RoleMemberships(RequestDelegate requestDelegate)
        {
            next = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            var mainIdentity = context.User.Identity;

            if (
                mainIdentity.IsAuthenticated && UsersAndClaims.Claims.ContainsKey(mainIdentity.Name)
            )
            {
                ClaimsIdentity identity = new ClaimsIdentity("Roles");
                identity.AddClaim(new Claim(ClaimTypes.Name, mainIdentity.Name));
                identity.AddClaims(UsersAndClaims.Claims[mainIdentity.Name]);

                context.User.AddIdentity(identity);
            }

            await next(context);
        }
    }
}
