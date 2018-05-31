// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core
{
    /// <summary>
    /// Represents a data context service.
    /// </summary>
    public interface ICakeDataService : ICakeDataResolver
    {
        /// <summary>
        /// Adds the data of the specified type.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="value">The value of the data.</param>
        void Add<TData>(TData value) where TData : class;
    }
}