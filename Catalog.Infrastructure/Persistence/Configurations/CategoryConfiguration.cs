﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(x => x.ParentCategory)
                .WithMany(x => x.ChildCategories)
                .HasForeignKey(x => x.ParentCategoryId)
                .OnDelete(DeleteBehavior.NoAction)
                ;


            builder.HasMany(c => c.ChildProducts)
                        .WithOne(p => p.Category)
                        .HasForeignKey(p => p.CategoryId)
                        .IsRequired();
        }
    }
}
