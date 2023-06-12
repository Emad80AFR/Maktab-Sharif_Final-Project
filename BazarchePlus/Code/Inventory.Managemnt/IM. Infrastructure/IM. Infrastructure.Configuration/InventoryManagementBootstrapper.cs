using FrameWork.Infrastructure.Permission;
using IM._Application.Contracts.Inventory;
using IM._Application.Implementation;
using IM._Domain.InventoryAgg;
using IM._Infrastructure.Configuration.Permissions;
using IM._Infrastructure.EFCore;
using IM._Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IM._Infrastructure.Configuration
{
    public class InventoryManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<InventoryContext>(x => x.UseSqlServer(connectionString));

            services.AddScoped<IInventoryApplication, InventoryApplication>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();

            services.AddScoped<IPermissionExposer, InventoryPermissionExposer>();
        }

    }
}