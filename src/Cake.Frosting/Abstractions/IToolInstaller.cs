// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Packaging;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting
{
    /// <summary>
    /// Represents the tool installer.
    /// </summary>
    public interface IToolInstaller
    {
        /// <summary>
        /// Tries to install the specified <see cref="PackageReference"/> using
        /// the most suitable <see cref="IPackageInstaller"/>.
        /// </summary>
        /// <param name="tool">Tool to install.</param>
        void Install(PackageReference tool);
    }
}
