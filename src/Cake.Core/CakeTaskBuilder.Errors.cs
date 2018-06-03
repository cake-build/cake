// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    public static partial class CakeTaskBuilderExtensions
    {
        /// <summary>
        /// Defers all exceptions until after all actions for this task have completed
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder DeferOnError(this CakeTaskBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Target.SetDeferExceptions(true);
            return builder;
        }

        /// <summary>
        /// Adds an indication to the task that a thrown exception will not halt the script execution.
        /// </summary>
        /// <param name="builder">The task builder.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder ContinueOnError(this CakeTaskBuilder builder)
        {
            return OnError(builder, () => { });
        }

        /// <summary>
        /// Adds an error handler to be executed if an exception occurs in the task.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="errorHandler">The error handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder OnError(this CakeTaskBuilder builder, Action errorHandler)
        {
            return OnError(builder, exception => errorHandler());
        }

        /// <summary>
        /// Adds an error handler to be executed if an exception occurs in the task.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="errorHandler">The error handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder OnError(this CakeTaskBuilder builder, Action<Exception> errorHandler)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Target.SetErrorHandler(errorHandler);
            return builder;
        }

        /// <summary>
        /// Adds a finally handler to be executed after the task have finished executing.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="finallyHandler">The finally handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder Finally(this CakeTaskBuilder builder, Action finallyHandler)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Target.SetFinallyHandler(finallyHandler);
            return builder;
        }

        /// <summary>
        /// Adds an error reporter for the task to be executed when an exception is thrown from the task.
        /// This action is invoked before the error handler, but gives no opportunity to recover from the error.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="errorReporter">The finally handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder ReportError(this CakeTaskBuilder builder, Action<Exception> errorReporter)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Target.SetErrorReporter(errorReporter);
            return builder;
        }
    }
}
