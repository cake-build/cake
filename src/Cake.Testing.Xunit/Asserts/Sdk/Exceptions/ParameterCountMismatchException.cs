// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Xunit.Sdk
{
    /// <summary>
    /// Exception to be thrown from theory execution when the number of
    /// parameter values does not the test method signature.
    /// </summary>
    public class ParameterCountMismatchException : Exception { }
}
