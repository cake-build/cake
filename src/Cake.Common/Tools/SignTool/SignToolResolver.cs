using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Microsoft.Win32;

namespace Cake.Common.Tools.SignTool
{
    internal static class SignToolResolver
    {
        private static string _programFiles;
        private static string _signToolPath;
        public static FilePath GetSignToolPath(ICakeEnvironment environment)
        {
             //try SDK first
            if (_programFiles == null)
                _programFiles = environment.Is64BitOperativeSystem()
                    ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                    : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);


            if (_signToolPath == null)
                _signToolPath = (
                    from path in (environment.Is64BitOperativeSystem())
                        ? new[]
                        {
                            System.IO.Path.Combine(_programFiles, @"Windows Kits\8.1\bin\x64\signtool.exe"),
                            System.IO.Path.Combine(_programFiles, @"Windows Kits\8.0\bin\x64\signtool.exe"),
                            System.IO.Path.Combine(_programFiles, @"Microsoft SDKs\Windows\v7.1A\Bin\signtool.exe")
                        }
                        : new[]
                        {
                            System.IO.Path.Combine(_programFiles, @"Windows Kits\8.1\bin\x86\signtool.exe"),
                            System.IO.Path.Combine(_programFiles, @"Windows Kits\8.0\bin\x86\signtool.exe"),
                            System.IO.Path.Combine(_programFiles, @"Microsoft SDKs\Windows\v7.1A\Bin\signtool.exe")
                        }
                    where System.IO.File.Exists(path)
                    orderby path descending
                    select path
                    ).FirstOrDefault();

            if (!string.IsNullOrEmpty(_signToolPath))
            {
                return _signToolPath;
            }

            //try fallback to registry for older SDK's
            using (var windows = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Microsoft SDKs\\Windows")
                )
            {
                if (windows == null)
                    throw new CakeException("No Microsoft Windows SDKs key found");

                _signToolPath = (
                    from key in windows.GetSubKeyNames()
                    let sdk = windows.OpenSubKey(key)
                    where sdk != null
                    let installationFolder = sdk.GetValue("InstallationFolder") as string
                    where !string.IsNullOrEmpty(installationFolder) && System.IO.Directory.Exists(installationFolder)
                    let signToolPath = System.IO.Path.Combine(installationFolder, "bin\\signtool.exe")
                    where System.IO.File.Exists(signToolPath)
                    orderby key descending
                    select signToolPath
                    ).FirstOrDefault();
            }

            if (string.IsNullOrEmpty(_signToolPath) || !System.IO.File.Exists(_signToolPath))
                throw new CakeException("Failed to find signtool");

            return _signToolPath;
        }
    }
}
