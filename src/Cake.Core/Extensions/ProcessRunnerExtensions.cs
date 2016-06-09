// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="IProcessRunner"/>.
    /// </summary>
    public static class ProcessRunnerExtensions
    {
        /// <summary>
        /// Starts a process using the specified information.
        /// </summary>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="filePath">The file name such as an application or document with which to start the process.</param>
        /// <returns>A process handle.</returns>
        public static IProcess Start(this IProcessRunner processRunner, FilePath filePath)
        {
            if (processRunner == null)
            {
                throw new ArgumentNullException("processRunner");
            }
            return processRunner.Start(filePath, new ProcessSettings());
        }
    }
}
