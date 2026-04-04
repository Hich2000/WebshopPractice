using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Data.DTO;

public class ShopUserDTO
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public UserLevel UserLevel { get; set; }
}
