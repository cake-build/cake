using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools
{
    internal static class VisualStudio
    {
        internal static class Versions
        {
            internal static ICollection<string> TenToFourteen { get; } = new[] { "14.0", "12.0", "11.0", "10.0" };

            internal static ICollection<string> TwentySeventeenAndLater { get; } = new[]
            {
                "2022",
                "2019",
                "2017"
            };
        }

        internal static class Editions
        {
            internal static ICollection<string> Preview { get; } = new[]
            {
                "Preview"
            };

            internal static ICollection<string> Stable { get; } = new[]
            {
                "Enterprise",
                "Professional",
                "Community",
                "BuildTools"
            };

            internal static ICollection<string> All { get; } = Preview
                .Concat(Stable)
                .ToArray();
        }

        internal static FilePath GetYearAndEditionToolPath(ICakeEnvironment environment, string year, string edition, FilePath relativeFile)
        {
            var root = GetYearAndEditionRootPath(environment, year, edition);
            return root.CombineWithFilePath(relativeFile);
        }

        internal static DirectoryPath GetYearAndEditionRootPath(ICakeEnvironment environment, string year, string edition)
        {
            var programFiles = (year, edition) switch
            {
                ("2022", "BuildTools") => environment.GetSpecialPath(SpecialPath.ProgramFilesX86),
                ("2022", _) => environment.GetSpecialPath(SpecialPath.ProgramFiles),
                (_, _) => environment.GetSpecialPath(SpecialPath.ProgramFilesX86),
            };

            return programFiles.Combine($"Microsoft Visual Studio/{year}/{edition}");
        }

        internal static FilePath GetVersionNumberToolPath(ICakeEnvironment environment, string version, FilePath relativeFile)
        {
            var root = GetVersionNumberRootPath(environment, version);
            return root.CombineWithFilePath(relativeFile);
        }

        internal static DirectoryPath GetVersionNumberRootPath(ICakeEnvironment environment, string version)
        {
            var programFiles = environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
            return programFiles.Combine($"Microsoft Visual Studio {version}");
        }
    }
}