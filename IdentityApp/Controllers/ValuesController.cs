using IdentityApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace IdentityApp.Controllers
{
  [Authorize]
  [ApiController]
  [Route("/api/data")]
  public class ValuesController : ControllerBase
  {
    private ProductDbContext DbContext;

    public ValuesController(ProductDbContext dbContext)
    {
      DbContext = dbContext;
    }

    [HttpGet]
    public IAsyncEnumerable<Product> GetProducts() => (IAsyncEnumerable<Product>)DbContext.Products;

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductBindingTarget target)
    {
      if (ModelState.IsValid)
      {
        Product product = new Product
        {
          Name = target.Name,
          Price = target.Price,
          Category = target.Category
        };
        await DbContext.AddAsync(product); await DbContext.SaveChangesAsync();

        return Ok(product);
      }

      return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public Task DeleteProduct(long id)
    {
      DbContext.Products.Remove(new Product { Id = id });

      return DbContext.SaveChangesAsync();
    }
  }

  public class ProductBindingTarget
  {
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public decimal Price { get; set; } = 0;
    [Required] public string Category { get; set; } = string.Empty;
  }
}
