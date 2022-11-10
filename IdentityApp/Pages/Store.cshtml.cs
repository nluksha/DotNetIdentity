using IdentityApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages
{
  public class StoreModel : PageModel
  {

    public StoreModel(ProductDbContext ctx) => DbContext = ctx;

    public ProductDbContext DbContext { get; set; }
  }
}