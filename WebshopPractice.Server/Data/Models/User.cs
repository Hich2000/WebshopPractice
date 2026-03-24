using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebshopPractice.Server.Data.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required UserLevel UserLevel { get; set; }
    public ShoppingCart? ShoppingCart { get; set; }
}

public enum UserLevel
{
    Customer,
    Admin
}