// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.ContinuaCI;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class ContinuaCIFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public ContinuaCIFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("C:\\build\\CAKE-CAKE-JOB1");
            Environment.GetEnvironmentVariable("ContinuaCI.Version").Returns((string)null);
        }

        public void IsRunningOnContinuaCI()
        {
            Environment.GetEnvironmentVariable("ContinuaCI.Version").Returns("1.7.0.666");
        }

        public ContinuaCIProvider CreateContinuaCIService()
        {
            return new ContinuaCIProvider(Environment);
        }
    }
}
