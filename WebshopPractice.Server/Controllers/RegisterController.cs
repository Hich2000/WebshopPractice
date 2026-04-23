using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebshopPractice.Server.Data.Context;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterController(
    UserManager<ShopUser> usermanager,
    WebshopDbContext db
) : Controller
{
    private readonly UserManager<ShopUser> _userManager = usermanager;
    private readonly WebshopDbContext _db = db;

    [HttpPost]
    [Route("user")]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterRequestBody body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await CreateUser(body, UserLevel.Customer);

        if (!result.Succeeded)
        {
            return Conflict(result.Errors);
        }

        return Ok();
    }

    [HttpPost]
    [Authorize]
    [Route("admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestBody body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await CreateUser(body, UserLevel.Admin);

        if (!result.Succeeded)
        {
            return Conflict(result.Errors);
        }

        return Ok();
    }

    [HttpPost]
    [Route("seller")]
    public async Task<IActionResult> RegisterSeller([FromBody] RegisterSellerRequestBody body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        ShopUser? user = await _userManager.FindByIdAsync(body.UserId.ToString());
        if (user == null)
        {
            return BadRequest("Could not find user to register for this seller.");
        }

        Guid newSellerId = Guid.NewGuid();
        SellerInfo newSeller = new SellerInfo
        {
            Id = newSellerId,
            OrganizationName = body.OrganizationName,
            Country = body.Country,
            City = body.City,
            PostalCode = body.PostalCode,
            Address = body.Address,
            Users = [user]
        };

        await _db.SellerInfo.AddAsync(newSeller);

        user.SellerInfo = newSeller;
        user.SellerInfoId = newSellerId;
        user.UserLevel = UserLevel.Seller;

        try
        {
            var result = await _db.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, "Failed to register Seller.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPatch]
    [Authorize]
    [Route("changeOwnPassword")]
    public async Task<IActionResult> ChangeOwnPassword([FromBody] ChangePasswordRequestBody body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.GetUserAsync(User);
        var correctPassword = await _userManager.CheckPasswordAsync(user, body.OldPassword);

        if (!correctPassword) return BadRequest("Password is not correct.");
        if (body.NewPassword != body.VerifyNewPassword) return BadRequest("New password does not match verification field.");

        var result = await _userManager.ChangePasswordAsync(user, body.OldPassword, body.NewPassword);

        if (result.Succeeded)
        {
            return Ok();
        }
        else
        {
            return Conflict(result.Errors);
        }
    }

    private async Task<IdentityResult> CreateUser(RegisterRequestBody body, UserLevel userLevel)
    {
        //first check if email exists
        var userExists = await _userManager.FindByEmailAsync(body.Email);
        if (userExists != null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "This email is already in use."
            });
        }

        var newUser = new ShopUser
        {
            UserName = body.Email,
            Email = body.Email,
            Name = body.Name,
            UserLevel = userLevel,
        };

        var result = await _userManager.CreateAsync(newUser, body.Password);
        return result;
    }

    public class RegisterRequestBody
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterSellerRequestBody
    {
        public required Guid UserId { get; set; }
        public required string OrganizationName { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string PostalCode { get; set; }
        public required string Address { get; set; }
    }

    public class ChangePasswordRequestBody
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string VerifyNewPassword { get; set; }
    }
}
