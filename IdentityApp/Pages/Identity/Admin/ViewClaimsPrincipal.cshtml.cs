using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityApp.Pages.Identity.Admin
{
  public class ViewClaimsPrincipalModel : AdminPageModel
  {
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string Callback { get; set; } = string.Empty;

    public UserManager<IdentityUser> UserManager { get; set; }
    public SignInManager<IdentityUser> SignInManager { get; set; }
    public ClaimsPrincipal Principal { get; set; }

    public ViewClaimsPrincipalModel(UserManager<IdentityUser> usrMgr, SignInManager<IdentityUser> signMgr)
    {
      UserManager = usrMgr;
      SignInManager = signMgr;
    }

    public async Task<IActionResult> OnGetAsync()
    {
      if (string.IsNullOrEmpty(Id))
      {
        return RedirectToPage("SelectUser", new { Label = "View ClaimsPrincipal", Callback = "ClaimsPrincipal" });
      }

      IdentityUser user = await UserManager.FindByIdAsync(Id);
      Principal = await SignInManager.CreateUserPrincipalAsync(user);

      return Page();
    }
  }
}