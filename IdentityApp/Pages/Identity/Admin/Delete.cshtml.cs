using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin
{
  public class DeleteModel : AdminPageModel
  {
    public UserManager<IdentityUser> UserManager { get; set; }
    public IdentityUser IdentityUser { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; } = string.Empty;

    public DeleteModel(UserManager<IdentityUser> mgr) => UserManager = mgr;

    public async Task<IActionResult> OnGetAsync()
    {
      if (string.IsNullOrEmpty(Id))
      {
        return RedirectToPage("Selectuser", new { Label = "Delete", Callback = "Delete" });
      }

      IdentityUser = await UserManager.FindByIdAsync(Id);

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      IdentityUser = await UserManager.FindByIdAsync(Id);
      IdentityResult result = await UserManager.DeleteAsync(IdentityUser);

      if (result.Process(ModelState))
      {
        return RedirectToPage("Dashboard");
      }

      return Page();
    }
  }
}