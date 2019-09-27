using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Read.Repositories.AutoMapper;

namespace PaymentGateway.DependencyInjection
{
    public static class AutoMapperProfile
    {
        public static void Register(IServiceCollection container, Assembly callerAssembly = null)
        {
            var assemblies = GetAssemblies(callerAssembly).ToArray();
            container.AddAutoMapper(assemblies);
        }

        private static IEnumerable<Assembly> GetAssemblies(Assembly assembly)
        {
            yield return typeof(PaymentMappingProfile).GetTypeInfo().Assembly;
            if (assembly != null)
            {
                yield return assembly;
            }
        }
    }
}
