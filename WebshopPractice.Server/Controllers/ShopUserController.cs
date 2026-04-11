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
public class ShopUserController(
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
    public async Task<PaginatedTable<ShopUserDTO>> GetPaged(int pageNumber = 1, int pageSize = _smallestPageLength)
    {
        if (!_allowedPageLength.Contains(pageSize)) pageSize = _smallestPageLength;
        if (pageNumber < 1) pageNumber = 1;

        List<ShopUserDTO> userSlice = await _db.ShopUsers
            .AsNoTracking()
            .OrderBy(u => u.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new ShopUserDTO
            {
                UserId = u.Id,
                Email = u.Email,
                Name = u.Name,
                UserLevel = u.UserLevel
            })
            .ToListAsync();

        int totalRecordCount = await _db.ShopUsers.CountAsync();
        int pageCount = (int)Math.Ceiling((double)totalRecordCount / pageSize);

        return new PaginatedTable<ShopUserDTO>
        {
            PageNumber = pageNumber,
            PageCount = pageCount,
            PageSize = pageSize,
            TotalRecordCount = totalRecordCount,
            Body = userSlice
        };
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ShopUserDTO>> Get(string id)
    {
        var user = await _db.ShopUsers
        .Where(u => u.Id == id)
        .Select(u => new ShopUserDTO
        {
            UserId = u.Id,
            Email = u.Email,
            Name = u.Name,
            UserLevel = u.UserLevel
        })
        .FirstOrDefaultAsync();

        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> Patch(string id, [FromBody] ShopUserDTO updatedUser)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != updatedUser.UserId) return BadRequest();

        //if the currently logged in user is not an admin then they cannot update anyone excepts themselves
        bool isAdmin = User.Claims.FirstOrDefault(c => c.Type == "UserLevel")?.Value == UserLevel.Admin.ToString();
        ShopUser? currentUser = await _userManager.GetUserAsync(User);
        if ( !isAdmin && currentUser?.Id != id)
        {
            return BadRequest("You are not authorized to update this user.");
        }

        try
        {
            await _db.ShopUsers
            .Where(u => u.Id == updatedUser.UserId)
            .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Email, p => updatedUser.Email)
                .SetProperty(p => p.Name, p => updatedUser.Name)
            );

            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var rowsAffected = await _db.ShopUsers
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

            if (rowsAffected == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}
