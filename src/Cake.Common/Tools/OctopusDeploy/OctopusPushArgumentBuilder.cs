// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    internal class OctopusPushArgumentBuilder : OctopusDeployArgumentBuilder<OctopusPushSettings>
    {
        private readonly List<FilePath> _packagePaths;

        public OctopusPushArgumentBuilder(IEnumerable<FilePath> packagePaths, string server, string apiKey, ICakeEnvironment environment, OctopusPushSettings settings) : base(server, apiKey, environment, settings)
        {
            _packagePaths = packagePaths as List<FilePath> ?? packagePaths.ToList();
        }

        public ProcessArgumentBuilder Get()
        {
            AppendPackageArguments();
            AppendCommonArguments();
            return Builder;
        }

        private void AppendPackageArguments()
        {
            Builder.Append("push");

            foreach (var packagePath in _packagePaths)
            {
                Builder.AppendSwitchQuoted("--package", packagePath.MakeAbsolute(Environment).FullPath);
            }

            if (Settings.ReplaceExisting)
            {
                Builder.Append("--replace-existing");
            }
        }
    }
}