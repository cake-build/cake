// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.Json.Serialization;

namespace Cake.Common.Build.GoCD.Data;

/// <summary>
/// A change made in the repository since the last time the Go.CD pipeline was run.
/// </summary>
public class GoCDModificationInfo
{
    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    /// <value>
    /// The email address.
    /// </value>
    [JsonPropertyName("email_address")]
    public string EmailAddress { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the modified time in milliseconds from the Unix epoch.
    /// </summary>
    /// <value>
    /// The modified time in milliseconds from the Unix epoch.
    /// </value>
    [JsonPropertyName("modified_time")]
    public long ModifiedTimeUnixMilliseconds { get; set; }

    /// <summary>
    /// Gets or sets the modified time.
    /// </summary>
    /// <value>
    /// The modified time.
    /// </value>
    [JsonIgnore]
    public DateTime ModifiedTime
    {
        get { return DateTimeOffset.FromUnixTimeMilliseconds(ModifiedTimeUnixMilliseconds).UtcDateTime; }
        set
        {
            var dateTime = value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
                : value;
            ModifiedTimeUnixMilliseconds = new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }
    }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>
    /// The username.
    /// </value>
    [JsonPropertyName("user_name")]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    /// <value>
    /// The comment.
    /// </value>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    /// <summary>
    /// Gets or sets the revision.
    /// </summary>
    /// <value>
    /// The revision.
    /// </value>
    [JsonPropertyName("revision")]
    public string Revision { get; set; }
}
