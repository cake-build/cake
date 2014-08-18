using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.AliaSql
{
    /// <summary>
    /// Contains functionality related to running AliaSql.
    /// </summary>
    [CakeAliasCategory("AliaSql")]
    public static class AliaSqlExtensions
    {
        /// <summary>
        /// Runs AliaSQL.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="command">The AliaSql command (ex. Rebuild, Create).</param>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="databaseName">The database name.</param>
        /// <param name="scriptsPath">The aliasql scripts path.</param>
        [CakeMethodAlias]
        public static void AliaSql(this ICakeContext context, string command,
            string connectionString, string databaseName, DirectoryPath scriptsPath)
        {
            AliaSql(context, new AliaSqlSettings
            {
                Command = command,
                ConnectionString = connectionString,
                DatabaseName = databaseName,
                ScriptsFolder = scriptsPath
            });
        }

        /// <summary>
        /// Runs AliaSQL.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void AliaSql(this ICakeContext context, AliaSqlSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var runner = new AliaSqlRunner(context.FileSystem, context.Environment, context.Globber,
                context.ProcessRunner);

            runner.Run(settings);
        }

        /// <summary>
        /// Runs AliaSQL.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="command">The AliaSql command (ex. Rebuild, Create).</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void AliaSql(this ICakeContext context, string command, AliaSqlSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Command = command;

            var runner = new AliaSqlRunner(context.FileSystem, context.Environment, context.Globber,
                context.ProcessRunner);

            runner.Run(settings);
        }
    }
}
