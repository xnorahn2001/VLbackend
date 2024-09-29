using Microsoft.EntityFrameworkCore;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    { }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Adress> Adresses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(customer =>
        {
            customer.HasKey(c => c.CustomerId);
            customer.Property(c => c.CustomerId).HasDefaultValueSql("uuid_generate_v4()");
            customer.Property(c => c.FirstName).IsRequired().HasMaxLength(55);
            customer.Property(c => c.LastName).IsRequired().HasMaxLength(55);
            customer.Property(c => c.Email).IsRequired().HasMaxLength(100);
            customer.HasIndex(c => c.Email).IsUnique();
            customer.Property(c => c.Password).IsRequired().HasMaxLength(50);
            customer.Property(c => c.Phone).IsRequired().HasMaxLength(50);
        });
    }

}