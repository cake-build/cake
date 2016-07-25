using System;
using Cake.Core.Packaging;

namespace Cake.Core.Factories
{
    /// <summary>
    /// Represents a factory for cake components.
    /// </summary>
    public interface ICakeComponentFactory
    {
        /// <summary>
        /// Creates a component of type <see cref="PackageReference"/>.
        /// </summary>
        /// <param name="uri">The uri</param>
        /// <returns>A <see cref="PackageReference"/></returns>
        PackageReference CreatePackageReference(Uri uri);
    }
}