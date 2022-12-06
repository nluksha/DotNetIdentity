using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityApp.Pages.Identity.Admin
{
  public class DashboardModel : AdminPageModel
  {
    private readonly string[] emails = { "alice@example.com", "bob@example.com", "charlie@example.com" };

    public int UsersCount { get; set; } = 0;
    public int UsersUnconfirmed { get; set; } = 0;
    public int UsersLockedout { get; set; } = 0;
    public int UsersTwoFactor { get; set; } = 0;

    public UserManager<IdentityUser> UserManager { get; set; }

    public DashboardModel(UserManager<IdentityUser> userMgr)
    {
      UserManager = userMgr;
    }

    public async Task<IActionResult> OnPostAsync()
    {
      foreach (string email in emails)
      {
        var userObject = new IdentityUser
        {
          UserName = email,
          Email = email,
          EmailConfirmed = true
        };

        IdentityResult result = await UserManager.CreateAsync(userObject);
        result.Process(ModelState);
      }

      if (ModelState.IsValid)
      {
        return RedirectToPage();
      }
      else
      {
        return Page();
      }
    }
  }
}