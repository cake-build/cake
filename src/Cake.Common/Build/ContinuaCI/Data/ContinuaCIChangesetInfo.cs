// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using Cake.Core;

namespace Cake.Common.Build.ContinuaCI.Data
{
    /// <summary>
    /// Provides Continua CI changeset information for a current build.
    /// </summary>
    public sealed class ContinuaCIChangesetInfo : ContinuaCIInfo
    {
        private readonly string _prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuaCIChangesetInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="prefix">The environment variable key prefix.</param>
        public ContinuaCIChangesetInfo(ICakeEnvironment environment, string prefix)
            : base(environment)
        {
            _prefix = prefix;
        }

        /// <summary>
        /// Gets the revision used to build this release. Format depends on the VCS used.
        /// </summary>
        public string Revision
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Revision", _prefix);
                return GetEnvironmentString(key);
            }
        }

        /// <summary>
        /// Gets the changeset branch name
        /// </summary>
        public string Branch
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Branch", _prefix);
                return GetEnvironmentString(key);
            }
        }

        /// <summary>
        /// Gets the changeset created date and time.
        /// </summary>
        public DateTime? Created
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Created", _prefix);
                return GetEnvironmentDateTime(key);
            }
        }

        /// <summary>
        /// Gets the count of the number of files in the changeset.
        /// </summary>
        public int FileCount
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.FileCount", _prefix);
                return GetEnvironmentInteger(key);
            }
        }

        /// <summary>
        /// Gets the changeset author user/committer name
        /// </summary>
        public string UserName
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.UserName", _prefix);
                return GetEnvironmentString(key);
            }
        }

        /// <summary>
        /// Gets the count of the number of tags associated with the changeset.
        /// </summary>
        public int TagCount
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.TagCount", _prefix);
                return GetEnvironmentInteger(key);
            }
        }

        /// <summary>
        /// Gets the count of the number of tags associated with the changeset.
        /// </summary>
        public int IssueCount
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.IssueCount", _prefix);
                return GetEnvironmentInteger(key);
            }
        }

        /// <summary>
        /// Gets list of changeset tag names
        /// </summary>
        public IEnumerable<string> TagNames
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.TagNames", _prefix);
                return GetEnvironmentStringList(key);
            }
        }

        /// <summary>
        /// Gets list of changeset issue names
        /// </summary>
        public IEnumerable<string> IssueNames
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.IssueNames", _prefix);
                return GetEnvironmentStringList(key);
            }
        }
    }
}
