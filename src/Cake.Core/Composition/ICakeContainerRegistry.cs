// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Composition
{
    /// <summary>
    /// Represents a container registry used to register types and instances with Cake.
    /// </summary>
    public interface ICakeContainerRegistry
    {
        /// <summary>
        /// Registers the specified module with the container registry.
        /// </summary>
        /// <param name="module">The module to register.</param>
        void RegisterModule(ICakeModule module);

        /// <summary>
        /// Registers a type with the container registry.
        /// </summary>
        /// <typeparam name="T">The implementation type to register.</typeparam>
        /// <returns>A registration builder used to configure the registration.</returns>
        ICakeRegistrationBuilder<T> RegisterType<T>();

        /// <summary>
        /// Registers an instance with the container registry.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <returns>A registration builder used to configure the registration.</returns>
        ICakeRegistrationBuilder<T> RegisterInstance<T>(T instance);
    }
}
