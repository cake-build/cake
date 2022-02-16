// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to arguments.
    /// </summary>
    [CakeAliasCategory("Arguments")]
    public static class ArgumentAliases
    {
        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <returns>Whether or not the specified argument exist.</returns>
        /// <example>
        /// This sample shows how to call the <see cref="HasArgument"/> method.
        /// <code>
        /// var argumentName = "myArgument";
        /// // Cake.exe .\hasargument.cake -myArgument="is specified"
        /// if (HasArgument(argumentName))
        /// {
        ///     Information("{0} is specified", argumentName);
        /// }
        /// // Cake.exe .\hasargument.cake
        /// else
        /// {
        ///     Warning("{0} not specified", argumentName);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static bool HasArgument(this ICakeContext context, string name)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return context.Arguments.HasArgument(name);
        }

        /// <summary>
        /// Gets an argument and throws if the argument is missing.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <returns>The value of the argument.</returns>
        /// <example>
        /// <code>
        /// // Cake.exe .\argument.cake --myArgument="is valid" --loopCount=5
        /// Information("Argument {0}", Argument&lt;string&gt;("myArgument"));
        /// var loopCount = Argument&lt;int&gt;("loopCount");
        /// for(var index = 0;index&lt;loopCount; index++)
        /// {
        ///     Information("Index {0}", index);
        /// }
        /// </code>
        /// </example>
        /// <exception cref="CakeException">Argument value is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
        [CakeMethodAlias]
        public static T Argument<T>(this ICakeContext context, string name)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var value = context.Arguments.GetArguments(name).FirstOrDefault();
            if (value == null)
            {
                const string format = "Argument '{0}' was not set.";
                var message = string.Format(CultureInfo.InvariantCulture, format, name);
                throw new CakeException(message);
            }

            return Convert<T>(value);
        }

        /// <summary>
        /// Gets all arguments with the specific name and throws if the argument is missing.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument values.</returns>
        /// <example>
        /// <code>
        /// // Cake.exe .\argument.cake --foo="foo" --foo="bar"
        /// var arguments = Arguments&lt;string&gt;("foo");
        /// Information("Arguments: {0}", string.Join(", ", arguments));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static ICollection<T> Arguments<T>(this ICakeContext context, string name)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var values = context.Arguments.GetArguments(name);
            if (values == null || values.Count == 0)
            {
                const string format = "Argument '{0}' was not set.";
                var message = string.Format(CultureInfo.InvariantCulture, format, name);
                throw new CakeException(message);
            }

            return values.Select(value => Convert<T>(value)).ToArray();
        }

        /// <summary>
        /// Gets all arguments with the specific name and returns the
        /// provided <paramref name="defaultValue"/> if the argument is missing.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="defaultValue">The value to return if the argument is missing.</param>
        /// <returns>The argument values.</returns>
        /// <example>
        /// <code>
        /// // Cake.exe .\argument.cake --foo="foo" --foo="bar"
        /// var arguments = Arguments&lt;string&gt;("foo", "default");
        /// Information("Arguments: {0}", string.Join(", ", arguments));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static ICollection<T> Arguments<T>(this ICakeContext context, string name, T defaultValue)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var values = context.Arguments.GetArguments(name);
            if (values == null || values.Count == 0)
            {
                return new T[] { defaultValue };
            }

            return values.Select(value => Convert<T>(value)).ToArray();
        }

        /// <summary>
        /// Gets all arguments with the specific name and returns the
        /// provided <paramref name="defaultValues"/> if the argument is missing.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="defaultValues">The values to return if the argument is missing.</param>
        /// <returns>The argument values.</returns>
        /// <example>
        /// <code>
        /// // Cake.exe .\argument.cake --foo="foo" --foo="bar"
        /// var arguments = Arguments&lt;string&gt;("foo", new [] { "default" });
        /// Information("Arguments: {0}", string.Join(", ", arguments));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static ICollection<T> Arguments<T>(this ICakeContext context, string name, ICollection<T> defaultValues)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var values = context.Arguments.GetArguments(name);
            if (values == null || values.Count == 0)
            {
                return defaultValues;
            }

            return values.Select(value => Convert<T>(value)).ToArray();
        }

        /// <summary>
        /// Gets all arguments with the specific name, evaluates and returns the
        /// provided <paramref name="defaultValues"/> if the argument is missing.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="defaultValues">The values to return if the argument is missing.</param>
        /// <returns>The argument values.</returns>
        /// <example>
        /// <code>
        /// // Cake.exe .\argument.cake --foo="foo" --foo="bar"
        /// var arguments = Arguments&lt;string&gt;("foo", ctx => new [] { "default" });
        /// Information("Arguments: {0}", string.Join(", ", arguments));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static ICollection<T> Arguments<T>(this ICakeContext context, string name, Func<ICakeContext, ICollection<T>> defaultValues)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var values = context.Arguments.GetArguments(name);
            if (values == null || values.Count == 0)
            {
                return defaultValues?.Invoke(context);
            }

            return values.Select(value => Convert<T>(value)).ToArray();
        }

        /// <summary>
        /// Gets an argument and returns the provided <paramref name="defaultValue"/> if the argument is missing.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="defaultValue">The value to return if the argument is missing.</param>
        /// <returns>The value of the argument if it exist; otherwise <paramref name="defaultValue"/>.</returns>
        /// <example>
        /// <code>
        /// // Cake.exe .\argument.cake --myArgument="is valid" --loopCount=5
        /// Information("Argument {0}", Argument&lt;string&gt;("myArgument", "is NOT valid"));
        /// var loopCount = Argument&lt;int&gt;("loopCount", 10);
        /// for(var index = 0;index&lt;loopCount; index++)
        /// {
        ///     Information("Index {0}", index);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static T Argument<T>(this ICakeContext context, string name, T defaultValue)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var value = context.Arguments.GetArguments(name)?.FirstOrDefault();
            return value == null
                ? defaultValue
                : Convert<T>(value);
        }

        /// <summary>
        /// Retrieves all command line arguments.
        /// </summary>
        /// <example>
        /// <code>
        /// var args = context.Arguments();
        ///
        /// if (args.ContainsKey("verbose"))
        /// {
        ///     Information("Verbose output enabled");
        /// }
        ///
        /// foreach(var arg in args)
        /// {
        ///     Information(
        ///         "Key: {0}\tValue: \"{1}\"",
        ///         arg.Key,
        ///         string.Join(";", arg.Value)
        ///         );
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>The command line arguments.</returns>
        [CakeMethodAlias]
        public static IDictionary<string, ICollection<string>> Arguments(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return context.Arguments.GetArguments();
        }

        private static T Convert<T>(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromInvariantString(value);
        }
    }
}