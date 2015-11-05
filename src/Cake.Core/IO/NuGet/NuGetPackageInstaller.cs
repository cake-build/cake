namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Implementation for a NuGet package installer.
    /// </summary>
    public sealed class NuGetPackageInstaller : INuGetPackageInstaller
    {
        private readonly INuGetToolResolver _resolver;
        private readonly IProcessRunner _processRunner;
        private FilePath _nugetPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageInstaller"/> class.
        /// </summary>
        /// <param name="resolver">The NuGet tool resolver.</param>
        /// <param name="processRunner">The process runner.</param>
        public NuGetPackageInstaller(INuGetToolResolver resolver, IProcessRunner processRunner)
        {
            _resolver = resolver;
            _processRunner = processRunner;
        }

        /// <summary>
        /// Installs the specified package.
        /// </summary>
        /// <param name="package">The package definition.</param>
        /// <param name="installationRoot">The installation root path.</param>
        public void InstallPackage(NuGetPackage package, DirectoryPath installationRoot)
        {
            var nugetPath = GetNuGetPath();
            var process = _processRunner.Start(nugetPath, new ProcessSettings
            {
                Arguments = GetNuGetToolInstallArguments(package, installationRoot),
                RedirectStandardOutput = true,
                Silent = true
            });
            process.WaitForExit();
        }

        private FilePath GetNuGetPath()
        {
            var nugetPath = _nugetPath ?? (_nugetPath = _resolver.ResolvePath());
            if (nugetPath == null)
            {
                throw new CakeException("Failed to find NuGet.");
            }
            return nugetPath;
        }

        private static ProcessArgumentBuilder GetNuGetToolInstallArguments(
            NuGetPackage definition,
            DirectoryPath installationRoot)
        {
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("install");
            arguments.AppendQuoted(definition.PackageId);
            arguments.Append("-OutputDirectory");
            arguments.AppendQuoted(installationRoot.FullPath);
            if (!string.IsNullOrWhiteSpace(definition.Source))
            {
                arguments.Append("-Source");
                arguments.AppendQuoted(definition.Source);
            }
            arguments.Append("-ExcludeVersion -NonInteractive -NoCache");
            return arguments;
        }
    }
}