// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting.Tests.Fakes;
using Cake.Testing;
using NSubstitute;

namespace Cake.Frosting.Tests.Fixtures
{
    public class CakeHostBuilderFixture
    {
        public CakeHostBuilder Builder { get; set; }

        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeEngine Engine { get; set; }
        public ICakeLog Log { get; set; }
        public ICakeDataService Data { get; set; }
        public IExecutionStrategy Strategy { get; set; }
        public IToolInstaller Installer { get; set; }
        public CakeHostOptions Options { get; set; }

        public CakeHostBuilderFixture()
        {
            Builder = new CakeHostBuilder();
            Environment = FakeEnvironment.CreateUnixEnvironment();

            FileSystem = new FakeFileSystem(Environment);
            FileSystem.CreateDirectory("/Working");

            Log = Substitute.For<ICakeLog>();
            Data = Substitute.For<ICakeDataService>();
            Engine = new CakeEngine(Data, Log);
            Installer = Substitute.For<IToolInstaller>();
            Options = new CakeHostOptions();
        }

        public ICakeHost Build()
        {
            // Replace registrations with more suitable ones.
            Builder.ConfigureServices(s => s.RegisterType<NullConsole>().As<IConsole>());
            Builder.ConfigureServices(s => s.RegisterInstance(Environment).As<ICakeEnvironment>());
            Builder.ConfigureServices(s => s.RegisterInstance(FileSystem).As<IFileSystem>());
            Builder.ConfigureServices(s => s.RegisterInstance(Engine).As<ICakeEngine>());
            Builder.ConfigureServices(s => s.RegisterInstance(Log).As<ICakeLog>());
            Builder.ConfigureServices(s => s.RegisterInstance(Installer).As<IToolInstaller>());
            Builder.ConfigureServices(s => s.RegisterInstance(Options).As<CakeHostOptions>());

            if (Strategy != null)
            {
                Builder.ConfigureServices(services => services.RegisterInstance(Strategy).As<IExecutionStrategy>());
            }

            Builder.ConfigureServices(s => s.RegisterInstance(Options).AsSelf().Singleton());

            return Builder.Build();
        }

        public int Run()
        {
            return Build().Run();
        }
    }
}
