using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.Roundhouse
{
    /// <summary>
    /// The Roundhouse console application runner.
    /// </summary>
    public sealed class RoundhouseRunner : Tool<RoundhouseSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundhouseRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public RoundhouseRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
        }

        /// <summary>
        /// Runs Roundhouse with the given settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Run(RoundhouseSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(settings), settings.ToolPath);
        }

        private ProcessArgumentBuilder GetArguments(RoundhouseSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            AddFolderArguments(builder, settings);
            AddFlagArguments(builder, settings);
            AddDatabaseArguments(builder, settings);
            AddRoundhouseArguments(builder, settings);

            return builder;
        }

        private static void AddFolderArguments(ProcessArgumentBuilder builder, RoundhouseSettings settings)
        {
            AppendQuotedIfExists(builder, "amg", settings.AfterMigrationFolderName);
            AppendQuotedIfExists(builder, "ad", settings.AlterDatabaseFolderName);
            AppendQuotedIfExists(builder, "bmg", settings.BeforeMigrationFolderName);
            AppendQuotedIfExists(builder, "fu", settings.FunctionsFolderName);
            AppendQuotedIfExists(builder, "ix", settings.IndexesFolderName);
            AppendQuotedIfExists(builder, "p", settings.PermissionsFolderName);
            AppendQuotedIfExists(builder, "racd", settings.RunAfterCreateDatabaseFolderName);
            AppendQuotedIfExists(builder, "ra", settings.RunAfterOtherAnyTimeScriptsFolderName);
            AppendQuotedIfExists(builder, "rb", settings.RunBeforeUpFolderName);
            AppendQuotedIfExists(builder, "rf", settings.RunFirstAfterUpFolderName);
            AppendQuotedIfExists(builder, "sp", settings.SprocsFolderName);
            AppendQuotedIfExists(builder, "vw", settings.ViewsFolderName);
            AppendQuotedIfExists(builder, "u", settings.UpFolderName);
        }

        private static void AddFlagArguments(ProcessArgumentBuilder builder, RoundhouseSettings settings)
        {
            AppendFlag(builder, "drop", settings.Drop);
            AppendFlag(builder, "dryrun", settings.DryRun);
            AppendFlag(builder, "restore", settings.Restore);
            AppendFlag(builder, "silent", settings.Silent);
            AppendFlag(builder, "w", settings.WarnOnOneTimeScriptChanges);
            AppendFlag(builder, "t", settings.WithTransaction);
        }

        private static void AddDatabaseArguments(ProcessArgumentBuilder builder, RoundhouseSettings settings)
        {
            AppendQuotedIfExists(builder, "ct", settings.CommandTimeout);
            AppendQuotedIfExists(builder, "cta", settings.CommandTimeoutAdmin);
            AppendQuotedIfExists(builder, "cs", settings.ConnectionString);
            AppendQuotedIfExists(builder, "csa", settings.ConnectionStringAdmin);
            AppendQuotedIfExists(builder, "d", settings.DatabaseName);
            AppendQuotedIfExists(builder, "rcm", settings.RecoveryMode);
            AppendQuotedIfExists(builder, "rfp", settings.RestoreFilePath);
            AppendQuotedIfExists(builder, "sc", settings.SchemaName);
            AppendQuotedIfExists(builder, "s", settings.ServerName);
        }

        private static void AddRoundhouseArguments(ProcessArgumentBuilder builder, RoundhouseSettings settings)
        {
            AppendQuotedIfExists(builder, "cds", settings.CreateDatabaseCustomScript);
            AppendQuotedIfExists(builder, "dt", settings.DatabaseType);
            AppendQuotedIfExists(builder, "env", settings.Environment);
            AppendQuotedIfExists(builder, "o", settings.OutputPath);
            AppendQuotedIfExists(builder, "r", settings.RepositoryPath);
            AppendQuotedIfExists(builder, "f", settings.SqlFilesDirectory);
            AppendQuotedIfExists(builder, "vf", settings.VersionFile);
            AppendQuotedIfExists(builder, "vx", settings.VersionXPath);
        }

        private static void AppendFlag(ProcessArgumentBuilder builder, string key, bool value)
        {
            if (value)
            {
                builder.Append(string.Format("--{0}", key));
            }
        }

        private static void AppendQuotedIfExists(ProcessArgumentBuilder builder, string key, object value)
        {
            if (value != null)
            {
                builder.AppendQuoted(string.Format("--{0}={1}", key, value));
            }
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Roundhouse";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "rh.exe" };
        }
    }
}