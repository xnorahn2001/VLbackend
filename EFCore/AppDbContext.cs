using Microsoft.EntityFrameworkCore;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    { }
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetails> OrderDetailses { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // fluent api for User
        modelBuilder.Entity<User>(user =>
        {
            user.HasKey(c => c.UserId);
            user.Property(c => c.UserId).HasDefaultValueSql("uuid_generate_v4()");
            user.Property(c => c.FirstName).IsRequired();
            user.Property(c => c.LastName).IsRequired();
            user.Property(c => c.Email).IsRequired().HasMaxLength(100);
            user.HasIndex(c => c.Email).IsUnique();
            user.Property(c => c.Password).IsRequired();
            user.Property(c => c.Phone).IsRequired();
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

        // fluent api for Product
        modelBuilder.Entity<Product>(product =>
        {
            product.HasKey(p => p.ProductId);
            product.Property(p => p.ProductId).HasDefaultValueSql("uuid_generate_v4()");
            product.HasIndex(p => p.Image).IsUnique();

        });

        // fluent api for Order
        modelBuilder.Entity<Order>(OrdersEntity =>
        {
            OrdersEntity.HasKey(s => s.OrderId);
            OrdersEntity.Property(o => o.OrderId).HasDefaultValueSql("uuid_generate_v4()");
        });

        // fluent api for OrderDetails
        modelBuilder.Entity<OrderDetails>(OrdersEntity =>
        {
            OrdersEntity.HasKey(s => s.OrdersDetailesId);
            OrdersEntity.Property(o => o.OrdersDetailesId).HasDefaultValueSql("uuid_generate_v4()");
        });

        // fluent api for Shipment
        modelBuilder.Entity<Shipment>(shipment =>
        {
            shipment.HasKey(s => s.ShipmentId);
            shipment.Property(o => o.ShipmentId).HasDefaultValueSql("uuid_generate_v4()");
        });

        modelBuilder.HasPostgresEnum<PaymentMethod>();
        modelBuilder.HasPostgresEnum<Size>();
        modelBuilder.HasPostgresEnum<Color>();
        modelBuilder.HasPostgresEnum<Material>();
        modelBuilder.HasPostgresEnum<Status>();

        // one users has many addresses
        modelBuilder.Entity<User>()
        .HasMany(c => c.Addresses)
        .WithOne(a => a.User)
        .HasForeignKey(a => a.UserId)
        .OnDelete(DeleteBehavior.Cascade);


        // one user has many payments
        modelBuilder.Entity<User>()
        .HasMany(c => c.Payments)
        .WithOne(p => p.User)
        .HasForeignKey(p => p.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        // one user has many orders
        modelBuilder.Entity<User>()
        .HasMany(c => c.Orders)
        .WithOne(o => o.User)
        .HasForeignKey(o => o.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        // one order has many order details 
        modelBuilder.Entity<Order>()
        .HasMany(c => c.OrderDetails)
        .WithOne(o => o.Order)
        .HasForeignKey(o => o.OrderId)
        .OnDelete(DeleteBehavior.Cascade);

        // one order has one shipment
        modelBuilder.Entity<Order>()
            .HasOne(u => u.Shipment)
            .WithOne(p => p.Order)
            .HasForeignKey<Shipment>(p => p.OrderId);

        // one order has one payment
        modelBuilder.Entity<Order>()
            .HasOne(u => u.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId);

        // one product has many order details
        modelBuilder.Entity<Product>()
        .HasMany(c => c.OrderDetailses)
        .WithOne(p => p.Product)
        .HasForeignKey(p => p.ProductId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}