using CM._Application.Contracts.Comment;
using CM._Application.Implementation;
using CM._Domain.CommentAgg;
using CM._Infrastructure.Configuration.Permissions;
using CM._Infrastructure.EFCore;
using CM._Infrastructure.EFCore.Repository;
using FrameWork.Infrastructure.Permission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CM._Infrastructure.Configuration
{
    public class CommentManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<CommentContext>(x => x.UseSqlServer(connectionString ));

            services.AddScoped<ICommentApplication, CommentApplication>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            services.AddScoped<IPermissionExposer, CommentPermissionExposer>();
        }
    }
}