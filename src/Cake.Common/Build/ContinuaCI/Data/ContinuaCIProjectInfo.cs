using System.Globalization;
using Cake.Core;

namespace Cake.Common.Build.ContinuaCI.Data
{
    /// <summary>
    /// Provides Continua CI project information for a current build.
    /// </summary>
    public sealed class ContinuaCIProjectInfo : ContinuaCIInfo
    {
        private readonly string _prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuaCIProjectInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="prefix">The environment variable key prefix.</param>
        public ContinuaCIProjectInfo(ICakeEnvironment environment, string prefix)
            : base(environment)
        {
            _prefix = prefix;
        }

        /// <summary>
        /// Gets the Continua CI Project Name
        /// </summary>
        /// <value>
        ///   The Continua CI Project Name.
        /// </value>
        public string Name
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Name", _prefix);
                return GetEnvironmentString(key);
            }
        }
    }
}