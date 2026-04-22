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

        List<SellerDTO> sellerSlice = await _db.SellerInfo
            .AsNoTracking()
            .OrderBy(s => s.OrganizationName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new SellerDTO
            {
                Id = s.Id,
                OrganizationName = s.OrganizationName,
                Country = s.Country,
                City = s.City,
                PostalCode = s.PostalCode,
                Address = s.Address,
                Verified = s.Verified,
                CreatedAt = s.CreatedAt,
                VerifiedAt = s.VerifiedAt,
            })
            .ToListAsync();

        int totalRecordCount = await _db.SellerInfo.CountAsync();
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
        var seller = await _db.SellerInfo
        .Where(u => u.Id == id)
        .Select(s => new SellerDTO
        {
            Id = s.Id,
            OrganizationName = s.OrganizationName,
            Country = s.Country,
            City = s.City,
            PostalCode = s.PostalCode,
            Address = s.Address,
            Verified = s.Verified,
            CreatedAt = s.CreatedAt,
            VerifiedAt = s.VerifiedAt,
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
        if (!isAdmin && currentUser?.SellerInfoId != id)
        {
            return Unauthorized("You are not authorized to update this seller.");
        }

        try
        {
            await _db.SellerInfo
            .Where(s => s.Id == updatedSeller.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.OrganizationName, p => updatedSeller.OrganizationName)
                .SetProperty(p => p.Country, p => updatedSeller.Country)
                .SetProperty(p => p.City, p => updatedSeller.City)
                .SetProperty(p => p.PostalCode, p => updatedSeller.PostalCode)
                .SetProperty(p => p.Address, p => updatedSeller.Address)
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
        if (!isAdmin && currentUser?.SellerInfoId != id)
        {
            return Unauthorized("You are not authorized to perform this action.");
        }

        SellerInfo? sellerToDelete = await _db.SellerInfo.FindAsync(id);
        if (sellerToDelete == null)
        {
            return NotFound();
        }

        try
        {
            await _db.SellerInfo
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
