using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.Test
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreTester" />.
    /// </summary>
    public sealed class DotNetCoreTestSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreTestSettings"/> class
        /// </summary>
        public DotNetCoreTestSettings()
        {
            this.AdditionalParameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the directory in which to place temporary outputs.
        /// </summary>
        public DirectoryPath BuildBasePath { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the target runtime.
        /// </summary>
        public string Runtime { get; set; }

        /// <summary>
        /// Gets or sets the configuration under which to build.
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets specific framework to compile.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not build the project before testing.
        /// </summary>
        public bool NoBuild { get; set; }

        /// <summary>
        /// Gets or sets the additional parameters to be sent to the test command
        /// </summary>
        public IDictionary<string, string> AdditionalParameters { get; set; }
    }
}
