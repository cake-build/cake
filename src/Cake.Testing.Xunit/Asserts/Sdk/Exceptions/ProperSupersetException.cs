// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections;

namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when a set is not a proper superset of another set.
    /// </summary>
    public class ProperSupersetException : AssertActualExpectedException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ProperSupersetException"/> class.
        /// </summary>
        public ProperSupersetException(IEnumerable expected, IEnumerable actual)
            : base(expected, actual, "Assert.ProperSuperset() Failure")
        { }
    }
}
