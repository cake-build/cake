// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace Cake.Common.Build.GoCD.Data
{
    /// <summary>
    /// A change made in the repository since the last time the Go.CD pipeline was run.
    /// </summary>
    [DataContract]
    public class GoCDModificationInfo
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [DataMember(Name = "email_address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the modified time in milliseconds from the Unix epoch.
        /// </summary>
        /// <value>
        /// The modified time in milliseconds from the Unix epoch.
        /// </value>
        [DataMember(Name = "modified_time")]
        public long ModifiedTimeUnixMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets the modified time.
        /// </summary>
        /// <value>
        /// The modified time.
        /// </value>
        public DateTime ModifiedTime
        {
            get { return FromUnixTimeMilliseconds(ModifiedTimeUnixMilliseconds); }
            set { ModifiedTimeUnixMilliseconds = ToUnixTimeMilliseconds(value); }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [DataMember(Name = "user_name")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the revision.
        /// </summary>
        /// <value>
        /// The revision.
        /// </value>
        [DataMember(Name = "revision")]
        public string Revision { get; set; }

        private static DateTime FromUnixTimeMilliseconds(long milliseconds)
        {
            if ((milliseconds < -62135596800000L) || (milliseconds > 0xe677d21fdbffL))
            {
                throw new ArgumentOutOfRangeException(nameof(milliseconds));
            }

            return new DateTime((milliseconds * 0x2710L) + 0x89f7ff5f7b58000L);
        }

        private static long ToUnixTimeMilliseconds(DateTime dateTime)
        {
            long num = dateTime.Ticks / 0x2710L;
            return num - 0x3883122cd800L;
        }
    }
}
