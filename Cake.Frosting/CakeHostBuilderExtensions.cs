// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Frosting.Internal;
using Cake.Frosting.Internal.Arguments;

namespace Cake.Frosting
{
    /// <summary>
    /// Contains extension methods for <see cref="ICakeHostBuilder"/>.
    /// </summary>
    public static class CakeHostBuilderExtensions
    {
        /// <summary>
        /// Specify the startup type to be used by the Cake host.
        /// </summary>
        /// <typeparam name="TStartup">The startup type.</typeparam>
        /// <param name="builder">The <see cref="ICakeHostBuilder"/> to configure.</param>
        /// <returns>The same <see cref="ICakeHostBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ICakeHostBuilder UseStartup<TStartup>(this ICakeHostBuilder builder)
            where TStartup : IFrostingStartup, new()
        {
            Guard.ArgumentNotNull(builder, nameof(builder));

            return builder.ConfigureServices(services =>
            {
                var startup = new TStartup();
                startup.Configure(services);
            });
        }

        /// <summary>
        /// Specify the arguments to be used when building the host.
        /// The arguments will be translated into a <see cref="CakeHostOptions"/> instance.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="args">The arguments to parse.</param>
        /// <returns>The same <see cref="ICakeHostBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ICakeHostBuilder WithArguments(this ICakeHostBuilder builder, string[] args)
        {
            Guard.ArgumentNotNull(builder, nameof(builder));

            return builder.ConfigureServices(services =>
                services.RegisterInstance(ArgumentParser.Parse(args)).AsSelf().Singleton());
        }
    }
}