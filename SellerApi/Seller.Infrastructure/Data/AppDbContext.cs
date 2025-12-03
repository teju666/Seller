using Microsoft.EntityFrameworkCore;

using Seller.Core.Entities;
 
namespace Seller.Infrastructure.Data;
 
public class AppDbContext : DbContext

{

    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) {}

    public DbSet<Seller> Seller => Set<Seller>();
 
    protected override void OnModelCreating(ModelBuilder b)

    {

        b.Entity<Seller>(e =>

        {

            e.HasKey(p => p.Id);

            e.Property(p => p.Name).IsRequired().HasMaxLength(100);

            e.Property(p => p.Sku).IsRequired().HasMaxLength(30);

            e.HasIndex(p => p.Sku).IsUnique();

            e.Property(p => p.Price).HasColumnType("decimal(18,2)");

            e.HasQueryFilter(p => !p.IsDeleted);

        });

    }

}

 