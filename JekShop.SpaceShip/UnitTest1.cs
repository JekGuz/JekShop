using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using JekShop.Data; // JekShopContext
using JekShop.Core.ServiceInterface; // ISpaceshipsServices, IFileServices
using JekShop.ApplicationServices.Services;
using JekShop.SpaceShipTest.Mock;  // SpaceshipsServices, FileServices

namespace JekShop.SpaceShipTest
{
    public abstract class UnitTest1 : IDisposable
    {
        protected IServiceProvider ServiceProvider { get; }

        protected UnitTest1()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        public virtual void SetupServices(IServiceCollection services)
        {
            // DI: сервис космических кораблей
            // !!! ЕСЛИ у тебя класс называется иначе (например, SpaceshipServices),
            // поменяй второй тип в этой строке.
            services.AddScoped<ISpaceshipsServices, SpaceshipsServices>();

            // Файловый сервис (если он реально нужен твоей реализацией)
            services.AddScoped<IFileServices, FileServices>();

            // Мок окружения (рабочая реализация ниже отдельным классом)
            services.AddSingleton<IHostEnvironment, MockHostEnvironment>();

            // InMemory DbContext
            services.AddDbContext<JekShopContext>(opts =>
            {
                opts.UseInMemoryDatabase("TestDb_Space");
                opts.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        }

        protected T Svc<T>() where T : notnull =>
            ServiceProvider.GetRequiredService<T>();

        public void Dispose()
        {
            if (ServiceProvider is IDisposable d) d.Dispose();
        }
    }
}