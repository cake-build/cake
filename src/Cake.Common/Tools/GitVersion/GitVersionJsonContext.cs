// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// Source-generated JSON serializer context for GitVersion types.
    /// Uses static options that include <see cref="JsonStringOrNumberConverter"/> so CLI number/string values deserialize to string properties.
    /// </summary>
    [JsonSerializable(typeof(GitVersionInternal))]
    internal partial class GitVersionJsonContext : JsonSerializerContext
    {
        /// <summary>
        /// Gets the static serializer options used for GitVersion JSON (includes <see cref="JsonStringOrNumberConverter"/>).
        /// </summary>
        public static JsonSerializerOptions SerializerOptions { get; } = new ()
        {
            Converters = { new JsonStringOrNumberConverter() }
        };

        /// <summary>
        /// Gets the context instance with options that allow number-or-string for string properties (uses <see cref="SerializerOptions"/>).
        /// Use this instead of <see cref="Default"/> when deserializing GitVersion CLI output.
        /// </summary>
        public static GitVersionJsonContext DefaultWithConverter { get; } = new (SerializerOptions);
    }
}
