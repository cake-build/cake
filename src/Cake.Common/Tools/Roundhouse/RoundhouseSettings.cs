// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Roundhouse
{
    /// <summary>
    /// Contains settings used by <see cref="RoundhouseRunner" />.
    /// </summary>
    public sealed class RoundhouseSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        /// <value>
        /// The server on which create/migrate should happen.
        /// </value>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        /// <value>
        /// The database you want to create/migrate.
        /// </value>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// As an alternative to ServerName and Database - You can provide an entire connection string instead.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the connection string for admin connections.
        /// </summary>
        /// <value>
        /// This is used for connecting to master when you may have a different uid and password than normal.
        /// </value>
        public string ConnectionStringAdmin { get; set; }

        /// <summary>
        /// Gets or sets the timeout (in seconds) for normal connections.
        /// </summary>
        /// <value>
        /// This is the timeout when commands are run. This is not for admin commands or restore.
        /// </value>
        public int? CommandTimeout { get; set; }

        /// <summary>
        /// Gets or sets the timeout (in seconds) for admin connections.
        /// </summary>
        /// <value>
        /// This is the timeout when administration commands are run (except for restore, which has its own).
        /// </value>
        public int? CommandTimeoutAdmin { get; set; }

        /// <summary>
        /// Gets or sets the sql files directory.
        /// </summary>
        /// <value>
        /// The directory where your SQL scripts are.
        /// </value>
        public string SqlFilesDirectory { get; set; }

        /// <summary>
        /// Gets or sets the location of the source code repository
        /// </summary>
        /// <value>
        /// Path to code repository to be able to correlate versions
        /// </value>
        public string RepositoryPath { get; set; }

        /// <summary>
        /// Gets or sets the version file.
        /// </summary>
        /// <value>
        /// Path to the file to use for applying version number. Either a .XML file, a .DLL or a .TXT file that a version can be resolved from.
        /// </value>
        public string VersionFile { get; set; }

        /// <summary>
        /// Gets or sets the XPath to locate version in the <see cref="VersionFile" />.
        /// </summary>
        /// <value>
        /// Works in conjunction with an XML version file.
        /// </value>
        public string VersionXPath { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'alterDatabase' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you keep your alter database scripts. Read up on token replacement. You will want to use {{DatabaseName}} here instead of specifying a database name.
        /// </value>
        public string AlterDatabaseFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'runAfterCreateDatabase' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you will keep scripts that ONLY run after a database is created.
        /// </value>
        public string RunAfterCreateDatabaseFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'runBeforeUp' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you keep scripts that you want to run before your update scripts.
        /// </value>
        public string RunBeforeUpFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'up' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you keep your update scripts.
        /// </value>
        public string UpFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'runFirstAfterUp' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you keep any functions, views, or sprocs that are order dependent. If you have a function that depends on a view, you definitely need the view in this folder.
        /// </value>
        public string RunFirstAfterUpFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'functions' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you keep your functions.
        /// </value>
        public string FunctionsFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'views' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you keep your views.
        /// </value>
        public string ViewsFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'sprocs' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you keep your stored procedures.
        /// </value>
        public string SprocsFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'indexes' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you keep your indexes.
        /// </value>
        public string IndexesFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'runAfterOtherAnyTimeScripts' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you keep scripts that will be run after all of the other any time scripts complete.
        /// </value>
        public string RunAfterOtherAnyTimeScriptsFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'permissions' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder where you keep your permissions scripts.
        /// </value>
        public string PermissionsFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'beforeMig' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder for scripts to run before migration and outside of a transaction.
        /// </value>
        public string BeforeMigrationFolderName { get; set; }

        /// <summary>
        /// Gets or sets the folder name for 'afterMig' scripts.
        /// </summary>
        /// <value>
        /// The name of the folder for scripts to run before migration and outside of a transaction.
        /// </value>
        public string AfterMigrationFolderName { get; set; }

        /// <summary>
        /// Gets or sets the schema name to use instead of [RoundhousE].
        /// </summary>
        /// <value>
        /// The schema where RH stores it's tables.
        /// </value>
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets or sets the environment for RH to be scoped.
        /// </summary>
        /// <value>
        /// This allows RH to be environment aware and only run scripts that are in a particular environment based on the namingof the script. LOCAL.something**.ENV.**sql would only be run in the LOCAL environment.
        /// </value>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether perform a restore.
        /// </summary>
        /// <value>
        /// This instructs RH to do a restore (with the <see cref="RestoreFilePath"/> parameter) of a database before running migration scripts.
        /// </value>
        public bool Restore { get; set; }

        /// <summary>
        /// Gets or sets the restore file path.
        /// </summary>
        /// <value>
        /// File path of back when Restore is set to true
        /// </value>
        public string RestoreFilePath { get; set; }

        /// <summary>
        /// Gets or sets the custom database creation script.
        /// </summary>
        /// <value>
        /// This instructs RH to use this script for creating a database instead of the default based on the SQLType.
        /// </value>
        public string CreateDatabaseCustomScript { get; set; }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>
        /// Path to where migration artifacts are stored.
        /// </value>
        public string OutputPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to warn when previously run scripts have changed.
        /// </summary>
        /// <value>
        /// Instructs RH to execute changed one time scripts (DDL/DML in 'Up'/<see cref="UpFolderName"/>) that have previously been run against the database instead of failing. A warning is logged for each one time scripts that is rerun.
        /// </value>
        public bool WarnOnOneTimeScriptChanges { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether to keep RH silent.
        /// </summary>
        /// <value>
        /// Tells RH not to ask for any input when it runs.
        /// </value>
        public bool Silent { get; set; }

        /// <summary>
        /// Gets or sets database type.
        /// </summary>
        /// <value>
        /// Database Type (fully qualified class name implementing [roundhouse.sql.Database, roundhouse])
        /// </value>
        public string DatabaseType { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether to drop the DB.
        /// </summary>
        /// <value>
        /// This instructs RH to remove a database and not run migration scripts.
        /// </value>
        internal bool Drop { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether to use transactions.
        /// </summary>
        /// <value>
        /// This instructs RH to run inside of a transaction.
        /// </value>
        public bool WithTransaction { get; set; }

        /// <summary>
        /// Gets or sets SQL Server recovery mode.
        /// </summary>
        /// <value>
        /// This sets the recovery model for SQL Server during migration. (NoChange, Simple, Full)
        /// </value>
        public RecoveryMode? RecoveryMode { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether to perform a dry run.
        /// </summary>
        /// <value>
        /// This instructs RH to log what would have run, but not to actually run anything against the database. Use this option if you are trying to figure out what RH is going to do.
        /// </value>
        public bool DryRun { get; set; }
    }
}
