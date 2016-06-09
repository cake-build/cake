// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.XUnit
{
    internal static class XUnitRunnerUtilities
    {
        internal static FilePath GetReportFileName(IReadOnlyList<FilePath> assemblyPaths)
        {
            return assemblyPaths.Count == 1
                ? assemblyPaths[0].GetFilename()
                : new FilePath("TestResults");
        }
    }
}
