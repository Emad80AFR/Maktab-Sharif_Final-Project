using AM._Infrastructure.Configuration;
using FrameWork.Application;
using IM._Infrastructure.Configuration;
using SM._Infrastructure.Configuration;

namespace WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            var connectionString = builder.Configuration.GetConnectionString("BazarchePlusDb");
            ShopManagementBootstrapper.Configure(builder.Services,connectionString);
            InventoryManagementBootstrapper.Configure(builder.Services, connectionString);
            AccountManagementBootstrapper.Configure(builder.Services,connectionString);


            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IFileUploader, FileUploader>();
            builder.Services.AddScoped<IAuthHelper, AuthHelper>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}