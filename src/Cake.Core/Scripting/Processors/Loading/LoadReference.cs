// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Core.Scripting.Processors.Loading
{
    /// <summary>
    /// Represents a resource to load via the #load directive.
    /// </summary>
    public sealed class LoadReference
    {
        /// <summary>
        /// Gets the original string.
        /// </summary>
        /// <value>The original string.</value>
        public string OriginalString { get; }

        /// <summary>
        /// Gets the scheme.
        /// </summary>
        /// <value>The scheme.</value>
        public string Scheme { get; }

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public Uri Address { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public IReadOnlyDictionary<string, IReadOnlyList<string>> Parameters { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadReference"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public LoadReference(Uri uri)
        {
            OriginalString = uri.OriginalString;
            Scheme = uri.Scheme;
            Parameters = uri.GetQueryString();

            if (Uri.TryCreate(uri.AbsolutePath, UriKind.Absolute, out uri))
            {
                Address = new Uri(uri.AbsoluteUri);
            }
        }
    }
}