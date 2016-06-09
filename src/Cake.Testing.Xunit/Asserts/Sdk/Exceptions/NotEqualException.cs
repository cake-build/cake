// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when two values are unexpectedly equal.
    /// </summary>
    public class NotEqualException : AssertActualExpectedException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="NotEqualException"/> class.
        /// </summary>
        public NotEqualException(string expected, string actual)
            : base("Not " + expected, actual, "Assert.NotEqual() Failure")
        { }
    }
}
