// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.SignTool
{
    internal sealed class SignToolResolver : ISignToolResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IRegistry _registry;
        private FilePath _signToolPath;

        public SignToolResolver(IFileSystem fileSystem, ICakeEnvironment environment, IRegistry registry)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _registry = registry;

            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
        }

        public FilePath GetPath()
        {
            if (_signToolPath != null)
            {
                return _signToolPath;
            }

            // Try look for the path.
            _signToolPath = GetFromDisc() ?? GetFromRegistry();
            if (_signToolPath == null)
            {
                throw new CakeException("Failed to find signtool.exe.");
            }

            // Return the sign tool path.
            return _signToolPath;
        }

        private FilePath GetFromDisc()
        {
            // Get the path to program files.
            var programFilesPath = _environment.Is64BitOperativeSystem()
                ? _environment.GetSpecialPath(SpecialPath.ProgramFilesX86)
                : _environment.GetSpecialPath(SpecialPath.ProgramFiles);

            // Get a list of the files we should check.
            var files = new List<FilePath>();
            if (_environment.Is64BitOperativeSystem())
            {
                // 64-bit specific paths.
                files.Add(programFilesPath.Combine(@"Windows Kits\8.1\bin\x64").CombineWithFilePath("signtool.exe"));
                files.Add(programFilesPath.Combine(@"Windows Kits\8.0\bin\x64").CombineWithFilePath("signtool.exe"));
                files.Add(programFilesPath.Combine(@"Microsoft SDKs\Windows\v7.1A\Bin").CombineWithFilePath("signtool.exe"));
            }
            else
            {
                // 32-bit specific paths.
                files.Add(programFilesPath.Combine(@"Windows Kits\8.1\bin\x86").CombineWithFilePath("signtool.exe"));
                files.Add(programFilesPath.Combine(@"Windows Kits\8.0\bin\x86").CombineWithFilePath("signtool.exe"));
                files.Add(programFilesPath.Combine(@"Microsoft SDKs\Windows\v7.1A\Bin").CombineWithFilePath("signtool.exe"));
            }

            // Return the first path that exist.
            return files.FirstOrDefault(file => _fileSystem.Exist(file));
        }

        private FilePath GetFromRegistry()
        {
            using (var root = _registry.LocalMachine.OpenKey("Software\\Microsoft\\Microsoft SDKs\\Windows"))
            {
                if (root == null)
                {
                    return null;
                }

                var keyName = root.GetSubKeyNames();
                foreach (var key in keyName)
                {
                    var sdkKey = root.OpenKey(key);
                    if (sdkKey != null)
                    {
                        var installationFolder = sdkKey.GetValue("InstallationFolder") as string;
                        if (!string.IsNullOrWhiteSpace(installationFolder))
                        {
                            var installationPath = new DirectoryPath(installationFolder);
                            var signToolPath = installationPath.CombineWithFilePath("bin\\signtool.exe");

                            if (_fileSystem.Exist(signToolPath))
                            {
                                return signToolPath;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
