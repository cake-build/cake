using System;
using Cake.Core.Packaging;

namespace Cake.Core.Factories
{
    /// <summary>
    /// Factory responsble for creating cake components.
    /// </summary>
    public class CakeComponentFactory : ICakeComponentFactory
    {
        /// <summary>
        /// Creates a component of type <see cref="PackageReference"/>.
        /// </summary>
        /// <param name="uri">The uri</param>
        /// <returns>A <see cref="PackageReference"/></returns>
        public PackageReference CreatePackageReference(Uri uri)
        {
            return new PackageReference(uri);
        }
    }
}