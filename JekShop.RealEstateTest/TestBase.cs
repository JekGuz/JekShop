using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using JekShop.ApplicationServices.Services;
using JekShop.Core.ServiceInterface;
using JekShop.Data; // где у тебя находится JekShopContext
using JekShop.RealEstateTest.Macros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

using JekShop.RealEstateTest.Fakes;
using Microsoft.Extensions.Hosting;
using JekShop.RealEstateTest.Mock;


namespace JekShop.RealEstateTest
{
    // abstract - 
    public abstract class TestBase
    {
        //
        protected IServiceProvider serviceProvider { get; set; }

        protected TestBase() 
        {
            var services = new ServiceCollection();
            SetupServices(services);
            serviceProvider = services.BuildServiceProvider();
        }


        //
        public virtual void SetupServices(IServiceCollection services)
        {
            //
            services.AddScoped<IRealEstateServices, RealEstateServis>();
            


            //services.AddScoped<IFileServices, FakeFileServices>();
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IHostEnvironment, MockHostEnvironment>();

            //
            services.AddDbContext<JekShopContext>(x =>
            {
                //
                x.UseInMemoryDatabase("TestDb");
                //
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            RegisterMacros(services);
        }


        // макрос - 
        private void RegisterMacros(IServiceCollection services) 
        {
            var macroBaseType = typeof(IMacros);

            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);
        }


        protected T Svc<T>() where T : notnull
        {
            return serviceProvider.GetRequiredService<T>();
        }

        // 
        //protected T Svc<T>()
        //{
        //    return serviceProvider.GetService<T>();
        //}

        //
        public void Dispose()
        {
            
        }

    }
}
