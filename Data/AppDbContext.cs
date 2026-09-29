using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Models.Customers;
using PaymentGateway.Models.Info;

namespace PaymentGateway.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PaymentGatewayCustomer> Customers { get; init; }
    public DbSet<Address> Addresses { get; init; }
    public DbSet<PhoneNumber> PhoneNumbers { get; init; }
    public DbSet<UserMetadata> Metadatas { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}