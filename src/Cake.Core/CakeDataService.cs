// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Implementation of <see cref="ICakeDataService"/>.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CakeDataService : ICakeDataService
    {
        private readonly Dictionary<Type, object> _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeDataService"/> class.
        /// </summary>
        public CakeDataService()
        {
            _data = new Dictionary<Type, object>();
        }

        /// <inheritdoc/>
        public TData Get<TData>()
            where TData : class
        {
            if (_data.TryGetValue(typeof(TData), out var data))
            {
                if (data is TData typedData)
                {
                    return typedData;
                }
                var message = $"Context data exists but is of the wrong type ({data.GetType().FullName}).";
                throw new InvalidOperationException(message);
            }
            throw new InvalidOperationException("The context data has not been setup.");
        }

        /// <inheritdoc/>
        public void Add<TData>(TData value)
            where TData : class
        {
            if (_data.ContainsKey(typeof(TData)))
            {
                var message = $"Context data of type '{typeof(TData).FullName}' has already been registered.";
                throw new InvalidOperationException(message);
            }
            _data.Add(typeof(TData), value);
        }
    }
}