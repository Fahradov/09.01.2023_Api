using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities;

namespace Store.Data.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Image).HasMaxLength(100);
            builder.Property(x => x.SalePrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CostPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.DiscountPercent).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Category).WithMany(x => x.Products);
        }
    }
}

