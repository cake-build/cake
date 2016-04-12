using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Common.Tools
{
    /// <summary>
    /// Define the available runtime values when working with DN* tools
    /// </summary>
    public enum DNRuntime
    {
        /// <summary>
        /// The full CLR on Windows
        /// </summary>
        Clr,
        
        /// <summary>
        /// The .NET core clr (Windows / *unix)
        /// </summary>
        CoreClr,
        
        /// <summary>
        /// The Mono CLR (Windows / *unix)
        /// </summary>
        Mono
    }
}
