// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.SDKCheck
{
    /// <summary>
    /// .NET SDK checker.
    /// </summary>
    public sealed class DotNetSDKChecker : DotNetTool<DotNetSDKCheckSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetSDKChecker" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetSDKChecker(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Lists the latest available version of the .NET SDK and .NET Runtime, for each feature band.
        /// </summary>
        public void Check()
        {
            var settings = new DotNetSDKCheckSettings();
            RunCommand(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(DotNetSDKCheckSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("sdk check");

            return builder;
        }
    }
}
