// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Globalization;

namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when the collection did not contain exactly the given number element.
    /// </summary>
    public class AssertCollectionCountException : XunitException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleException"/> class.
        /// </summary>
        /// <param name="expectedCount">The expected number of items in the collection.</param>
        /// <param name="actualCount">The actual number of items in the collection.</param>
        public AssertCollectionCountException(int expectedCount, int actualCount)
            : base(string.Format(CultureInfo.CurrentCulture, "The collection contained {0} matching element(s) instead of {1}.", actualCount, expectedCount))
        { }
    }
}
