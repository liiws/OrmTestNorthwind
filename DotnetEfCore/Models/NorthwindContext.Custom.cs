using DotnetEfCore.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetEfCore.Models
{
    public partial class NorthwindContext : DbContext
    {
        public virtual DbSet<SimpleQueryRow> SimpleQueryRows { get; set; }
        public virtual DbSet<ComplexQueryRow> ComplexQueryRows { get; set; }

        protected void OnCustomModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SimpleQueryRow>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.OrderDate).HasColumnName("OrderDate");
                entity.Property(e => e.Country).HasColumnName("Country");
                entity.Property(e => e.CompanyName).HasColumnName("CompanyName");
            });

            modelBuilder.Entity<ComplexQueryRow>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.Quantity).HasColumnName("Quantity");
                entity.Property(e => e.UnitPrice).HasColumnName("UnitPrice");
                entity.Property(e => e.Discount).HasColumnName("Discount");
                entity.Property(e => e.ShipCountry).HasColumnName("ShipCountry");
                entity.Property(e => e.Country).HasColumnName("Country");
            });
        }
    }
}
