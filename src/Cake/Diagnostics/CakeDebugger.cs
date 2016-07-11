// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Cake.Diagnostics
{
    internal sealed class CakeDebugger : IDebugger
    {
        public int GetProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }

        public bool WaitForAttach(TimeSpan timeout)
        {
            return Task.Run(() =>
            {
                while (!Debugger.IsAttached)
                {
                    Thread.Sleep(100);
                }
            }).Wait(timeout);
        }
    }
}
