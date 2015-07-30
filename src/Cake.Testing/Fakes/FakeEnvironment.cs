using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Testing.Fakes
{
    /// <summary>
    /// Represents a fake environment.
    /// </summary>
    public class FakeEnvironment : ICakeEnvironment
    {
        private readonly bool _isUnix;

        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        public DirectoryPath WorkingDirectory { get; set; }

        private FakeEnvironment(bool isUnix)
        {
            _isUnix = isUnix;
            WorkingDirectory = new DirectoryPath("/Working");
        }

        /// <summary>
        /// Creates a Unix environment.
        /// </summary>
        /// <returns>A Unix environment.</returns>
        public static FakeEnvironment CreateUnixEnvironment()
        {
            return new FakeEnvironment(true);
        }

        /// <summary>
        /// Creates a Windows environment.
        /// </summary>
        /// <returns></returns>
        public static FakeEnvironment CreateWindowsEnvironment()
        {
            var environment = new FakeEnvironment(false);
            environment.WorkingDirectory = new DirectoryPath("C:/Working");
            return environment;
        }

        /// <summary>
        /// Gets whether or not the current operative system is 64 bit.
        /// </summary>
        /// <returns>Whether or not the current operative system is 64 bit.</returns>
        public bool Is64BitOperativeSystem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        public bool IsUnix()
        {
            return _isUnix;
        }

        /// <summary>
        /// Gets a special path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A <see cref="DirectoryPath" /> to the special path.
        /// </returns>
        public DirectoryPath GetSpecialPath(SpecialPath path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the application root path.
        /// </summary>
        /// <returns>
        /// The application root path.
        /// </returns>
        public DirectoryPath GetApplicationRoot()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an environment variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns>
        /// The value of the environment variable.
        /// </returns>
        public string GetEnvironmentVariable(string variable)
        {
            throw new NotImplementedException();
        }
    }
}
