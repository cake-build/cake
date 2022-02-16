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
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
        }

        /// <inheritdoc/>
        public FilePath GetPath()
        {
            if (_signToolPath != null)
            {
                return _signToolPath;
            }

            // Try look for the path.
            _signToolPath = GetFromRegistry() ?? GetFromDisc();
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
            var programFilesPath = _environment.Platform.Is64Bit
                ? _environment.GetSpecialPath(SpecialPath.ProgramFilesX86)
                : _environment.GetSpecialPath(SpecialPath.ProgramFiles);

            // Gets a list of the files we should check.
            var files = new List<FilePath>();
            if (_environment.Platform.Is64Bit)
            {
                // 64-bit specific paths.
                files.Add(programFilesPath.Combine(@"Windows Kits\10\bin\x64").CombineWithFilePath("signtool.exe"));
                files.Add(programFilesPath.Combine(@"Windows Kits\8.1\bin\x64").CombineWithFilePath("signtool.exe"));
                files.Add(programFilesPath.Combine(@"Windows Kits\8.0\bin\x64").CombineWithFilePath("signtool.exe"));
                files.Add(programFilesPath.Combine(@"Microsoft SDKs\Windows\v7.1A\Bin").CombineWithFilePath("signtool.exe"));
            }
            else
            {
                // 32-bit specific paths.
                files.Add(programFilesPath.Combine(@"Windows Kits\10\bin\x86").CombineWithFilePath("signtool.exe"));
                files.Add(programFilesPath.Combine(@"Windows Kits\8.1\bin\x86").CombineWithFilePath("signtool.exe"));
                files.Add(programFilesPath.Combine(@"Windows Kits\8.0\bin\x86").CombineWithFilePath("signtool.exe"));
                files.Add(programFilesPath.Combine(@"Microsoft SDKs\Windows\v7.1A\Bin").CombineWithFilePath("signtool.exe"));
            }
            files.Add(programFilesPath.Combine(@"Windows Kits\10\App Certification Kit").CombineWithFilePath("signtool.exe"));

            // Return the first path that exists.
            return files.FirstOrDefault(file => _fileSystem.Exist(file));
        }

        private FilePath GetFromRegistry()
        {
            // Gets a list of the files we should check.
            var files = new List<FilePath>();

            using (var root = _registry.LocalMachine.OpenKey("Software\\Microsoft\\Windows Kits\\Installed Roots"))
            {
                string kitsRoot = root?.GetValue("KitsRoot10") as string;
                if (!string.IsNullOrWhiteSpace(kitsRoot))
                {
                    var kitsPath = new DirectoryPath(kitsRoot);
                    string[] versions = root.GetSubKeyNames();

                    foreach (string version in versions.OrderByDescending(x => x))
                    {
                        var versionPath = kitsPath.Combine($"bin\\{version}");

                        files.Add(_environment.Platform.Is64Bit
                            ? versionPath.CombineWithFilePath("x64\\signtool.exe")
                            : versionPath.CombineWithFilePath("x86\\signtool.exe"));
                    }
                }
            }

            using (var root = _registry.LocalMachine.OpenKey("Software\\Microsoft\\Windows Kits\\Installed Roots"))
            {
                if (root != null)
                {
                    var windowsKits = new[] { "KitsRoot10", "KitsRoot81", "KitsRoot" };
                    foreach (var kit in windowsKits)
                    {
                        var kitsRoot = root.GetValue(kit) as string;
                        if (!string.IsNullOrWhiteSpace(kitsRoot))
                        {
                            var kitsPath = new DirectoryPath(kitsRoot);

                            files.Add(_environment.Platform.Is64Bit
                                ? kitsPath.CombineWithFilePath("bin\\x64\\signtool.exe")
                                : kitsPath.CombineWithFilePath("bin\\x86\\signtool.exe"));
                        }
                    }
                }
            }

            using (var root = _registry.LocalMachine.OpenKey("Software\\Microsoft\\Microsoft SDKs\\Windows"))
            {
                if (root != null)
                {
                    var keyName = root.GetSubKeyNames();
                    foreach (var key in keyName)
                    {
                        var sdkKey = root.OpenKey(key);
                        var installationFolder = sdkKey?.GetValue("InstallationFolder") as string;
                        if (!string.IsNullOrWhiteSpace(installationFolder))
                        {
                            var installationPath = new DirectoryPath(installationFolder);
                            files.Add(installationPath.CombineWithFilePath("bin\\signtool.exe"));
                        }
                    }
                }
            }

            // Return the first path that exists.
            return files.FirstOrDefault(file => _fileSystem.Exist(file));
        }
    }
}