using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Spectre.Console.Cli;
using static Cake.Cli.Infrastructure.Constants;

namespace Cake.Cli.Infrastructure
{
    /// <summary>
    /// Spectre.Console <see cref="IRemainingArguments"/> extensions.
    /// </summary>
    public static class IRemainingArgumentsExtensions
    {
        /// <summary>
        /// Parses Spectre.Console <see cref="IRemainingArguments"/> to <see cref="CakeArguments"/>.
        /// </summary>
        /// <param name="remainingArguments">The remainingArguments.</param>
        /// <param name="targets">The optional targets, i.e. if specified by command.</param>
        /// <param name="preProcessArgs">The optional pre-process arguments.</param>
        /// <returns><see cref="CakeArguments"/>.</returns>
        public static CakeArguments ToCakeArguments(
            this IRemainingArguments remainingArguments,
            string[] targets = null,
            Action<IDictionary<string, List<string>>> preProcessArgs = null)
            => remainingArguments.ToCakeArguments(
                noReport: false,
                targets: targets,
                preProcessArgs: preProcessArgs);

        /// <summary>
        /// Parses Spectre.Console <see cref="IRemainingArguments"/> to <see cref="CakeArguments"/>.
        /// </summary>
        /// <param name="remainingArguments">The remainingArguments.</param>
        /// <param name="noReport">No report argument supplied.</param>
        /// <param name="targets">The optional targets, i.e. if specified by command.</param>
        /// <param name="preProcessArgs">The optional pre-process arguments.</param>
        /// <returns><see cref="CakeArguments"/>.</returns>
        public static CakeArguments ToCakeArguments(
            this IRemainingArguments remainingArguments,
            bool noReport,
            string[] targets = null,
            Action<IDictionary<string, List<string>>> preProcessArgs = null)
        {
            var arguments = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            // Keep the actual remaining arguments in the cake arguments
            foreach (var group in remainingArguments.Parsed)
            {
                arguments.TryAdd(
                    group.Key.TrimStart('-'),
                    [.. group]);
            }

            // Fixes #3291, We have to add arguments manually which are defined within the DefaultCommandSettings type. Those are not considered "as remaining" because they could be parsed
            const string targetArgumentName = "target";
            if (!arguments.TryGetValue(targetArgumentName, out List<string> value))
            {
                arguments[targetArgumentName] = value =
                    [];
            }

            if (targets != null)
            {
                foreach (var target in targets)
                {
                    value.Add(target);
                }
            }

            if (noReport)
            {
                arguments.TryAdd(
                    Settings.NoReport,
                    [bool.TrueString]);
            }

            preProcessArgs?.Invoke(arguments);

            var argumentLookUp = arguments.SelectMany(a => a.Value, Tuple.Create).ToLookup(a => a.Item1.Key, a => a.Item2);
            return new CakeArguments(argumentLookUp);
        }
    }
}
