using Cake.Common.Build.AppVeyor.Data;
using Cake.Core.IO;

namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// Represents a service that communicates with AppVeyor.
    /// </summary>
    public interface IAppVeyorProvider
    {
        /// <summary>
        /// Gets a value indicating whether the current build is running on AppVeyor.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on AppVeyor.; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnAppVeyor { get; }

        /// <summary>
        /// Gets the AppVeyor environment.
        /// </summary>
        /// <value>
        /// The AppVeyor environment.
        /// </value>
        AppVeyorEnvironmentInfo Environment { get; }

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        void UploadArtifact(FilePath path);
    }
}