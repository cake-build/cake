// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Globalization;

namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when a string unexpectedly matches a regular expression.
    /// </summary>
    public class DoesNotMatchException : XunitException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DoesNotMatchException"/> class.
        /// </summary>
        /// <param name="expectedRegexPattern">The regular expression pattern expected not to match</param>
        /// <param name="actual">The actual value</param>
        public DoesNotMatchException(object expectedRegexPattern, object actual)
            : base(string.Format(CultureInfo.CurrentCulture, "Assert.DoesNotMatch() Failure:{2}Regex: {0}{2}Value: {1}", expectedRegexPattern, actual, Environment.NewLine))
        { }
    }
}
