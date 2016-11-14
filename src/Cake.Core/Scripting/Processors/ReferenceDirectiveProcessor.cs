// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class ReferenceDirectiveProcessor : LineProcessor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        public ReferenceDirectiveProcessor(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        public override bool Process(IScriptAnalyzerContext context, string line, out string replacement)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            replacement = null;

            var tokens = Split(line);
            if (tokens.Length <= 0)
            {
                return false;
            }

            if (!tokens[0].Equals("#r", StringComparison.Ordinal) &&
                !tokens[0].Equals("#reference", StringComparison.Ordinal))
            {
                return false;
            }

            var referencePath = new FilePath(tokens[1].UnQuote());

            var directoryPath = context.Current.Path.MakeAbsolute(_environment).GetDirectory();
            var absoluteReferencePath = referencePath.MakeAbsolute(directoryPath);

            context.Current.References.Add(_fileSystem.Exist(absoluteReferencePath)
                ? absoluteReferencePath.FullPath : referencePath.FullPath);

            return true;
        }
    }
}