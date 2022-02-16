using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Cli;

namespace Cake.Tests.Fakes
{
    public sealed class FakeRemainingArguments : IRemainingArguments
    {
        public ILookup<string, string> Parsed { get; }

        public IReadOnlyList<string> Raw => throw new NotImplementedException();

        public FakeRemainingArguments(IDictionary<string, string> arguments)
        {
            Parsed = (arguments ?? new Dictionary<string, string>())
                .ToLookup(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
        }
    }
}
