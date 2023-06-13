using AM._Application.Contracts.Auction;
using AM._Application.Implementation;
using AM._Domain.AuctionAgg;
using AM._Infrastructure.EFCore;
using AM._Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AM._Infrastructure.Configuration
{
    public class AuctionManagementBootstrapper
    {

        public static void Configure(IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<AuctionContext>(x => x.UseSqlServer(connectionString));

            services.AddScoped<IAuctionApplication, AuctionApplication>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
        }
    }
}