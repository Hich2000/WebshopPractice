using Microsoft.AspNetCore.Authorization;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Helpers;

public class MinimumUserLevelHandler: AuthorizationHandler<MinimumUserLeveLRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumUserLeveLRequirement requirement)
    {
        var claim = context.User.FindFirst("UserLevel");
        if (claim == null) return Task.CompletedTask;

        if (Enum.TryParse<UserLevel>(claim.Value, out var userLevel))
        {
            if (userLevel >= requirement.MinimumLevel) 
                context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
