using System.ComponentModel.DataAnnotations;

namespace WebshopPractice.Server.Data.Models;

public class ShoppingCart
{
    [Key]
    public Guid Id { get; set; }
    public IEnumerable<Product>? Products { get; set; }

    //user coupling
    public required string ShopUserId { get; set; }
    public required ShopUser ShopUser { get; set; }
}
