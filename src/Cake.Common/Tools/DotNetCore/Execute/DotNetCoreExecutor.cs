using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.Execute
{
    /// <summary>
    /// .NET Core assembly executor.
    /// </summary>
    public sealed class DotNetCoreExecutor : DotNetCoreTool<DotNetCoreSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreExecutor" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public DotNetCoreExecutor(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Execute an assembly using arguments and settings.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        public void Execute(FilePath assemblyPath, string arguments, DotNetCoreSettings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            
            Run(settings, GetArguments(assemblyPath, arguments, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath assemblyPath, string arguments, DotNetCoreSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            assemblyPath = assemblyPath.IsRelative ? assemblyPath.MakeAbsolute(_environment) : assemblyPath;
            builder.Append(assemblyPath.FullPath);

            if (!string.IsNullOrEmpty(arguments))
            {
                builder.Append(arguments);
            }

            return builder;
        }
    }
}
