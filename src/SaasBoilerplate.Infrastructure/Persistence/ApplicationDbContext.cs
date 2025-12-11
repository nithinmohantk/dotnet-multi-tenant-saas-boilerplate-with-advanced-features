using Microsoft.EntityFrameworkCore;
using SaasBoilerplate.Application.Common.Interfaces;
using SaasBoilerplate.Domain.Common;
using SaasBoilerplate.Domain.Entities;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SaasBoilerplate.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ITenantService _tenantService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);

            ConfigureGlobalFilters(builder);
        }

        private void ConfigureGlobalFilters(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(IMustHaveTenant).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ApplicationDbContext)
                        .GetMethod(nameof(ConfigureTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(this, new object[] { builder });
                }
            }
        }

        private void ConfigureTenantFilter<T>(ModelBuilder builder) where T : class, IMustHaveTenant
        {
            builder.Entity<T>().HasQueryFilter(e => e.TenantId == _tenantService.TenantId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    if (string.IsNullOrEmpty(entry.Entity.TenantId) && !string.IsNullOrEmpty(_tenantService.TenantId))
                    {
                        entry.Entity.TenantId = _tenantService.TenantId;
                    }
                }
            }

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = "System"; // TODO: Implement CurrentUser
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "System";
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Here we could switch connection string based on _tenantService.CurrentTenant.ConnectionString
            // But usually this is done at the Dependency Injection level or using a factory.
            // For this boilerplate, if the tenant has a specific connection string, 
            // the DbContext might need to be re-initialized or configured differently.
            // A common pattern is checking if the Tenant has a ConnectionString and using it.
            
            if (_tenantService.CurrentTenant != null && !string.IsNullOrEmpty(_tenantService.CurrentTenant.ConnectionString))
            {
                 optionsBuilder.UseSqlServer(_tenantService.CurrentTenant.ConnectionString);
            }
            
            base.OnConfiguring(optionsBuilder);
        }
    }
}
