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
    public async Task<IActionResult> Register([FromBody] RegisterRequestBody body)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        //first check if email exists
        var userExists = await _userManager.FindByEmailAsync(body.Email);
        if (userExists != null)
        {
            return Conflict(new { message = "This email is already in use." });
        }

        var newUser = new ShopUser
        {
            UserName = body.Email,
            Email = body.Email,
            Name = body.Name,
            UserLevel = body.UserLevel,
        };

        var result = await _userManager.CreateAsync(newUser, body.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { errors });
        }

        return Ok();
    }

    public class RegisterRequestBody
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public UserLevel UserLevel { get; } = UserLevel.Customer;
    }
}
