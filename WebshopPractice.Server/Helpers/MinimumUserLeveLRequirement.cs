using Microsoft.AspNetCore.Authorization;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Helpers;

public class MinimumUserLeveLRequirement(UserLevel minimumLevel) : IAuthorizationRequirement
{
    public UserLevel MinimumLevel { get; } = minimumLevel;
}
