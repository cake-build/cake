// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to resolving services from Cake's IoC container.
    /// </summary>
    /// <para>
    /// The <c>ServiceProvider</c> property alias exposes the script execution container
    /// so services can be resolved from a Cake.Tool build script. Built-in Cake services
    /// such as <see cref="Cake.Core.Diagnostics.ICakeLog"/> are always available.
    /// Custom services are registered by an <c>ICakeModule</c> loaded via the
    /// <c>#module</c> preprocessor directive.
    /// </para>
    [CakeAliasCategory("IoC")]
    public static class ServiceProviderAliases
    {
        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The service provider.</returns>
        /// <example>
        /// <code>
        /// #module nuget:?package=Cake.MyService.Module&amp;version=1.0.0
        ///
        /// Task("MyTask")
        ///     .Does(() =>
        /// {
        ///     var log = ServiceProvider.GetRequiredService&lt;ICakeLog&gt;();
        ///     log.Information("Hello from IoC");
        ///
        ///     var myService = ServiceProvider.GetRequiredService&lt;IMyService&gt;();
        ///     myService.DoSomething();
        /// });
        /// </code>
        /// </example>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Microsoft.Extensions.DependencyInjection")]
        public static IServiceProvider ServiceProvider(this ICakeContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return context.ServiceProvider;
        }
    }
}
