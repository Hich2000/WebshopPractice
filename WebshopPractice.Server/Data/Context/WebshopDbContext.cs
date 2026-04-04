using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Data.Context;

public class WebshopDbContext(DbContextOptions<WebshopDbContext> options)
        : IdentityDbContext<ShopUser, IdentityRole, string>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ShopUser> ShopUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ShopUser>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
