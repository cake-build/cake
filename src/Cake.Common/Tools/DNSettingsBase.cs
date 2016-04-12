using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.Tooling;

namespace Cake.Common.Tools
{
    /// <summary>
    /// Represent the base class for all DN* tools settings
    /// </summary>
    public abstract class DNSettingsBase : ToolSettings
    {
        /// <summary>
        /// Gets or sets the DNX Version to be used (see dnvm help run for more information)
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the DNX architecture to be used (see dnvm help run for more information)
        /// </summary>
        public DNArchitecture? Architecture { get; set; }

        /// <summary>
        /// Gets or sets the DNX runtime to be used (see dnvm help run for more information)
        /// </summary>
        public DNRuntime? Runtime { get; set; }
    }
}
