using Cake.Core.IO;

namespace Cake.Scripting.Roslyn.Installation
{
    /// <summary>
    /// Represents an installer for NuGet packages.
    /// </summary>
    public interface IRoslynInstaller
    {
        /// <summary>
        /// Determines whether Roslyn is installed using the specified installer instructions.
        /// </summary>
        /// <param name="root">The application root.</param>
        /// <param name="instructions">The instructions.</param>
        /// <returns><c>true</c> if Roslyn is installed; otherwise <c>false</c>.</returns>
        bool IsInstalled(DirectoryPath root, RoslynInstallerInstructions instructions);

        /// <summary>
        /// Installs Roslyn in the specified application root.
        /// </summary>
        /// <param name="root">The application root.</param>
        /// <param name="instructions">The instructions.</param>
        void Install(DirectoryPath root, RoslynInstallerInstructions instructions);
    }
}
