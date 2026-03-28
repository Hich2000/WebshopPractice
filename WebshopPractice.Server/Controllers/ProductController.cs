using Microsoft.AspNetCore.Mvc;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    [HttpGet(Name = "GetProduct")]
    public IEnumerable<Product>? Get()
    {
        return Enumerable.Range(1, 20).Select(index => new Product { 
            Id = index,
            Name = $"Product{index}",
            Price = 1.50M
        }).ToArray();
    }
}
