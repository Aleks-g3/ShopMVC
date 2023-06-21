using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopMVC.Data.Converters;
using ShopMVC.Models.Carts;
using ShopMVC.Models.Orders;
using ShopMVC.Models.Shared;

namespace ShopMVC.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartProduct> CartProducts { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderLine> OrderLines { get; set; } = null!;
    public DbSet<Shipment> Shipments { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Payment>().HaveConversion<PaymentConverter>();
        configurationBuilder.Properties<decimal>().HavePrecision(18, 4);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Cart>(entity =>
        {
            entity.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);
        });

        builder.Entity<CartProduct>(entity =>
        {
            entity.HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId);
        });
        
        base.OnModelCreating(builder);
    }
}