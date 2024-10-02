using Microsoft.EntityFrameworkCore;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    { }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Payment> Payments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // fluent api for Customer
        modelBuilder.Entity<Customer>(customer =>
        {
            customer.HasKey(c => c.CustomerId);
            customer.Property(c => c.CustomerId).HasDefaultValueSql("uuid_generate_v4()");
            customer.Property(c => c.FirstName).IsRequired();
            customer.Property(c => c.LastName).IsRequired();
            customer.Property(c => c.Email).IsRequired().HasMaxLength(100);
            customer.HasIndex(c => c.Email).IsUnique();
            customer.Property(c => c.Password).IsRequired();
            customer.Property(c => c.Phone).IsRequired();
        });

        // fluent api for Address
        modelBuilder.Entity<Address>(address =>
        {
            address.HasKey(a => a.AddressId);
            address.Property(a => a.AddressId).HasDefaultValueSql("uuid_generate_v4()");
            address.Property(a => a.AddressName).IsRequired().HasMaxLength(55);
            address.HasIndex(a => a.AddressName).IsUnique();
            address.Property(a => a.StreetName).IsRequired().HasMaxLength(55);
            address.Property(a => a.StreetNumber).IsRequired().HasMaxLength(20);
            address.Property(a => a.City).IsRequired().HasMaxLength(55);
            address.Property(a => a.State).IsRequired().HasMaxLength(55);

        });

        modelBuilder.Entity<Address>().Navigation(s => s.Customer).AutoInclude();
        modelBuilder.Entity<Payment>().Navigation(s => s.Customer).AutoInclude();


        // fluent api for Payment
        modelBuilder.Entity<Payment>(payment =>
        {
            payment.HasKey(p => p.PaymentId);
            payment.Property(p => p.PaymentId).HasDefaultValueSql("uuid_generate_v4()");
            payment.HasIndex(p => p.CardNumber).IsUnique();
            payment.Property(p => p.CardNumber).IsRequired();
            payment.Property(p => p.PaymentMethod).IsRequired();
            payment.Property(p => p.TotalPrice).IsRequired();

        });
        modelBuilder.HasPostgresEnum<PaymentMethod>();

        // one customer has many addresses
        modelBuilder.Entity<Customer>()
        .HasMany(c => c.Addresses)
        .WithOne(a => a.Customer)
        .HasForeignKey(a => a.CustomerId)
        .OnDelete(DeleteBehavior.Cascade);


        // one customer has many payments
        modelBuilder.Entity<Customer>()
        .HasMany(c => c.Payments)
        .WithOne(p => p.Customer)
        .HasForeignKey(p => p.CustomerId)
        .OnDelete(DeleteBehavior.Cascade);

        // one customer has many orders
        // modelBuilder.Entity<Customer>()
        // .HasMany(c => c.Orders)
        // .WithOne(o => o.Customer)
        // .HasForeignKey(o => o.CustomerId)
        // .OnDelete(DeleteBehavior.Cascade);

    }
}