// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.VSWhere;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tests.Fixtures.Tools.VSWhere
{
    using System.Linq;
    using Cake.Core;

    internal class VSWhereToolFixture : VSWhereFixture<ToolSettings>
    {
        internal VSWhereToolFixture(bool is64BitOperativeSystem)
            : base(is64BitOperativeSystem)
        {
        }

        protected override void RunTool()
        {
            var tool = new VSWhereTool(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Settings);
        }

        private sealed class VSWhereTool : VSWhereTool<ToolSettings>
        {
            public VSWhereTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator toolLocator)
                : base(fileSystem, environment, processRunner, toolLocator)
            {
            }
            public DirectoryPath Run(ToolSettings settings)
            {
                return RunVSWhere(settings, new ProcessArgumentBuilder()).FirstOrDefault();
            }
        }
    }
}
