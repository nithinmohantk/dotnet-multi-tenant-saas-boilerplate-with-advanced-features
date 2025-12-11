using Microsoft.EntityFrameworkCore;
using SaasBoilerplate.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SaasBoilerplate.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Tenant> Tenants { get; }
        DbSet<Product> Products { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
