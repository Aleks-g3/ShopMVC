using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopMVC.Data.Converters;
using ShopMVC.Models;
using ShopMVC.Models.Orders;
using ShopMVC.Models.Shared;

namespace ShopMVC.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Product> Products { get; set; } = null!;
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
}