using System.Reflection;
using Catalog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Catalog.Domain.Entities;
using Catalog.Domain.Common;

namespace Catalog.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        DbSet<Category> IApplicationDbContext.Categories => Set<Category>();


        DbSet<Product> IApplicationDbContext.Products => Set<Product>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "InsertUser";
                        entry.Entity.Created = DateTime.Now; 
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = "EditUser";
                        entry.Entity.LastModified = DateTime.Now; 
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
