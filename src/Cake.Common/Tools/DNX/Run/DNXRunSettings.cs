using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DNX.Run
{
    /// <summary>
    /// Contains settings used by <see cref="DNXRunner"/>
    /// </summary>
    public class DNXRunSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the project dir to be used (see dnx --project option)
        /// </summary>
        public string Project { get; set; }

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
        public string AppBase { get; set; }

        /// <summary>
        /// Gets or sets the lib dir to be used by dnx (see dnx --lib option)
        /// </summary>
        public string Lib { get; set; }
    }
}
