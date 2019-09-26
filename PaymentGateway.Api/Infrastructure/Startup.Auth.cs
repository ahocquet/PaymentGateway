using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.SharedKernel.Models;

namespace PaymentGateway.Api.Infrastructure
{
    public static class StartupAuth
    {
        public static void WireAuthentication(this IServiceCollection services, AppSettings appSettings, IHostingEnvironment env)
        {
            //Azure AD (for applications)
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                     {
                         options.Authority           = "RealAuthority";
                         options.Audience            = "AudienceApp";
                         options.IncludeErrorDetails = true;
                     });
        }
    }
}
