using Cake.Core.IO;
using System.Collections.Generic;

namespace Cake.Common.Tools.DNX.Run
{
    /// <summary>
    /// Contains settings used by <see cref="DNXRunner"/>
    /// </summary>
    public class DNXRunSettings : DNSettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DNXRunSettings"/> class
        /// </summary>
        public DNXRunSettings()
        {
            this.Lib = new HashSet<DirectoryPath>(PathComparer.Default);
        }

        /// <summary>
        /// Gets or sets the project dir or the project.json file to be used (see dnx --project option)
        /// </summary>
        public Path Project { get; set; }

        /// <summary>
        /// Gets or sets the framework to be used when running dnx
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets the configuration to be used when running dnx (see dnx --configuration option)
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets the app base dir to be used by dnx (see dnx --appbase option)
        /// </summary>
        public DirectoryPath AppBase { get; set; }

        /// <summary>
        /// Gets or sets the lib dir to be used by dnx (see dnx --lib option)
        /// </summary>
        public ICollection<DirectoryPath> Lib { get; set; }

        /// <summary>
        /// Gets or sets the packages directory to be ysed by dnx (see dnx --packages option)
        /// </summary>
        public DirectoryPath Packages { get; set; }
    }
}
