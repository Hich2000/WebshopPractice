using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Data.Context;

public class WebshopDbContext(DbContextOptions<WebshopDbContext> options)
        : IdentityDbContext<ShopUser, IdentityRole, string>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ShopUser> ShopUsers { get; set; }
    public DbSet<SellerInfo> SellerInfo { get; set; }

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

        //set created at default value
        builder.Entity<SellerInfo>()
            .Property(s => s.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        //fixing on delete behaviour for seller info
        builder.Entity<ShopUser>()
            .HasOne(u => u.SellerInfo)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.SellerInfoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
