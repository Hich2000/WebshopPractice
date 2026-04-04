using Microsoft.AspNetCore.Identity;

namespace WebshopPractice.Server.Data.Models;

public class ShopUser: IdentityUser
{
    public required string Name { get; set; }
    public required UserLevel UserLevel { get; set; }
    public ShoppingCart? ShoppingCart { get; set; }
}

public enum UserLevel
{
    Customer,
    Seller,
    Admin
}