using AM._Application.Contracts.Account;
using AM._Application.Contracts.Role;
using AM._Application.Implementation;
using AM._Domain.AccountAgg;
using AM._Domain.RollAgg;
using AM._Infrastructure.Configuration.Permissions;
using AM._Infrastructure.EFCore;
using AM._Infrastructure.EFCore.Repository;
using FrameWork.Infrastructure.Permission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AM._Infrastructure.Configuration
{
    public class AccountManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<AccountContext>(x => x.UseSqlServer(connectionString));

            services.AddScoped<IAccountApplication, AccountApplication>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<IRoleApplication, RoleApplication>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IPermissionExposer, AccountPermissionExposer>();
        }

    }
}