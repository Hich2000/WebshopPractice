using Microsoft.AspNetCore.Authorization;
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
    [Route("User")]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterRequestBody body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await CreateUser(body, UserLevel.Customer);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { errors });
        }

        return Ok();
    }

    [HttpPost]
    [Authorize]
    [Route("Admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestBody body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await CreateUser(body, UserLevel.Admin);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { errors });
        }

        return Ok();
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
}
