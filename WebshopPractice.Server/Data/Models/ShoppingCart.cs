using System.ComponentModel.DataAnnotations;

namespace WebshopPractice.Server.Data.Models;

public class ShoppingCart
{
    [Key]
    public int Id { get; set; }
    public IEnumerable<Product>? Products { get; set; }

    //user coupling
    public int UserId { get; set; }
    public required User User { get; set; }
}
