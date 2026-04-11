using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Helpers;

public class ShopUserClaimsPrincipalFactory(UserManager<ShopUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) 
    : UserClaimsPrincipalFactory<ShopUser, IdentityRole>(userManager, roleManager, options)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ShopUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("UserLevel", user.UserLevel.ToString()));
        return identity;
    }
}
