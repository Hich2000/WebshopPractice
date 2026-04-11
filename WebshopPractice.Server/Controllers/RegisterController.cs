using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterController(
    UserManager<ShopUser> usermanager
) : Controller
{
    private readonly UserManager<ShopUser> _userManager = usermanager;

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
        } else
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

    public class ChangePasswordRequestBody
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string VerifyNewPassword { get; set; }
    }
}
