// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// Allows deserializing JSON number or string values into a <see cref="string"/> property.
    /// GitVersion CLI may output numeric values (e.g. Major: 6) or strings; this converter accepts both.
    /// </summary>
    internal sealed class JsonStringOrNumberConverter : JsonConverter<string>
    {
        /// <inheritdoc />
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.TryGetInt64(out var n) ? n.ToString(CultureInfo.InvariantCulture) : reader.GetDouble().ToString(CultureInfo.InvariantCulture),
                JsonTokenType.Null => null,
                _ => throw new JsonException($"Unexpected token {reader.TokenType} when reading string.")
            };
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value);
            }
        }
    }
}
