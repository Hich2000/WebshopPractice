using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebshopPractice.Server.Data.DTO;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Controllers;

[ApiController]
[Route("")]
public class LoginController(
    UserManager<ShopUser> userManager,
    SignInManager<ShopUser> signInManager
) : Controller
{
    private readonly UserManager<ShopUser> _userManager = userManager;
    private readonly SignInManager<ShopUser> _signInManager = signInManager;

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ShopUserDTO>> Me()
    {
        ShopUser? user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

        return Ok(new
        {
            id = user.Id,
            name = user.Name,
            email = user.Email,
            level = user.UserLevel.ToString()
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestBody body)
    {
        var user = await _userManager.FindByEmailAsync(body.Email);
        // Todo add propery security on password
        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            body.Password,
            isPersistent: false,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    public class LoginRequestBody
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
