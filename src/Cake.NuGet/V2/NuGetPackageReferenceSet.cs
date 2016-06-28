// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Cake.Core.IO;

namespace Cake.NuGet.V2
{
    internal sealed class NuGetPackageReferenceSet : global::NuGet.IFrameworkTargetable
    {
        private readonly FilePath[] _references;
        private readonly FrameworkName _targetFramework;

        public NuGetPackageReferenceSet(FrameworkName targetFramework, IEnumerable<FilePath> references)
        {
            if (references == null)
            {
                throw new ArgumentNullException("references");
            }

            _targetFramework = targetFramework;
            _references = references.ToArray();
        }

        public FilePath[] References
        {
            get { return _references; }
        }

        public FrameworkName TargetFramework
        {
            get { return _targetFramework; }
        }

        public IEnumerable<FrameworkName> SupportedFrameworks
        {
            get
            {
                if (_targetFramework == null)
                {
                    return Enumerable.Empty<FrameworkName>();
                }
                return new[] { _targetFramework };
            }
        }
    }
}
#endif