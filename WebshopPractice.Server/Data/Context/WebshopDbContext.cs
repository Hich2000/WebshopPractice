using Microsoft.EntityFrameworkCore;
using WebshopPractice.Server.Data.Models;

namespace WebshopPractice.Server.Data.Context;

public class WebshopDbContext : DbContext
{
    public WebshopDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}
