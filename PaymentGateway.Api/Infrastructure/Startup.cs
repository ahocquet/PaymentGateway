using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using PaymentGateway.Api.Features.Payment;
using PaymentGateway.SharedKernel.Models;

namespace PaymentGateway.Api.Infrastructure
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly AppSettings         _appSettings = new AppSettings();

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env          = env;
            Configuration = configuration;
            Configuration.Bind(_appSettings);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(_appSettings.ApplicationInsights.InstrumentationKey);
            services.WireAuthentication(_appSettings, _env);

            services.AddMvc(opts =>
                     {
#if DEBUG
                         opts.Filters.Add<AllowAnonymousFilter>();
#endif
                     })
                    .AddJsonOptions(options =>
                     {
                         options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                         options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                         options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                     })
                    .AddFluentValidation(fv =>
                     {
                         fv.RegisterValidatorsFromAssemblyContaining<Submit.Validator>();
                         fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                     })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.WireSwagger();
            services.SetupIoC(_appSettings, Configuration);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
        }
    }
}
