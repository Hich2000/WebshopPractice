using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebshopPractice.Server.Data.Context;
using WebshopPractice.Server.Data.DTO;
using WebshopPractice.Server.Data.Models;
using WebshopPractice.Server.Helpers;

namespace WebshopPractice.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(
    UserManager<ShopUser> usermanager,
    WebshopDbContext db
) : ControllerBase
{
    private const int _smallestPageLength = 10;
    private readonly int[] _allowedPageLength = [_smallestPageLength, 25, 50];
    private readonly WebshopDbContext _db = db;
    private readonly UserManager<ShopUser> _usermanager = usermanager;

    [HttpGet]
    [Route("Paged")]
    public async Task<Paginated<ProductDTO>> GetPaged(int pageNumber = 1, int pageSize = _smallestPageLength)
    {

        if (!_allowedPageLength.Contains(pageSize)) pageSize = _smallestPageLength;
        if (pageNumber < 1) pageNumber = 1;

        List<ProductDTO> productSlice = await _db.Products
            .AsNoTracking()
            .OrderBy(product => product.Name)
            .Take(pageSize)
            .Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                SellerId = product.SellerId
            })
            .ToListAsync();

        int totalRecordCount = await _db.Products.CountAsync();
        int pageCount = (int)Math.Ceiling((double)totalRecordCount / pageSize);

        return new Paginated<ProductDTO>
        {
            PageNumber = pageNumber,
            PageCount = pageCount,
            PageSize = pageSize,
            TotalRecordCount = totalRecordCount,
            Body = productSlice
        };
    }

    [HttpPost]
    [Authorize(Policy = "Seller")]
    public async Task<IActionResult> Post([FromBody] CreateProductDTO body)
    {
        ShopUser? user = await _usermanager.GetUserAsync(User);
        if (user!.SellerId == null) return BadRequest();

        Guid newId = Guid.NewGuid();
        await _db.Products.AddAsync(new Product
        {
            Id = newId,
            Name = body.Name,
            Price = body.Price,
            SellerId = user.SellerId.Value,
        });

        var result = await _db.SaveChangesAsync();
        return result > 0 ? Ok(newId) : StatusCode(500, "Failed to register product.");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> Get(Guid id)
    {
        var product = await _db.Products
        .Where(product => product.Id == id)
        .Select(product => new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            SellerId = product.SellerId
        })
        .FirstOrDefaultAsync();

        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPatch("{id}")]
    [Authorize(Policy =  "Seller")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] CreateProductDTO updatedProduct)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return BadRequest();

        if (!(await VerifySeller(product))) return BadRequest();

        try
        {
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            await _db.SaveChangesAsync();

            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Seller")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return Ok();

        if (!(await VerifySeller(product))) return BadRequest();

        try
        {
            _db.Products.Remove(product);
            var rows = await _db.SaveChangesAsync();
            return rows > 0 ? Ok() : StatusCode(500, "Failed to delete product");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    private async Task<bool> VerifySeller(Product product)
    {
        var user = await _usermanager.GetUserAsync(User);
        return user?.SellerId == product.SellerId;
    }

    public class CreateProductDTO
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }
    }
}
