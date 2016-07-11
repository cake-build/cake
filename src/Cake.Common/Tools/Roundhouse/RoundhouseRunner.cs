// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

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
        /// <param name="tools">The tool locator.</param>
        public RoundhouseRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Runs Roundhouse with the given settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="drop">Will drop/delete the database if set to <c>true</c>.</param>
        public void Run(RoundhouseSettings settings, bool drop = false)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Drop |= drop;
            Run(settings, GetArguments(settings));
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
            AppendQuotedSecretIfExists(builder, "cs", settings.ConnectionString);
            AppendQuotedSecretIfExists(builder, "csa", settings.ConnectionStringAdmin);
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
                builder.Append("--{0}", key);
            }
        }

        private static void AppendQuotedIfExists(ProcessArgumentBuilder builder, string key, object value)
        {
            if (value != null)
            {
                builder.AppendQuoted("--{0}={1}", key, value);
            }
        }

        private static void AppendQuotedSecretIfExists(ProcessArgumentBuilder builder, string key, object value)
        {
            if (value != null)
            {
                builder.AppendQuotedSecret("--{0}={1}", key, value);
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
