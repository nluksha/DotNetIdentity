using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

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

    public void OnGet()
    {
      UsersCount = UserManager.Users.Count();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      foreach (var existingUser in UserManager.Users.ToList())
      {
        var result = await UserManager.DeleteAsync(existingUser);

        result.Process(ModelState);
      }

      foreach (string email in emails)
      {
        var userObject = new IdentityUser
        {
          UserName = email,
          Email = email,
          EmailConfirmed = true
        };

        IdentityResult result = await UserManager.CreateAsync(userObject);


        if (result.Process(ModelState)) {
          result = await UserManager.AddPasswordAsync(userObject, "mysecret");
          
          result.Process(ModelState);
        }
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