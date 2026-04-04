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

        // setup email to be unique on database layer
        builder.Entity<ShopUser>()
            .HasIndex(u => u.NormalizedEmail)
            .IsUnique();

        //setup email to be required
        builder.Entity<ShopUser>()
            .Property(u => u.Email)
            .IsRequired();
    }
}
