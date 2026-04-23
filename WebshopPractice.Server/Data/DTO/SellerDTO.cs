using System.ComponentModel.DataAnnotations;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Data.DTO;

public class SellerDTO
{
    public Guid Id { get; set; }

    public IEnumerable<ShopUser> Users { get; set; } = [];

    public required string OrganizationName { get; set; }
    public required string CommerceNumber { get; set; }

    public required string Country { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
    public required string Address { get; set; }

    public SellerStatus Verified { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
}
