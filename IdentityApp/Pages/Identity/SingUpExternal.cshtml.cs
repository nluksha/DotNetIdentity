using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityApp.Pages.Identity
{
  [AllowAnonymous]
  public class SignUpExternalModel : UserPageModel
  {
    public UserManager<IdentityUser> UserManager { get; set; }
    public SignInManager<IdentityUser> SignInManager { get; set; }
    public IdentityUser IdentityUser { get; set; }
    public async Task<string> ExternalProvider() => (await UserManager.GetLoginsAsync(IdentityUser)).FirstOrDefault()?.ProviderDisplayName;

    public SignUpExternalModel(UserManager<IdentityUser> usrMgr, SignInManager<IdentityUser> signMgr)
    {
      UserManager = usrMgr;
      SignInManager = signMgr;
    }

    public IActionResult OnPost(string provider)
    {
      string callbackUrl = Url.Page("SignUpExternal", "Callback");
      AuthenticationProperties props = SignInManager.ConfigureExternalAuthenticationProperties(provider, callbackUrl);

      return new ChallengeResult(provider, props);
    }

    public async Task<IActionResult> OnGetCallbackAsync()
    {
      ExternalLoginInfo info = await SignInManager.GetExternalLoginInfoAsync();
      string email = info?.Principal?.FindFirst(ClaimTypes.Email)?.Value;

      if (string.IsNullOrEmpty(email))
      {
        return Error("External service has not provided an email address.");
      }
      else if ((await UserManager.FindByEmailAsync(email)) != null)
      {
        return Error("An account already exists with your email address.");
      }

      IdentityUser identUser = new IdentityUser
      {
        UserName = email,
        Email = email,
        EmailConfirmed = true
      };
      IdentityResult result = await UserManager.CreateAsync(identUser);

      if (result.Succeeded)
      {
        identUser = await UserManager.FindByEmailAsync(email);
        result = await UserManager.AddLoginAsync(identUser, info);

        return RedirectToPage(new { id = identUser.Id });
      }

      return Error("An account could not be created.");
    }

    public async Task<IActionResult> OnGetAsync(string id)
    {
      if (id == null)
      {
        return RedirectToPage("SignUp");
      }
      else
      {
        IdentityUser = await UserManager.FindByIdAsync(id);

        if (IdentityUser == null)
        {
          return RedirectToPage("SignUp");
        }
      }
      return Page();
    }

    private IActionResult Error(string err)
    {
      TempData["errorMessage"] = err;

      return RedirectToPage();
    }
  }
}