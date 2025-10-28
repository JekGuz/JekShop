using JekShop.Core.ServiceInterface;
using JekShop.Data;
using Microsoft.EntityFrameworkCore;
using JekShop.ApplicationServices.Services;


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

            builder.Services.AddScoped<IOpenWeatherService, OpenWeatherService>();


            builder.Services.AddDbContext<JekShopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnections")));


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

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
