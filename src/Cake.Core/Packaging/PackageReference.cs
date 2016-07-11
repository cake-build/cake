// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cake.Core.Packaging
{
    /// <summary>
    /// Represents an URI resource.
    /// </summary>
    public sealed class PackageReference
    {
        private readonly string _originalString;
        private readonly string _scheme;
        private readonly Uri _address;
        private readonly IReadOnlyDictionary<string, string> _parameters;
        private readonly string _package;

        /// <summary>
        /// Gets the original string.
        /// </summary>
        /// <value>The original string.</value>
        public string OriginalString
        {
            get { return _originalString; }
        }

        /// <summary>
        /// Gets the scheme.
        /// </summary>
        /// <value>The scheme.</value>
        public string Scheme
        {
            get { return _scheme; }
        }

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public Uri Address
        {
            get { return _address; }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public IReadOnlyDictionary<string, string> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Gets the package.
        /// </summary>
        /// <value>The package.</value>
        public string Package
        {
            get { return _package; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageReference"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public PackageReference(string uri)
            : this(new Uri(uri))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageReference"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <exception cref="System.ArgumentException">Package query string parameter is missing.;uri</exception>
        internal PackageReference(Uri uri)
        {
            _originalString = uri.OriginalString;
            _scheme = uri.Scheme;
            _parameters = new ReadOnlyDictionary<string, string>(uri.GetQueryString());

            _package = _parameters.ContainsKey("package") ? _parameters["package"] : null;
            if (_package == null)
            {
                throw new ArgumentException(
                    "Query string parameter 'package' is missing in package reference.",
                    "uri");
            }

            Uri address;
            if (Uri.TryCreate(uri.AbsolutePath, UriKind.Absolute, out address))
            {
                _address = new Uri(address.AbsoluteUri);
            }
        }
    }
}
