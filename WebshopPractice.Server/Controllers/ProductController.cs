using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{

    private static readonly string[] productNames =
    [
        "productA", "productB", "productC", "productD", "productE"
    ];

    [HttpGet(Name = "GetProduct")]
    public IEnumerable<Product>? Get()
    {
        return Enumerable.Range(0, 4).Select(index => new Product { 
            Id = index,
            Name = productNames[index],
            Price = 1.50M
        }).ToArray();
    }
}
