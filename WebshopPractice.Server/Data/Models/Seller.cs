using System.ComponentModel.DataAnnotations;

namespace WebshopPractice.Server.Data.Models;

public class Seller
{
    [Key]
    public Guid Id { get; set; }

    public ICollection<ShopUser> Users { get; set; } = [];

    public required string OrganizationName { get; set; }

    public required string Country { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
    public required string Address { get; set; }
    
    // when a seller is made it is pending and they can't do anything yet until an admin verifies them.
    public SellerStatus Verified { get; set; } = SellerStatus.Pending;

    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime? VerifiedAt { get; set; }
}

public enum SellerStatus
{
    Pending,
    Verified,
    Rejected,
    Suspended
}
