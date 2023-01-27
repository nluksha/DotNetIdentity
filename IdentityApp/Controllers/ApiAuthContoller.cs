using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityApp.Controllers
{
  [ApiController]
  [Route("/api/auth")]
  public class ApiAuthController : ControllerBase
  {
    private SignInManager<IdentityUser> SignInManager;

    public ApiAuthController(SignInManager<IdentityUser> signMgr)
    {
      SignInManager = signMgr;
    }

    [HttpPost("signin")]
    public async Task<object> ApiSignIn([FromBody] SignInCredentials creds)
    {
      SignInResult result = await SignInManager.PasswordSignInAsync(creds.Email, creds.Password, true, true);

      return new { success = result.Succeeded };
    }

    [HttpPost("signout")]
    public async Task<IActionResult> ApiSignOut()
    {
      await SignInManager.SignOutAsync();
      
      return Ok();
    }
  }

  public class SignInCredentials
  {
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
  }
}
