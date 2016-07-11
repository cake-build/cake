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
    /// Provides Continua CI build information for a current build.
    /// </summary>
    public sealed class ContinuaCIBuildInfo : ContinuaCIInfo
    {
        private readonly string _prefix;

        private readonly ContinuaCIChangesetInfo _latestChangesetInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuaCIBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="prefix">The prefix for environment variables in this clas</param>
        public ContinuaCIBuildInfo(ICakeEnvironment environment, string prefix)
            : base(environment)
        {
            _latestChangesetInfo = new ContinuaCIChangesetInfo(environment, string.Format(CultureInfo.InvariantCulture, "{0}.LatestChangeset", prefix));
            _prefix = prefix;
        }

        /// <summary>
        /// Gets the build id.
        /// </summary>
        public int Id
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Id", _prefix);
                return GetEnvironmentInteger(key);
            }
        }

        /// <summary>
        /// Gets the build version.
        /// </summary>
        public string Version
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Version", _prefix);
                return GetEnvironmentString(key);
            }
        }

        /// <summary>
        /// Gets the name of the user or trigger starting the build.
        /// </summary>
        public string StartedBy
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.StartedBy", _prefix);
                return GetEnvironmentString(key);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the build uses the feature branch.
        /// </summary>
        public bool IsFeatureBranchBuild
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.IsFeatureBranchBuild", _prefix);
                return GetEnvironmentBoolean(key);
            }
        }

        /// <summary>
        /// Gets the build number.
        /// </summary>
        public int BuildNumber
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.BuildNumber", _prefix);
                return GetEnvironmentInteger(key);
            }
        }

        /// <summary>
        /// Gets the build start date and time.
        /// </summary>
        public DateTime? Started
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Started", _prefix);
                return GetEnvironmentDateTime(key);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the build uses the default branch.
        /// </summary>
        public bool UsesDefaultBranch
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.UsesDefaultBranch", _prefix);
                return GetEnvironmentBoolean(key);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the build has new changes.
        /// </summary>
        public bool HasNewChanges
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.HasNewChanges", _prefix);
                return GetEnvironmentBoolean(key);
            }
        }

        /// <summary>
        /// Gets build the number of changesets associated with this build
        /// </summary>
        public int ChangesetCount
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.ChangesetCount", _prefix);
                return GetEnvironmentInteger(key);
            }
        }

        /// <summary>
        /// Gets build the number of issues associated with this build
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
        /// Gets build elapsed time on queue as a time span.
        /// </summary>
        public TimeSpan? Elapsed
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Elapsed", _prefix);
                return GetEnvironmentTimeSpan(key);
            }
        }

        /// <summary>
        /// Gets build time on queue in ticks.
        /// </summary>
        public long TimeOnQueue
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.TimeOnQueue", _prefix);
                return GetEnvironmentLong(key);
            }
        }

        /// <summary>
        /// Gets list of repository names
        /// </summary>
        public IEnumerable<string> Repositories
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Repositories", _prefix);
                return GetEnvironmentStringList(key);
            }
        }

        /// <summary>
        /// Gets list of repository branch names
        /// </summary>
        public IEnumerable<string> RepositoryBranches
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.RepositoryBranches", _prefix);
                return GetEnvironmentStringList(key);
            }
        }

        /// <summary>
        /// Gets triggering branch name
        /// </summary>
        public string TriggeringBranch
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.TriggeringBranch", _prefix);
                return GetEnvironmentString(key);
            }
        }

        /// <summary>
        /// Gets list of changeset revisions
        /// </summary>
        public IEnumerable<string> ChangesetRevisions
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.ChangesetRevisions", _prefix);
                return GetEnvironmentStringList(key);
            }
        }

        /// <summary>
        /// Gets list of changeset user names
        /// </summary>
        public IEnumerable<string> ChangesetUserNames
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.ChangesetUserNames", _prefix);
                return GetEnvironmentStringList(key);
            }
        }

        /// <summary>
        /// Gets list of changeset tag names
        /// </summary>
        public IEnumerable<string> ChangesetTagNames
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.ChangesetTagNames", _prefix);
                return GetEnvironmentStringList(key);
            }
        }

        /// <summary>
        /// Gets the latest build changeset
        /// </summary>
        public ContinuaCIChangesetInfo LatestChangeset
        {
            get { return _latestChangesetInfo; }
        }
    }
}
