using WebApp.Infrastructure.Events;
using WebApp.Infrastructure.Shared.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebApp.Infrastructure.Discovery
{
    public static class EventAggregatorExtensions
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

        public static IServiceCollection DiscoverEventHandlers(this IServiceCollection services)
        {
            LoadAssemblies();

            services.AddScoped<IEventAggregator, EventAggregator>((s) => {
                var ea = new EventAggregator();
                foreach (var assembly in _loadedAssemblies) {
                    var types = assembly.GetTypes().Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(IEventHandler)));
                    foreach (var t in types) {
                        ea.SubscribeOnPublishedThread(Activator.CreateInstance(t));
                    }
                }

                return ea;
            });

            return services;
        }
    }
}
