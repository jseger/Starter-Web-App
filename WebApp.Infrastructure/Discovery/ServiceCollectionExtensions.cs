using WebApp.Infrastructure.Shared.Discovery;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebApp.Infrastructure.Discovery
{
    public static class ServiceCollectionExtensions
    {
        private static List<Assembly> _loadedAssemblies = new List<Assembly>();
        private static IEnumerable<string> ignore = new string[] {
            "Microsoft.",
            "System.",
            "NuGet.",
            "Newtonsoft."
        };

        private static void LoadAssemblies()
        {
            if (_loadedAssemblies.Count == 0) {
                foreach (string assemblyPath in Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.AllDirectories)) {
                    if (ignore.All(i => !assemblyPath.Contains(i))) {
                        var assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                        _loadedAssemblies.Add(assembly);
                    }
                }
            }
        }

        public static IServiceCollection DiscoverScopedServices(this IServiceCollection services)
        {
            return services.DiscoverServices<IScopedService>();
        }

        public static IServiceCollection DiscoverTransientServices(this IServiceCollection services)
        {
            return services.DiscoverServices<ITransientService>();
        }

        public static IServiceCollection DiscoverSingletonServices(this IServiceCollection services)
        {
            return services.DiscoverServices<ISingletonService>();
        }

        private static IServiceCollection DiscoverServices<TServiceType>(this IServiceCollection services)
        {
            LoadAssemblies();

            ServiceLifetime lifetime = ServiceLifetime.Scoped;

            switch (typeof(TServiceType).Name) {
                case nameof(ITransientService):
                    lifetime = ServiceLifetime.Transient;
                    break;
                case nameof(IScopedService):
                    lifetime = ServiceLifetime.Scoped;
                    break;
                case nameof(ISingletonService):
                    lifetime = ServiceLifetime.Singleton;
                    break;
                default:
                    throw new ArgumentException($"The type {typeof(TServiceType).Name} is not a valid type in this context.");
            }

            List<Type> implementations = _loadedAssemblies
                .SelectMany(assembly => assembly.GetTypes()).Where(type => typeof(TServiceType).IsAssignableFrom(type) && type.IsClass).ToList();

            foreach (Type implementation in implementations) {
                Type[] servicesToBeRegistered = implementation.GetInterfaces()
                    .Where(i => i != typeof(IScopedService) && i != typeof(ITransientService) && i != typeof(ISingletonService)).ToArray();

                if (servicesToBeRegistered.Any()) {
                    foreach (Type serviceType in servicesToBeRegistered) {
                        bool isGenericTypeDefinition = implementation.IsGenericType && implementation.IsGenericTypeDefinition;
                        Type service = isGenericTypeDefinition
                            && serviceType.IsGenericType
                            && serviceType.IsGenericTypeDefinition == false
                            && serviceType.ContainsGenericParameters
                                ? serviceType.GetGenericTypeDefinition()
                                : serviceType;

                        bool isAlreadyRegistered = services.Any(s => s.ServiceType == service && s.ImplementationType == implementation);

                        if (!isAlreadyRegistered) {
                            Console.WriteLine(implementation.Name);
                            services.Add(new ServiceDescriptor(service, implementation, lifetime));
                        }
                    }
                }
                else {
                    if (implementation.IsClass) {
                        bool isAlreadyRegistered = services.Any(s => s.ServiceType == implementation && s.ImplementationType == implementation);

                        if (!isAlreadyRegistered) {
                            services.Add(new ServiceDescriptor(implementation, implementation, lifetime));
                        }
                    }
                }
            }

            return services;
        }
    }
}
