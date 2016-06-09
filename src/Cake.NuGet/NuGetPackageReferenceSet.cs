// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Cake.Core.IO;

namespace Cake.NuGet
{
    /// <summary>
    /// Represents a targetFramework-specific set of assembly references belonging to a nuget package.
    /// </summary>
    public sealed class NuGetPackageReferenceSet : IFrameworkTargetable
    {
        private readonly IReadOnlyCollection<FilePath> _references;
        private readonly FrameworkName _targetFramework;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageReferenceSet"/> class.
        /// </summary>
        /// <param name="targetFramework">The target framework. May be null.</param>
        /// <param name="references">The references.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="references"/> is null.</exception>
        public NuGetPackageReferenceSet(FrameworkName targetFramework, IEnumerable<FilePath> references)
        {
            if (references == null)
            {
                throw new ArgumentNullException("references");
            }

            _targetFramework = targetFramework;
            _references = references.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <value>
        /// The references.
        /// </value>
        public IReadOnlyCollection<FilePath> References
        {
            get { return _references; }
        }

        /// <summary>
        /// Gets the target framework.
        /// </summary>
        /// <value>
        /// The target framework.
        /// </value>
        public FrameworkName TargetFramework
        {
            get { return _targetFramework; }
        }

        /// <summary>
        /// Gets the supported frameworks.
        /// </summary>
        /// <value>
        /// The supported frameworks.
        /// </value>
        public IEnumerable<FrameworkName> SupportedFrameworks
        {
            get
            {
                if (TargetFramework == null)
                {
                    return Enumerable.Empty<FrameworkName>();
                }
                return new[] { TargetFramework };
            }
        }
    }
}
