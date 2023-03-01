using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExampleApp.Custom
{
    public class CustomAuthentication
    {
        private RequestDelegate next;

        public CustomAuthentication(RequestDelegate requestDelegate)
        {
            next = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            string user = context.Request.Cookies["authUser"];

            if (user != null)
            {
                var claim = new Claim(ClaimTypes.Name, user);
                var identity = new ClaimsIdentity("QueryStringValue");

                identity.AddClaim(claim);
                context.User = new ClaimsPrincipal(identity);
            }

            await next(context);
        }
    }
}
