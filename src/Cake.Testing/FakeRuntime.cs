using System;
using System.Runtime.Versioning;
using Cake.Core;

namespace Cake.Testing
{
    /// <summary>
    /// An implementation of a fake <see cref="ICakeRuntime"/>
    /// </summary>
    public sealed class FakeRuntime : ICakeRuntime
    {
        /// <summary>
        /// Gets or sets the target .NET framework version that the current AppDomain is targeting.
        /// </summary>
        public FrameworkName TargetFramework { get; set; }

        /// <summary>
        /// Gets the version of Cake executing the script.
        /// </summary>
        public Version CakeVersion { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeRuntime"/> class.
        /// </summary>
        public FakeRuntime()
        {
            TargetFramework = new FrameworkName(".NETFramework,Version=v4.5");
            CakeVersion = typeof(ICakeRuntime).Assembly.GetName().Version;
        }
    }
}