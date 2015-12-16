using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DNVM.Use
{
    /// <summary>
    /// Settings for the dnvm command
    /// </summary>
    public class DNVMSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the runtime to be used
        /// </summary>
        public string Runtime { get; set; }

        /// <summary>
        /// Gets or sets the arch to be used
        /// </summary>
        public string Arch { get; set; }
    }
}
