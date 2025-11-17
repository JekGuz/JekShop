using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace JekShop.SpaceShipTest.Mock
{
    public class MockHostEnvironment : IHostEnvironment
    {

        public string EnvironmentName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IFileProvider ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        ////В каком окружении «работает» приложение
        ////public string EnvironmentName { get; set; }

        ////Имя приложения(можно любое)
        ////public string ApplicationName { get; set; }

        ////Корневая папка приложения
        ////public string ContentRootPath { get; set; }

        ////Провайдер файлов для этой папки
        ////public IFileProvider ContentRootFileProvider { get; set; }

        ////public MockHostEnvironment()
        ////{
        ////    Берём текущую папку тестового проекта
        ////   ContentRootPath = Directory.GetCurrentDirectory();
        ////    ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
        ////}
    }
}
