using System.Runtime.Versioning;
using Cake.Core.Polyfill;

namespace Cake.Core
{
    /// <summary>
    /// Extensions for ICakeRuntime
    /// </summary>
    public static class CakeRuntimeExtensions
    {
        /// <summary>
        /// Does a default translation of the Runtime to a FrameworkName for purposes of internal NuGet resolution
        /// </summary>
        /// <param name="runtime">Cake Runtime</param>
        /// <returns>FrameworkName</returns>
        /// <exception cref="CakeException">Unknown runtime</exception>
        public static FrameworkName GetExecutingFramework(this ICakeRuntime runtime)
        {
            switch (runtime.Runtime)
            {
                case Runtime.Clr:
                    return new FrameworkName(".NETFramework,Version=v4.6.1");
                case Runtime.CoreClr:
                    return new FrameworkName(".NETStandard,Version=v2.0");
                default:
                    throw new CakeException($"Unknown Runtime value {runtime.Runtime}");
            }
        }
    }
}