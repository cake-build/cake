// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Cake.Frosting.Tests
{
    public sealed class CakeHostFixture
    {
        public CakeHost Host { get; set; }
        public FakeEnvironment Environment { get; set; }
        public FakeFileSystem FileSystem { get; set; }
        public FakeConsole Console { get; set; }
        public ICakeLog Log { get; set; }
        public IExecutionStrategy Strategy { get; set; }
        public IToolInstaller Installer { get; set; }

        public CakeHostFixture()
        {
            Host = new CakeHost();
            Environment = FakeEnvironment.CreateUnixEnvironment();
            Console = new FakeConsole();
            Log = Substitute.For<ICakeLog>();
            Installer = Substitute.For<IToolInstaller>();

            FileSystem = new FakeFileSystem(Environment);
            FileSystem.CreateDirectory("/Working");
        }

        public void RegisterTask<T>()
            where T : class, IFrostingTask
        {
            Host.ConfigureServices(services => services.AddSingleton<IFrostingTask, T>());
        }

        public int Run(params string[] args)
        {
            Host.ConfigureServices(services => services.AddSingleton<IFileSystem>(FileSystem));
            Host.ConfigureServices(services => services.AddSingleton<ICakeEnvironment>(Environment));
            Host.ConfigureServices(services => services.AddSingleton<IConsole>(Console));
            Host.ConfigureServices(services => services.AddSingleton(Log));
            Host.ConfigureServices(services => services.AddSingleton(Installer));

            if (Strategy != null)
            {
                Host.ConfigureServices(services => services.AddSingleton(Strategy));
            }

            return Host.Run(args);
        }
    }
}
