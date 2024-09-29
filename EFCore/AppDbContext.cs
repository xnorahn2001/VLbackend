using Microsoft.EntityFrameworkCore;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    { }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<Address>(address =>
        {
            address.HasKey(a => a.AdressId);
            address.Property(a => a.AdressId).HasDefaultValueSql("uuid_generate_v4()");
            address.Property(a => a.CustomerId).HasDefaultValueSql("uuid_generate_v4()");
            address.Property(a => a.AddressName).IsRequired().HasMaxLength(55);
            address.HasIndex(a => a.AddressName).IsUnique();
            address.Property(a => a.StreetName).IsRequired().HasMaxLength(55);
            address.Property(a => a.StreetNumber).IsRequired().HasMaxLength(20);
            address.Property(a => a.City).IsRequired().HasMaxLength(55);
            address.Property(a => a.State).IsRequired().HasMaxLength(55);

        });

    }
}