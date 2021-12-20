// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Cake.Common.Build.GitHubActions.Commands
{
    internal sealed class Values<T>
    {
        [JsonPropertyName("count")]
        public long Count { get; set; }

        [JsonPropertyName("value")]
        public T[] Value { get; set; }
    }
}
