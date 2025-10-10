using Microsoft.Extensions.DependencyInjection;

namespace Cake.Frosting.TaskChains
{
    /// <summary>
    /// Extension methods for task chains.
    /// </summary>
    public static class CakeHostExtensions
    {
        /// <summary>
        /// Registers the task configurator that will configure the build task upon startup.
        /// </summary>
        /// <typeparam name="T">Task configurator type.</typeparam>
        /// <param name="host">Cake host.</param>
        /// <returns>Cake host.</returns>
        public static CakeHost UseTaskConfigurator<T>(this CakeHost host) where T : class, ITaskConfigurator
        {
            host.ConfigureServices(services => { services.AddSingleton<ITaskConfigurator, T>(); });
            return host;
        }

        /// <summary>
        /// Registers the chained task configurator that will configure the build task chains upon startup.
        /// </summary>
        /// <typeparam name="T">Task configurator type.</typeparam>
        /// <param name="host">Cake host.</param>
        /// <returns>Cake host.</returns>
        public static CakeHost UseChainedTaskConfigurator<T>(this CakeHost host) where T : class, ITaskChainProvider
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<ITaskChainProvider, T>();
                services.AddSingleton<ITaskConfigurator, ChainedTaskConfigurator>();
            });
            return host;
        }
    }
}