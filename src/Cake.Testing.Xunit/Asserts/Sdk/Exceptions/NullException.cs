// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when an object reference is unexpectedly not null.
    /// </summary>
    public class NullException : AssertActualExpectedException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="NullException"/> class.
        /// </summary>
        /// <param name="actual"></param>
        public NullException(object actual)
            : base(null, actual, "Assert.Null() Failure")
        { }
    }
}
