// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script alias type.
    /// </summary>
    public enum ScriptAliasType
    {
        /// <summary>
        /// Represents an unknown script alias type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Represents a script alias method.
        /// </summary>
        Method,

        /// <summary>
        /// Represents a script alias property.
        /// </summary>
        Property
    }
}
