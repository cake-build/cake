// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Cake.Common.Build.GoCD.Data;

/// <summary>
/// The Go.CD pipeline history.
/// </summary>
public class GoCDPipelineHistoryInfo
{
    /// <summary>
    /// Gets or sets the build cause.
    /// </summary>
    /// <value>
    /// The build cause.
    /// </value>
    [JsonPropertyName("build_cause")]
    public GoCDBuildCauseInfo BuildCause { get; set; }

    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    /// <value>
    /// The comment.
    /// </value>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the natural order.
    /// </summary>
    /// <value>
    /// The natural order.
    /// </value>
    [JsonPropertyName("natural_order")]
    [JsonConverter(typeof(GoCDStringOrNumberConverter))]
    public string NaturalOrder { get; set; }
}
