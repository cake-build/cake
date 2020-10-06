// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Frosting.Internal.Diagnostics.Formatting
{
    internal abstract class FormatToken
    {
        public abstract string Render(object[] args);
    }
}