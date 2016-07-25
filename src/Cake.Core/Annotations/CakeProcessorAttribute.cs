// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core.Annotations
{
    /// <summary>
    /// An attribute used to mark a processor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CakeProcessorAttribute : CakeAliasAttribute
    {
    }
}