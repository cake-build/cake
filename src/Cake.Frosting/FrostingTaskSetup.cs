// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Frosting;

namespace Cake.Frosting
{
    /// <summary>
    /// Base class for the setup logic of a task.
    /// </summary>
    public abstract class FrostingTaskSetup : FrostingTaskSetup<ICakeContext>
    {
    }

    /// <summary>
    /// Base class for the setup logic of a task.
    /// </summary>
    /// <typeparam name="TContext">The build context type.</typeparam>
    public abstract class FrostingTaskSetup<TContext> : IFrostingTaskSetup
        where TContext : ICakeContext
    {
        /// <summary>
        /// This method is executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The setup information.</param>
        public abstract void Setup(TContext context, ITaskSetupContext info);

        void IFrostingTaskSetup.Setup(ICakeContext context, ITaskSetupContext info)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Setup((TContext)context, info);
        }
    }
}
