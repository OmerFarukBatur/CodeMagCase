using Core.Entities;
using Core.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<PackagingUnit> PackagingUnits { get; set; }
        public DbSet<SerialNumber> SerialNumbers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CUSTOMER
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Gln).IsRequired().HasMaxLength(13).IsFixedLength();
                entity.HasIndex(e => e.Gln).IsUnique();
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.HasMany(e => e.Products)
                      .WithOne(p => p.Customer)
                      .HasForeignKey(p => p.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // PRODUCT
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Gtin).IsRequired().HasMaxLength(14);
                entity.HasIndex(e => e.Gtin).IsUnique();

                entity.HasMany(e => e.WorkOrders)
                      .WithOne(w => w.Product)
                      .HasForeignKey(w => w.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // WORKORDER
            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.Property(e => e.TargetQuantity).IsRequired();
                entity.ToTable(t => t.HasCheckConstraint("CK_WorkOrder_TargetQuantity_Min", "[TargetQuantity] > 0"));
                entity.Property(e => e.LotNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.SerialStartValue).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ExpiryDate).HasColumnType("datetime2");
                entity.Property(e => e.WorkOrderStatusId).IsRequired();


                entity.HasMany(e => e.SerialNumbers)
                      .WithOne(s => s.WorkOrder)
                      .HasForeignKey(s => s.WorkOrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // SERIALNUMBER
            modelBuilder.Entity<SerialNumber>(entity =>
            {
                entity.Property(e => e.SerialValue).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Gs1FullString).IsRequired().HasMaxLength(200);
                entity.HasIndex(e => new { e.WorkOrderId, e.SerialValue }).IsUnique();

                entity.HasOne(s => s.WorkOrder)
                      .WithMany(w => w.SerialNumbers)
                      .HasForeignKey(s => s.WorkOrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.PackagingUnit)
                      .WithMany(p => p.SerialNumbers)
                      .HasForeignKey(s => s.PackagingUnitId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // PACKAGINGUNIT
            modelBuilder.Entity<PackagingUnit>(entity =>
            {
                entity.Property(e => e.Sscc).IsRequired().HasMaxLength(18).IsFixedLength();
                entity.HasIndex(e => e.Sscc).IsUnique();
                entity.Property(e => e.PackagingLevelId).IsRequired();

                entity.HasOne(p => p.Parent)
                      .WithMany(p => p.Children)
                      .HasForeignKey(p => p.ParentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}