using AM._Infrastructure.Configuration;
using BM._Infrastructure.Configuration;
using FrameWork.Application.Authentication;
using FrameWork.Application.Authentication.PasswordHashing;
using IM._Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using SM._Infrastructure.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using CM._Infrastructure.Configuration;
using DM._Infrastructure.Configuration;
using FrameWork.Application.FileUpload;
using FrameWork.Infrastructure;

namespace WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();

            var connectionString = builder.Configuration.GetConnectionString("BazarchePlusDb");

            ShopManagementBootstrapper.Configure(builder.Services,connectionString);
            InventoryManagementBootstrapper.Configure(builder.Services, connectionString);
            DiscountManagementBootstrapper.Configure(builder.Services,connectionString);
            AccountManagementBootstrapper.Configure(builder.Services,connectionString);
            BlogManagementBootstrapper.Configure(builder.Services,connectionString);
            CommentManagementBootstrapper.Configure(builder.Services,connectionString);


            builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IFileUploader, FileUploader>();
            builder.Services.AddScoped<IAuthHelper, AuthHelper>();

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = new PathString("/Account");
                    o.LogoutPath = new PathString("/Account");
                    o.AccessDeniedPath = new PathString("/AccessDenied");
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminArea",
                    builder => builder.RequireRole(new List<string> { Roles.Administrator, Roles.Seller }));

                options.AddPolicy("Discount",
                    builder => builder.RequireRole(new List<string> { Roles.Seller }));

                options.AddPolicy("Account",
                    builder => builder.RequireRole(new List<string> { Roles.Administrator }));

                options.AddPolicy("Comment",
                    builder => builder.RequireRole(new List<string> { Roles.Administrator }));

            });

            builder.Services.AddRazorPages()
                .AddMvcOptions(options => options.Filters.Add<SecurityPageFilter>())
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("Administration", "/", "AdminArea");
                    options.Conventions.AuthorizeAreaFolder("Administration", "/Discounts", "Discount");
                    options.Conventions.AuthorizeAreaFolder("Administration", "/Accounts", "Account");
                    options.Conventions.AuthorizeAreaFolder("Administration", "/Comments", "Comment");
                });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}