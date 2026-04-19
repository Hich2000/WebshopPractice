using System.ComponentModel.DataAnnotations;

namespace WebshopPractice.Server.Data.Models;

public class Product
{
    [Key]
    public string Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}
