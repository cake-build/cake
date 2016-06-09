// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.InspectCode
{
    /// <summary>
    /// Represents Resharper's settings layers.
    /// </summary>
    public enum SettingsLayer
    {
        /// <summary>
        /// SettingsLayer: <c>GlobalAll</c>.
        /// </summary>
        GlobalAll = 1,

        /// <summary>
        /// SettingsLayer: <c>GlobalPerProduct</c>.
        /// </summary>
        GlobalPerProduct = 2,

        /// <summary>
        /// SettingsLayer: <c>SolutionShared</c>. This layer is saved in <c>%SolutionName%.sln.DotSettings</c>
        /// </summary>
        SolutionShared = 3,

        /// <summary>
        /// SettingsLayer: <c>SolutionPersonal</c>. This layer is saved in <c>%SolutionName%.sln.DotSettings.user</c>.
        /// </summary>
        SolutionPersonal = 4,

        /// <summary>
        /// SettingsLayer: <c>ProjectShared</c>. This layer is saved in <c>%ProjectName%.csproj.DotSettings</c>.
        /// </summary>
        ProjectShared = 5,

        /// <summary>
        /// SettingsLayer: <c>ProjectPersonal</c>. This layer is saved in <c>%ProjectName%.csproj.DotSettings.user</c>.
        /// </summary>
        ProjectPersonal = 6
    }
}
