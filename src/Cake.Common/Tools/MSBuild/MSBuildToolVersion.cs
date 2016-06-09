// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Represents a MSBuild tool version.
    /// </summary>
    public enum MSBuildToolVersion
    {
        /// <summary>
        /// The highest available MSBuild tool version.
        /// </summary>
        Default = 0,

        /// <summary>
        /// MSBuild tool version: <c>.NET 2.0</c>
        /// </summary>
        NET20 = 1,

        /// <summary>
        /// MSBuild tool version: <c>.NET 3.0</c>
        /// </summary>
        NET30 = 1,

        /// <summary>
        /// MSBuild tool version: <c>Visual Studio 2005</c>
        /// </summary>
        VS2005 = 1,

        /// <summary>
        /// MSBuild tool version: <c>.NET 3.5</c>
        /// </summary>
        NET35 = 2,

        /// <summary>
        /// MSBuild tool version: <c>Visual Studio 2008</c>
        /// </summary>
        VS2008 = 2,

        /// <summary>
        /// MSBuild tool version: <c>.NET 4.0</c>
        /// </summary>
        NET40 = 3,

        /// <summary>
        /// MSBuild tool version: <c>.NET 4.5</c>
        /// </summary>
        NET45 = 3,

        /// <summary>
        /// MSBuild tool version: <c>Visual Studio 2010</c>
        /// </summary>
        VS2010 = 3,

        /// <summary>
        /// MSBuild tool version: <c>Visual Studio 2011</c>
        /// </summary>
        VS2011 = 3,

        /// <summary>
        /// MSBuild tool version: <c>Visual Studio 2012</c>
        /// </summary>
        VS2012 = 3,

        /// <summary>
        /// MSBuild tool version: <c>.NET 4.5.1</c>
        /// </summary>
        NET451 = 4,

        /// <summary>
        /// MSBuild tool version: <c>.NET 4.5.2</c>
        /// </summary>
        NET452 = 4,

        /// <summary>
        /// MSBuild tool version: <c>Visual Studio 2013</c>
        /// </summary>
        VS2013 = 4,

        /// <summary>
        /// MSBuild tool version: <c>Visual Studio 2015</c>
        /// </summary>
        VS2015 = 5,

        /// <summary>
        /// MSBuild tool version: <c>.NET 4.6</c>
        /// </summary>
        NET46 = 5,
    }
}
