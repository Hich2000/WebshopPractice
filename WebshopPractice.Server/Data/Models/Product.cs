using System.ComponentModel.DataAnnotations;

namespace WebshopPractice.Server.Data.Models;

public class Product
{
    [Key]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }

    public Guid SellerId { get; set; }
    public Seller? Seller { get; set; }
}
