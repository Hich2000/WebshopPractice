using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        var result = await CreateUser(body, UserLevel.Customer);
        return result.Succeeded ? Ok(result.UserId) : BadRequest(result.Errors);
    }

    [HttpPost]
    [Authorize]
    [Route("admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestBody body)
    {
        var result = await CreateUser(body, UserLevel.Admin);
        return result.Succeeded ? Ok(result.UserId) : BadRequest(result.Errors);
    }

    [HttpPost]
    [Route("seller")]
    public async Task<IActionResult> RegisterSeller([FromBody] RegisterSellerRequestBody body)
    {
        ShopUser? user = await _userManager.FindByIdAsync(body.UserId.ToString());
        if (user == null)
            return BadRequest(new { error = "Could not find user to register for this seller." });

        if (user.SellerId != null)
            return BadRequest(new { error = "Cannot link user to this seller." });

        Guid newSellerId = Guid.NewGuid();
        Seller newSeller = new Seller
        {
            Id = newSellerId,
            OrganizationName = body.OrganizationName,
            CommerceNumber = body.CommerceNumber,
            Country = body.Country,
            City = body.City,
            PostalCode = body.PostalCode,
            Address = body.Address,
            Users = [user]
        };

        await _db.Seller.AddAsync(newSeller);
        user.Seller = newSeller;
        user.SellerId = newSellerId;
        user.UserLevel = UserLevel.Seller;
        var result = await _db.SaveChangesAsync();
        return result > 0 ? Ok(newSellerId) : BadRequest("Failed to register Seller.");
    }

    [HttpPatch]
    [Authorize]
    [Route("changeOwnPassword")]
    public async Task<IActionResult> ChangeOwnPassword([FromBody] ChangePasswordRequestBody body)
    {
        var user = await _userManager.GetUserAsync(User);
        var correctPassword = await _userManager.CheckPasswordAsync(user, body.OldPassword);

        if (!correctPassword) return BadRequest("Password is not correct.");
        if (body.NewPassword != body.VerifyNewPassword) return BadRequest("New password does not match verification field.");

        var result = await _userManager.ChangePasswordAsync(user, body.OldPassword, body.NewPassword);
        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }

    private async Task<CreateUserResult> CreateUser(RegisterRequestBody body, UserLevel userLevel)
    {
        //first check if user exists
        var userExists = await _userManager.FindByEmailAsync(body.Email);
        if (userExists != null)
        {
            return new CreateUserResult
            {
                Succeeded = false,
                Errors = new[]
                {
                    new IdentityError
                    {
                        Code = "DuplicateEmail",
                        Description = "This email is already in use."
                    }
                }
            };
        }

        var newUser = new ShopUser
        {
            UserName = body.Email,
            Email = body.Email,
            Name = body.Name,
            UserLevel = userLevel,
        };

        var result = await _userManager.CreateAsync(newUser, body.Password);
        return new CreateUserResult
        {
            Succeeded = result.Succeeded,
            UserId = result.Succeeded ? newUser.Id : null,
            Errors = result.Errors
        };
    }

    public class CreateUserResult
    {
        public bool Succeeded { get; set; }
        public string? UserId { get; set; }
        public IEnumerable<IdentityError>? Errors { get; set; }
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
        public required string CommerceNumber { get; set; }
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
