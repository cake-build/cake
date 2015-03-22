using System;
using System.Collections.Generic;
using Cake.Core.IO;
using NuGet;

namespace Cake.Scripting.Roslyn.Installation
{
    internal static class RoslynHelpers
    {
        public static RoslynInstallerInstructions GetInstallationInstructions(bool experimental)
        {
            return experimental 
                ? CreateNightlyInstructions() 
                : CreateStableInstructions();
        }

        private static RoslynInstallerInstructions CreateStableInstructions()
        {
            var paths = new FilePath[] 
            {
                @"Roslyn.Compilers.CSharp.1.2.20906.2\lib\net45\Roslyn.Compilers.CSharp.dll",
                @"Roslyn.Compilers.Common.1.2.20906.2\lib\net45\Roslyn.Compilers.dll"
            };

            var packages = new Dictionary<string, SemanticVersion>
            {
                { "Roslyn.Compilers.CSharp", new SemanticVersion(new Version(1, 2, 20906, 2)) }
            };

            var sources = new[] 
            {
                new PackageSource("https://packages.nuget.org/api/v2")
            };

            return new RoslynInstallerInstructions(paths, packages, sources);
        }

        private static RoslynInstallerInstructions CreateNightlyInstructions()
        {
            var paths = new FilePath[] 
            {
                @"Microsoft.CodeAnalysis.Scripting.CSharp.1.0.0-rc2-20150322-01\lib\net45\Microsoft.CodeAnalysis.Scripting.CSharp.dll",
                @"Microsoft.CodeAnalysis.Scripting.Common.1.0.0-rc2-20150322-01\lib\net45\Microsoft.CodeAnalysis.Scripting.dll",
                @"Microsoft.CodeAnalysis.Common.1.0.0-rc2-20150322-01\lib\net45\Microsoft.CodeAnalysis.Desktop.dll",
                @"Microsoft.CodeAnalysis.Common.1.0.0-rc2-20150322-01\lib\net45\Microsoft.CodeAnalysis.dll",
                @"System.Collections.Immutable.1.1.33-beta\lib\portable-net45+win8+wp8+wpa81\System.Collections.Immutable.dll",
                @"System.Reflection.Metadata.1.0.18-beta\lib\portable-net45+win8\System.Reflection.Metadata.dll",
                @"Microsoft.CodeAnalysis.CSharp.1.0.0-rc2-20150322-01\lib\net45\Microsoft.CodeAnalysis.CSharp.dll",
                @"Microsoft.CodeAnalysis.CSharp.1.0.0-rc2-20150322-01\lib\net45\Microsoft.CodeAnalysis.CSharp.Desktop.dll",
            };

            var packages = new Dictionary<string, SemanticVersion>
            {
                { "Microsoft.CodeAnalysis.Scripting.CSharp", new SemanticVersion(1, 0, 0, "rc2-20150322-01") },
                { "Microsoft.CodeAnalysis.CSharp", new SemanticVersion(1, 0, 0, "rc2-20150322-01") }
            };

            var sources = new[] 
            {
                new PackageSource("https://www.myget.org/F/roslyn-nightly/"), 
                new PackageSource("https://packages.nuget.org/api/v2")
            };

            return new RoslynInstallerInstructions(paths, packages, sources);
        }
    }
}
