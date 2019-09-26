using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PaymentGateway.Api.Infrastructure
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly        ILogger                _logger;
        private static readonly JsonSerializerSettings JsonSerializerSettings;


#pragma warning disable S3963 // "static" fields should be initialized inline
        static ErrorHandlingMiddleware()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        public ErrorHandlingMiddleware(ILogger logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new
            {
                // customize as you need
                error = new
                {
                    message   = exception.Message,
                    exception = exception.GetType().Name
                }
            };
            var json = JsonConvert.SerializeObject(response, Formatting.None, JsonSerializerSettings);

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode  = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(json);
        }
    }
}