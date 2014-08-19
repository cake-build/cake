using Cake.Core.IO;

namespace Cake.Common.Tools.AliaSql
{
    /// <summary>
    /// Contains settings used by <see cref="AliaSqlRunner"/>.
    /// </summary>
    public class AliaSqlSettings
    {       
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// The Connection String to connect to the target database.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The database to run scripts against.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// The scripts folder containing Create, Update, and/or TestData scripts.
        /// </summary>
        public DirectoryPath ScriptsFolder { get; set; }

        /// <summary>
        /// The AliaSql command to run. (ex: TestData, Rebuild)
        /// </summary>
        public string Command { get; set; }
    }
}
