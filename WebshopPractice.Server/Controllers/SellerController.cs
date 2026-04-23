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
public class SellerController(
    UserManager<ShopUser> userManager,
    WebshopDbContext db
) : Controller
{

    private const int _smallestPageLength = 10;
    private readonly int[] _allowedPageLength = [_smallestPageLength, 25, 50];
    private readonly WebshopDbContext _db = db;
    private readonly UserManager<ShopUser> _userManager = userManager;

    [HttpGet]
    [Route("Paged")]
    [Authorize(Policy = "Admin")]
    public async Task<PaginatedTable<SellerDTO>> GetPaged(int pageNumber = 1, int pageSize = _smallestPageLength)
    {
        if (!_allowedPageLength.Contains(pageSize)) pageSize = _smallestPageLength;
        if (pageNumber < 1) pageNumber = 1;

        List<SellerDTO> sellerSlice = await _db.Seller
            .AsNoTracking()
            .OrderBy(seller => seller.OrganizationName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(seller => new SellerDTO
            {
                Id = seller.Id,
                OrganizationName = seller.OrganizationName,
                CommerceNumber = seller.CommerceNumber,
                Country = seller.Country,
                City = seller.City,
                PostalCode = seller.PostalCode,
                Address = seller.Address,
                Verified = seller.Verified,
                CreatedAt = seller.CreatedAt,
                VerifiedAt = seller.VerifiedAt,
            })
            .ToListAsync();

        int totalRecordCount = await _db.Seller.CountAsync();
        int pageCount = (int)Math.Ceiling((double)totalRecordCount / pageSize);

        return new PaginatedTable<SellerDTO>
        {
            PageNumber = pageNumber,
            PageCount = pageCount,
            PageSize = pageSize,
            TotalRecordCount = totalRecordCount,
            Body = sellerSlice
        };
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ShopUserDTO>> Get(Guid id)
    {
        var seller = await _db.Seller
        .Where(seller => seller.Id == id)
        .Select(seller => new SellerDTO
        {
            Id = seller.Id,
            OrganizationName = seller.OrganizationName,
            CommerceNumber = seller.CommerceNumber,
            Country = seller.Country,
            City = seller.City,
            PostalCode = seller.PostalCode,
            Address = seller.Address,
            Verified = seller.Verified,
            CreatedAt = seller.CreatedAt,
            VerifiedAt = seller.VerifiedAt,
        })
        .FirstOrDefaultAsync();

        if (seller == null) return NotFound();
        return Ok(seller);
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> Patch(Guid id, [FromBody] SellerDTO updatedSeller)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != updatedSeller.Id) return BadRequest();

        //if the currently logged in user is not an admin then they cannot update anyone excepts themselves
        bool isAdmin = User.Claims.FirstOrDefault(c => c.Type == "UserLevel")?.Value == UserLevel.Admin.ToString();
        ShopUser? currentUser = await _userManager.GetUserAsync(User);
        if (!isAdmin && currentUser?.SellerId != id)
        {
            return Unauthorized("You are not authorized to update this seller.");
        }

        try
        {
            await _db.Seller
            .Where(seller => seller.Id == updatedSeller.Id)
            .ExecuteUpdateAsync(seller => seller
                .SetProperty(prop => prop.OrganizationName, prop => updatedSeller.OrganizationName)
                .SetProperty(prop => prop.Country, prop => updatedSeller.Country)
                .SetProperty(prop => prop.City, prop => updatedSeller.City)
                .SetProperty(prop => prop.PostalCode, prop => updatedSeller.PostalCode)
                .SetProperty(prop => prop.Address, prop => updatedSeller.Address)
            );

            return Ok(updatedSeller);
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
        //if the currently logged in user is not an admin then they cannot update anyone excepts themselves
        bool isAdmin = User.Claims.FirstOrDefault(c => c.Type == "UserLevel")?.Value == UserLevel.Admin.ToString();
        ShopUser? currentUser = await _userManager.GetUserAsync(User);
        if (!isAdmin && currentUser?.SellerId != id)
        {
            return Unauthorized("You are not authorized to perform this action.");
        }

        Seller? sellerToDelete = await _db.Seller.FindAsync(id);
        if (sellerToDelete == null)
        {
            return NotFound();
        }

        try
        {
            await _db.ShopUsers
                .Where(user => user.SellerId == id)
                .ExecuteUpdateAsync( user => user
                    .SetProperty(prop => prop.SellerId,  prop => null)
                    .SetProperty(prop => prop.SellerId,  prop => null)
                    .SetProperty(prop => prop.UserLevel, prop => UserLevel.Customer)
                );

            await _db.Seller
                .Where(seller => seller.Id == id)
                .ExecuteDeleteAsync();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
