﻿using DM._Domain.CustomerDiscountAgg;
using DM._Infrastructure.EFCore;
using DM._Infrastructure.EFCore.Repository;
using DM.Application.Contracts.CustomerDiscount;
using DM.Application.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Infrastructure.Configuration
{
    public class DiscountManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DiscountContext>(x => x.UseSqlServer(connectionString));

            services.AddScoped<ICustomerDiscountApplication, CustomerDiscountApplication>();
            services.AddScoped<ICustomerDiscountRepository, CustomerDiscountRepository>();

        }
    }
}