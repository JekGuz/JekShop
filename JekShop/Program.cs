using JekShop.ApplicationServices.Services;
using JekShop.Controllers;
using JekShop.Core.Domain;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using JekShop.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;


namespace JekShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ISpaceshipsServices, SpaceshipsServices>();

            builder.Services.AddScoped<IFileServices, FileServices>();

            builder.Services.AddScoped<IRealEstateServices, RealEstateServis>();

            builder.Services.AddHttpClient<WeatherForecastServices>();
            builder.Services.AddScoped<IWeatherForecastServices, WeatherForecastServices>();

            builder.Services.AddScoped<IChuckJokeService, ChuckJokeService>();

            builder.Services.AddHttpClient<ICocktailServices, CocktailService>();

            builder.Services.AddTransient<IEmailServices, EmailServices>();

            builder.Services.AddScoped<ChatController>();


            builder.Services.AddDbContext<JekShopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnections")));


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<JekShopContext>()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("CustomEmailConfirmation")
                //AddDefaultUI()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Accounts/Login";
                options.AccessDeniedPath = "/Accounts/Login";
            });

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            // app.MapRazorPages();
            app.MapHub<UserHub>("/hubs/chat");

            app.Run();
        }
    }
}
