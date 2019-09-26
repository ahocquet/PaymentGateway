using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.DependencyInjection
{
    public static class MediatRProfile
    {
        public static void Register(IServiceCollection container, Assembly callerAssembly = null)
        {
            var assemblies = GetAssemblies(callerAssembly).ToArray();
            container.AddMediatR(assemblies);
        }

        private static IEnumerable<Assembly> GetAssemblies(Assembly assembly)
        {
            yield return typeof(IMediator).GetTypeInfo().Assembly;
            if (assembly != null)
            {
                yield return assembly;
            }
        }
    }
}
