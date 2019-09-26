using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace PaymentGateway.TestKernel.Extensions
{
    public static class ContainerExtensions
    {
        public static void RegisterSubstitute<T>(this IServiceCollection container) where T : class
        {
            container.AddTransient(provider => Substitute.For<T>());
        }
    }
}
