// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting.Cli
{
    internal static class CakeHostBuilderExtensions
    {
        public static ICakeHostBuilder UseStartup(this ICakeHostBuilder builder, Type type)
        {
            return builder.ConfigureServices(services =>
            {
                if (!typeof(IFrostingStartup).IsAssignableFrom(type))
                {
                    throw new FrostingException("The specified startup class is not inherited from IFrostingStartup.");
                }
                if (type.GetConstructor(Type.EmptyTypes) == null)
                {
                    throw new FrostingException("The specified startup class does not have a default constructor.");
                }
                var startup = (IFrostingStartup)Activator.CreateInstance(type);
                startup.Configure(services);
            });
        }
    }
}
