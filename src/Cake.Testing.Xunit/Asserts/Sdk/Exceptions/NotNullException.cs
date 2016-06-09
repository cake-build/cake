// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when an object is unexpectedly null.
    /// </summary>
    public class NotNullException : XunitException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="NotNullException"/> class.
        /// </summary>
        public NotNullException()
            : base("Assert.NotNull() Failure")
        { }
    }
}
