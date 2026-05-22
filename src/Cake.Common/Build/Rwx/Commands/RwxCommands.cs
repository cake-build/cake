// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Cake.Common.Build.Rwx.Data;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.Rwx.Commands
{
    /// <summary>
    /// Provides RWX commands for the current build, allowing tasks to write
    /// output values, export environment variables to downstream tasks, and
    /// upload artifacts at runtime via the filesystem conventions documented at
    /// <see href="https://www.rwx.com/docs/output-values"/>,
    /// <see href="https://www.rwx.com/docs/use#environment-variables"/>,
    /// and <see href="https://www.rwx.com/docs/artifacts"/>.
    /// </summary>
    public sealed class RwxCommands
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;
        private readonly RwxEnvironmentInfo _rwxEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="RwxCommands"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="rwxEnvironment">The RWX environment.</param>
        public RwxCommands(
            ICakeEnvironment environment,
            IFileSystem fileSystem,
            RwxEnvironmentInfo rwxEnvironment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _rwxEnvironment = rwxEnvironment ?? throw new ArgumentNullException(nameof(rwxEnvironment));
        }

        /// <summary>
        /// Sets an RWX output value for the current task. The value is written to
        /// <c>{RWX_VALUES}/{key}</c>; each key is a separate file, so repeated
        /// calls with the same key overwrite the previous value. Multiline values
        /// are written verbatim.
        /// </summary>
        /// <param name="key">The value key. May not contain path separators or <c>..</c>.</param>
        /// <param name="value">The value contents.</param>
        public void SetValue(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            ArgumentNullException.ThrowIfNull(value);

            // RWX writes each value into its own file inside $RWX_VALUES, keyed by filename.
            // A separator or `..` in the key would let callers write outside that directory.
            if (key.Contains('/') || key.Contains('\\') || key.Contains(".."))
            {
                throw new CakeException("RWX value key may not contain path separators or '..'.");
            }

            if (_rwxEnvironment.Runtime.ValuesPath == null)
            {
                throw new CakeException("RWX Runtime ValuesPath missing.");
            }

            var valuesPath = _rwxEnvironment.Runtime.ValuesPath.MakeAbsolute(_environment);
            var valuesDirectory = _fileSystem.GetDirectory(valuesPath);
            if (!valuesDirectory.Exists)
            {
                valuesDirectory.Create();
            }

            var targetPath = valuesPath.CombineWithFilePath(key);
            var file = _fileSystem.GetFile(targetPath);
            using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(stream);
            writer.Write(value);
        }

        /// <summary>
        /// Exports an environment variable for downstream RWX tasks. The value is
        /// written to <c>{RWX_ENV}/{name}</c>; each name is a separate file, so
        /// repeated calls with the same name overwrite the previous value. The
        /// variable becomes visible to tasks that depend on the current one via
        /// <c>use</c>. Note that RWX trims a single trailing <c>\n</c> from the
        /// file contents when materializing the variable.
        /// </summary>
        /// <param name="name">The environment variable name. May not contain path separators or <c>..</c>.</param>
        /// <param name="value">The environment variable value.</param>
        public void SetEnvironmentVariable(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            ArgumentNullException.ThrowIfNull(value);

            // RWX writes each env var into its own file inside $RWX_ENV, keyed by filename;
            // files in subdirectories are ignored by the runtime. A separator or `..` in the
            // name would let callers write outside that directory or into an ignored location.
            if (name.Contains('/') || name.Contains('\\') || name.Contains(".."))
            {
                throw new CakeException("RWX environment variable name may not contain path separators or '..'.");
            }

            if (_rwxEnvironment.Runtime.EnvPath == null)
            {
                throw new CakeException("RWX Runtime EnvPath missing.");
            }

            var envPath = _rwxEnvironment.Runtime.EnvPath.MakeAbsolute(_environment);
            var envDirectory = _fileSystem.GetDirectory(envPath);
            if (!envDirectory.Exists)
            {
                envDirectory.Create();
            }

            var targetPath = envPath.CombineWithFilePath(name);
            var file = _fileSystem.GetFile(targetPath);
            using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(stream);
            writer.Write(value);
        }

        /// <summary>
        /// Uploads a file as an RWX artifact for the current task. The file is
        /// copied into <c>{RWX_ARTIFACTS}/{filename}</c>, where <c>filename</c> is
        /// the leaf name of <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Path to the local file to upload.</param>
        public void UploadArtifact(FilePath path)
        {
            ArgumentNullException.ThrowIfNull(path);

            if (_rwxEnvironment.Runtime.ArtifactsPath == null)
            {
                throw new CakeException("RWX Runtime ArtifactsPath missing.");
            }

            var absoluteFilePath = path.MakeAbsolute(_environment);
            var file = _fileSystem.GetFile(absoluteFilePath);
            if (!file.Exists)
            {
                throw new FileNotFoundException("Artifact file not found.", absoluteFilePath.FullPath);
            }

            var artifactsPath = _rwxEnvironment.Runtime.ArtifactsPath.MakeAbsolute(_environment);
            var artifactsDirectory = _fileSystem.GetDirectory(artifactsPath);
            if (!artifactsDirectory.Exists)
            {
                artifactsDirectory.Create();
            }

            var destination = artifactsPath.CombineWithFilePath(absoluteFilePath.GetFilename());
            file.Copy(destination, true);
        }
    }
}
