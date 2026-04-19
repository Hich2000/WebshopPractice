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
public class ProductController(WebshopDbContext db) : ControllerBase
{
    private const int _smallestPageLength = 10;
    private readonly int[] _allowedPageLength = [_smallestPageLength, 25, 50];
    private readonly WebshopDbContext _db = db;

    [HttpGet]
    [Route("Paged")]
    public async Task<PaginatedTable<ProductDTO>> GetPaged(int pageNumber = 1, int pageSize = _smallestPageLength)
    {

        if (!_allowedPageLength.Contains(pageSize)) pageSize = _smallestPageLength;
        if (pageNumber < 1) pageNumber = 1;

        //todo this will be made paged like
        List<ProductDTO> productSlice = await _db.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Take(20)
            .Select(u => new ProductDTO
            {
                Id = u.Id,
                Name = u.Name,
                Price = u.Price,
            })
            .ToListAsync();

        int totalRecordCount = await _db.Products.CountAsync();
        int pageCount = (int)Math.Ceiling((double)totalRecordCount / pageSize);

        return new PaginatedTable<ProductDTO>
        {
            PageNumber = pageNumber,
            PageCount = pageCount,
            PageSize = pageSize,
            TotalRecordCount = totalRecordCount,
            Body = productSlice
        };
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] ProductDTO newProduct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _db.Products.AddAsync(new Product
        {
            Id = Guid.NewGuid(),
            Name = newProduct.Name,
            Price = newProduct.Price
        });

        try
        {
            var result = await _db.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, "Failed to register product.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> Get(Guid id)
    {
        var product = await _db.Products
        .Where(p => p.Id == id)
        .Select(p => new ProductDTO
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        })
        .FirstOrDefaultAsync();

        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> Patch(Guid id, [FromBody] ProductDTO updatedProduct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != updatedProduct.Id) return BadRequest();
        
        //todo when seller accounts are setup check if the user is an admin or the seller that owns the product

        try
        {
            await _db.Products
            .Where(p => p.Id == updatedProduct.Id)
            .ExecuteUpdateAsync(p => p
                .SetProperty(prop => prop.Name, prop => updatedProduct.Name)
                .SetProperty(prop => prop.Price, prop => updatedProduct.Price)
            );

            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        //todo when seller accounts are setup check if the user is an admin or the seller that owns the product

        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        try
        {
            _db.Products.Remove(product);
            var rows = await _db.SaveChangesAsync();

            if (rows > 0)
            {
                return NoContent();
            } else
            {
                return StatusCode(500, "Failed to delete product");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
