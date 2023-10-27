using Abp.AspNetCore.Dependency;
using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using QuestPDF.Infrastructure;

namespace Bwr.Exchange.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;
            CreateHostBuilder(args).Build().Run();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseCastleWindsor(IocManager.Instance.IocContainer);
    }
}
