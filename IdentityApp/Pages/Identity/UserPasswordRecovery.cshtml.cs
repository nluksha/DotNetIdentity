using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IdentityApp.Pages.Identity
{
  public class UserPasswordRecoveryModel : UserPageModel
  {
    public UserManager<IdentityUser> UserManager { get; set; }
    public IdentityEmailService EmailService { get; set; }

    public UserPasswordRecoveryModel(UserManager<IdentityUser> usrMgr, IdentityEmailService emailService)
    {
      UserManager = usrMgr;
      EmailService = emailService;
    }

    public async Task<IActionResult> OnPostAsync([Required] string email)
    {
      if (ModelState.IsValid)
      {
        IdentityUser user = await UserManager.FindByEmailAsync(email);

        if (user != null)
        {
          await EmailService.SendPasswordRecoveryEmail(user, "UserPasswordRecoveryConfirm");
        }

        TempData["message"] = "We have sent you an email. " + " Click the link it contains to choose a new password.";

        return RedirectToPage();
      }
      
      return Page();
    }
  }
}