using Microsoft.AspNetCore.Identity;

namespace WebshopPractice.Server.Data.Models;

public class ShopUser: IdentityUser
{
    public required string Name { get; set; }
    public override string Email { get; set; } = null!;
    public required UserLevel UserLevel { get; set; }

    public Guid? ShoppingCartId { get; set; }
    public ShoppingCart? ShoppingCart { get; set; }

    public Guid? SellerInfoId { get; set; }
    public SellerInfo? SellerInfo { get; set; }
}

public enum UserLevel
{
    Customer,
    Seller,
    Admin
}