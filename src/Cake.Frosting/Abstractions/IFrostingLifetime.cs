// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting
{
    /// <summary>
    /// Represents the Setup/Teardown logic for a Cake run.
    /// </summary>
    public interface IFrostingLifetime
    {
        /// <summary>
        /// This method is executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="context">The context.</param>
        void Setup(ICakeContext context);

        /// <summary>
        /// This method is executed after all tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The teardown information.</param>
        void Teardown(ICakeContext context, ITeardownContext info);
    }
}
