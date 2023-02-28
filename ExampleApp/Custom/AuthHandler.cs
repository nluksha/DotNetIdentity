using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExampleApp.Custom
{
  public class AuthHandler : IAuthenticationHandler
  {
    private HttpContext context;
    private AuthenticationScheme scheme;

    public Task InitializeAsync(AuthenticationScheme authScheme, HttpContext httpContext)
    {
      context = httpContext;
      scheme = authScheme;

      return Task.CompletedTask;
    }

    public Task<AuthenticateResult> AuthenticateAsync()
    {
      AuthenticateResult result;
      string user = context.Request.Cookies["authUser"];

      if (user != null)
      {
        Claim claim = new Claim(ClaimTypes.Name, user);
        ClaimsIdentity ident = new ClaimsIdentity(scheme.Name);

        ident.AddClaim(claim);
        result = AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(ident), ident.Name));
      }
      else
      {
        result = AuthenticateResult.NoResult();
      }

      return Task.FromResult(result);
    }

    public Task ChallengeAsync(AuthenticationProperties properties)
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      return Task.CompletedTask;
    }

    public Task ForbidAsync(AuthenticationProperties properties)
    {
      context.Response.StatusCode = StatusCodes.Status403Forbidden;
      return Task.CompletedTask;
    }
  }
}
