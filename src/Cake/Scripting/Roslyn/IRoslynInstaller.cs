using Cake.Core.IO;

namespace Cake.Scripting.Roslyn
{
    /// <summary>
    /// Represents an installer for Roslyn.
    /// </summary>
    public interface IRoslynInstaller
    {
        /// <summary>
        /// Determines whether or not Roslyn is installed.
        /// </summary>
        /// <param name="root">The application root.</param>
        /// <returns>Whether or not Roslyn is installed.</returns>
        bool IsInstalled(DirectoryPath root);

        /// <summary>
        /// Installs Roslyn in the specified application root.
        /// </summary>
        /// <param name="root">The application root.</param>
        void Install(DirectoryPath root);
    }
}
