// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace Cake.Core
{
    public static partial class CakeTaskBuilderOfTExtensions
    {
        /// <summary>
        /// Adds an indication to the task that a thrown exception will not halt the script execution.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> ContinueOnError<TData>(
            this CakeTaskBuilder<TData> builder)
            where TData : class
            => builder.Process(builder => builder.ContinueOnError());

        /// <summary>
        /// Defers all exceptions until after all actions for this task have completed.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The task builder.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> DeferOnError<TData>(
            this CakeTaskBuilder<TData> builder)
            where TData : class
            => builder.Process(builder => builder.DeferOnError());

        /// <summary>
        /// Adds an error handler to be executed if an exception occurs in the task.
        /// </summary>
        /// <typeparam name="TData">The extra data to operate with inside the error handler.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="errorHandler">The error handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> OnError<TData>(
            this CakeTaskBuilder<TData> builder,
            Action<Exception, ICakeContext, TData> errorHandler)
            where TData : class
            => builder.Process(builder => builder.OnError(errorHandler));

        /// <summary>
        /// Adds an error reporter for the task to be executed when an exception is thrown from the task.
        /// This action is invoked before the error handler, but gives no opportunity to recover from the error.
        /// </summary>
        /// <typeparam name="TData">The extra data to operate with inside the error handler.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="errorReporter">The error report handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> ReportError<TData>(
            this CakeTaskBuilder<TData> builder,
            Action<Exception> errorReporter)
            where TData : class
            => builder.Process(builder => builder.ReportError(errorReporter));

        /// <summary>
        /// Adds an error handler to be executed if an exception occurs in the task.
        /// </summary>
        /// <typeparam name="TData">The extra data to operate with inside the error handler.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="errorHandler">The error handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> OnError<TData>(
            this CakeTaskBuilder<TData> builder,
            Func<Exception, ICakeContext, TData, Task> errorHandler)
            where TData : class
            => builder.Process(builder => builder.OnError(errorHandler));

        /// <summary>
        /// Adds a finally handler to be executed after the task have finished executing.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="finallyHandler">The finally handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> Finally<TData>(
            this CakeTaskBuilder<TData> builder,
            Func<ICakeContext, TData, Task> finallyHandler)
            where TData : class
            => builder.Process(builder => builder.Finally(finallyHandler));

        /// <summary>
        /// Adds a finally handler to be executed after the task have finished executing.
        /// </summary>
        /// <typeparam name="TData">The type of the data context.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="finallyHandler">The finally handler.</param>
        /// <returns>The same <see cref="CakeTaskBuilder{TData}"/> instance so that multiple calls can be chained.</returns>
        public static CakeTaskBuilder<TData> Finally<TData>(
            this CakeTaskBuilder<TData> builder,
            Action<ICakeContext, TData> finallyHandler)
            where TData : class
            => builder.Process(builder => builder.Finally(finallyHandler));
    }
}