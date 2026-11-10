// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;
using Cake.Common.Build.GoCD.Data;

namespace Cake.Common.Build.GoCD;

/// <summary>
/// Source-generated JSON serialization context for GoCD pipeline history payloads.
/// </summary>
[JsonSerializable(typeof(GoCDHistoryInfo))]
internal partial class GoCDJsonContext : JsonSerializerContext
{
}
