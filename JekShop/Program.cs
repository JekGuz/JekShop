using JekShop.ApplicationServices.Services;
using JekShop.Controllers;
using JekShop.Core.Domain;
using JekShop.Core.ServiceInterface;
using JekShop.Data;
using JekShop.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;


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

                // Custom token provider for email confirmation
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                // Lockout settings
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

                options.Lockout.AllowedForNewUsers = true;


            })
            .AddEntityFrameworkStores<JekShopContext>()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("CustomEmailConfirmation")
            .AddDefaultTokenProviders();

            // Глобально: все страницы требуют логин (кроме тех, где [AllowAnonymous])
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

                // чтобы кука была "сессионной" (исчезала при закрытии браузера)
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = false;
            });

            builder.Services.ConfigureExternalCookie(options =>
            {
                // ВАЖНО для OAuth (Facebook часто ломается без этого)
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            builder.Services.AddCookiePolicy(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // ВНЕШНЯЯ АВТОРИЗАЦИЯ — ОБЯЗАТЕЛЬНО ДО builder.Build()
            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
                    options.SignInScheme = IdentityConstants.ExternalScheme;

                    options.Events.OnRedirectToAuthorizationEndpoint = context =>
                    {
                        // Разбираем URL и гарантированно ставим ТОЛЬКО ОДИН prompt
                        var url = context.RedirectUri;

                        // убираем старый prompt если он уже есть, и ставим новый
                        url = QueryHelpers.AddQueryString(url, "prompt", "select_account");

                        context.Response.Redirect(url);
                        return Task.CompletedTask;
                    };
                })
                .AddFacebook(options =>
                {
                    options.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
                    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
                    options.SignInScheme = IdentityConstants.ExternalScheme;

                    options.Scope.Clear();
                    options.Scope.Add("public_profile");
                    options.Scope.Add("email");

                    options.Fields.Clear();
                    options.Fields.Add("id");
                    options.Fields.Add("name");
                    options.Fields.Add("email");
                });

            builder.Services.AddSignalR();

            // Build ПОСЛЕ всех builder.Services.Add...
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapHub<UserHub>("/hubs/chat");

            app.Run();
        }
    }
}
