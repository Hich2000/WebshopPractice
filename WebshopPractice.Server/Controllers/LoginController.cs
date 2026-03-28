using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebshopPractice.Server.Data.Repositories;

namespace WebshopPractice.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController(UserRepository userRepository) : Controller
{
    private readonly UserRepository _userRepository = userRepository;

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> me()
    {
        var username = User.Identity?.Name;
        return Ok(new
        {
            username
        });
    }

    [HttpPost]
    [Route("[controller]")]
    public async Task<IActionResult> Login([FromBody] LoginRequestBody body)
    {
        var user = await _userRepository.GetUser(body.Username);
        if (user == null || user.Password != body.Password)
        {
            return Unauthorized("Invalid credentials");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    public class LoginRequestBody
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
