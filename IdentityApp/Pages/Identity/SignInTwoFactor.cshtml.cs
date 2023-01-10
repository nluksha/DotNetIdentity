using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityApp.Pages.Identity
{
  [AllowAnonymous]
  public class SignInTwoFactorModel : UserPageModel
  {
    public UserManager<IdentityUser> UserManager { get; set; }
    public SignInManager<IdentityUser> SignInManager { get; set; }

    [BindProperty]
    public string? ReturnUrl { get; set; }

    [BindProperty]
    [Required]
    public string Token { get; set; } = string.Empty;

    [BindProperty]
    public bool RememberMe { get; set; } = false;

    public SignInTwoFactorModel(UserManager<IdentityUser> usrMgr, SignInManager<IdentityUser> signMgr)
    {
      UserManager = usrMgr;
      SignInManager = signMgr;
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (ModelState.IsValid)
      {
        IdentityUser user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

        if (user != null)
        {
          string token = Regex.Replace(Token, @"\s", "");
          SignInResult result = await SignInManager.TwoFactorAuthenticatorSignInAsync(token, true, RememberMe);

          if (!result.Succeeded)
          {
            result = await SignInManager.TwoFactorRecoveryCodeSignInAsync(token);
          }

          if (result.Succeeded)
          {
            if (await UserManager.CountRecoveryCodesAsync(user) <= 3)
            {
              return RedirectToPage("SignInCodesWarning");
            }

            return Redirect(ReturnUrl ?? "/");
          }
        }

        ModelState.AddModelError("", "Invalid token or recovery code");
      }

      return Page();
    }
  }
}