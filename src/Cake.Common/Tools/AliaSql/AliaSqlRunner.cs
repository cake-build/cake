using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.AliaSql
{
    /// <summary>
    /// Runner for AliaSQL.
    /// </summary>
    public sealed class AliaSqlRunner : Tool<AliaSqlSettings>
    {
        private readonly IGlobber _globber;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliaSqlRunner"/> class.
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="environment"></param>
        /// <param name="globber"></param>
        /// <param name="processRunner"></param>
        public AliaSqlRunner(IFileSystem fileSystem, ICakeEnvironment environment,
            IGlobber globber, IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner)
        {
            _globber = globber;
        }

        /// <summary>
        /// Runs AliaSql with the provided settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Run(AliaSqlSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(settings), settings.ToolPath);
        }

        private ToolArgumentBuilder GetArguments(AliaSqlSettings settings)
        {
            // AliaSql Format: [Command] [Database Server] [Database Name] [Scripts path] 
            var builder = new ToolArgumentBuilder();
            builder.AppendQuotedText(settings.Command);
            builder.AppendQuotedText(settings.ConnectionString);
            builder.AppendQuotedText(settings.DatabaseName);
            builder.AppendQuotedText(settings.ScriptsFolder.FullPath);
            return builder;
        }

        /// <summary>
        /// Get AliaSql tool name.
        /// </summary>
        /// <returns></returns>
        protected override string GetToolName()
        {
            return "AliaSql";
        }

        /// <summary>
        /// Get AliaSql's default path.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        protected override FilePath GetDefaultToolPath(AliaSqlSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (settings.ToolPath != null)
                return settings.ToolPath;

            const string expression = "./tools/**/AliaSql.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }
    }
}
