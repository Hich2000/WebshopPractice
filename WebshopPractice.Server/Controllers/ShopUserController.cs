using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebshopPractice.Server.Data.Context;
using WebshopPractice.Server.Data.DTO;
using WebshopPractice.Server.Helpers;

namespace WebshopPractice.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ShopUserController(
    WebshopDbContext db
) : Controller
{
    private const int _smallestPageLength = 10;
    private readonly int[] _allowedPageLength = [_smallestPageLength, 25, 50];
    private readonly WebshopDbContext _db = db;

    [HttpGet]
    [Route("Table")]
    public async Task<PaginatedTable<ShopUserDTO>> GetTable(int PageNumber = 1, int PageSize = _smallestPageLength)
    {
        if (!_allowedPageLength.Contains(PageSize)) PageSize = _smallestPageLength;
        if (PageNumber < 1) PageNumber = 1;

        var UserSlice = await _db.ShopUsers
                     .OrderBy(u => u.Id)
                     .Skip((PageNumber - 1) * PageSize)
                     .Take(PageSize)
                     .ToListAsync();

        int TotalRecordCount = await _db.ShopUsers.CountAsync();
        int PageCount = TotalRecordCount / PageSize;

        return new PaginatedTable<ShopUserDTO>
        {
            PageNumber = PageNumber,
            PageCount = PageCount,
            PageSize = PageSize,
            TotalRecordCount = TotalRecordCount,
            Body = UserSlice.Select(u =>
                    new ShopUserDTO
                    {
                        UserId = u.Id,
                        Email = u.Email,
                        Name = u.Name,
                        UserLevel = u.UserLevel
                    }
                ).ToList()
        };
    }
}
