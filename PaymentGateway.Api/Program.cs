using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PaymentGateway.Api.Infrastructure;

namespace PaymentGateway.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>();
    }
}
