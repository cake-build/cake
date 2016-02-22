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
