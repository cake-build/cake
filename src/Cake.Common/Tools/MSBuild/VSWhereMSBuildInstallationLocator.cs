// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.VSWhere.Latest;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSBuild;

/// <summary>
/// Locates Visual Studio installations containing MSBuild using vswhere.
/// </summary>
internal sealed class VSWhereMSBuildInstallationLocator : IMSBuildInstallationLocator
{
    private readonly VSWhereLatest _vsWhere;

    /// <summary>
    /// Initializes a new instance of the <see cref="VSWhereMSBuildInstallationLocator"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="toolLocator">The tool locator.</param>
    public VSWhereMSBuildInstallationLocator(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator toolLocator)
    {
        _vsWhere = new VSWhereLatest(fileSystem, environment, processRunner, toolLocator);
    }

    /// <inheritdoc/>
    public DirectoryPath FindInstallation(string versionRange, bool includePrerelease)
    {
        var settings = new VSWhereLatestSettings
        {
            // Build Tools are only included when searching all products.
            Products = "*",
            Requires = "Microsoft.Component.MSBuild",
            Version = versionRange,
            IncludePrerelease = includePrerelease,
        };

        try
        {
            return _vsWhere.Latest(settings);
        }
        catch (CakeException)
        {
            // vswhere isn't available or failed, so fall back to the default resolution.
            return null;
        }
    }
}
