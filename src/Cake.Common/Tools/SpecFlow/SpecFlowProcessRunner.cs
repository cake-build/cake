// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Common.Tools.SpecFlow
{
    internal sealed class SpecFlowProcessRunner : IProcessRunner
    {
        public FilePath FilePath { get; set; }

        public ProcessSettings ProcessSettings { get; set; }

        private sealed class InterceptedProcess : IProcess
        {
            public void Dispose()
            {
            }

            public void WaitForExit()
            {
            }

            public bool WaitForExit(int milliseconds)
            {
                return true;
            }

            public int GetExitCode()
            {
                return 0;
            }

            public IEnumerable<string> GetStandardOutput()
            {
                return Enumerable.Empty<string>();
            }

            public void Kill()
            {
            }
        }

        public IProcess Start(FilePath filePath, ProcessSettings settings)
        {
            FilePath = filePath;
            ProcessSettings = settings;
            return new InterceptedProcess();
        }
    }
}
