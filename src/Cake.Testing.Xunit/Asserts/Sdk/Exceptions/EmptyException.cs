// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when a collection is unexpectedly not empty.
    /// </summary>
    public class EmptyException : XunitException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="EmptyException"/> class.
        /// </summary>
        public EmptyException()
            : base("Assert.Empty() Failure")
        { }
    }
}
