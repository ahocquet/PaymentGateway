using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace PaymentGateway.Api.Infrastructure
{
    public static class Swagger
    {
        public static void UseSwagger(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseOpenApi();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUi3();
        }

        public static void WireSwagger(this IServiceCollection services)
        {
            services.AddSwaggerDocument(cfg =>
            {
                cfg.DocumentName = "v1";
                cfg.PostProcess  = document => { document.Info.Title = "PaymentGateway"; };

                cfg.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
                cfg.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token", new List<string>(), new OpenApiSecurityScheme
                {
                    Type        = OpenApiSecuritySchemeType.ApiKey,
                    Name        = "Authorization",
                    In          = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Copy 'Bearer ' + valid JWT token into field"
                }));
            });
        }
    }
}
