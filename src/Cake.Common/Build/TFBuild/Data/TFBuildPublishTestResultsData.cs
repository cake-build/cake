﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.TFBuild.Data
{
    /// <summary>
    ///  Provides TF Build agent info for publishing test results
    /// </summary>
    public class TFBuildPublishTestResultsData
    {
        /// <summary>
        /// Gets or Sets the type test runner the results are formated in
        /// </summary>
        public TFTestRunnerType? TestRunner { get; set; }

        /// <summary>
        /// Gets or sets the list of Test Result files to publish.
        /// </summary>
        public string[] TestResultsFiles { get; set; }

        /// <summary>
        /// Gets or Sets whether to merge all Test Result Files into one run
        /// </summary>
        public bool? MergeTestResults { get; set; }

        /// <summary>
        /// Gets or Sets the Platform for which the tests were run on
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or Sets the configuration for which the tests were run on
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or Sets a name for the Test Run.
        /// </summary>
        public string TestRunTitle { get; set; }

        /// <summary>
        /// Gets or Sets whether to opt in/out of publishing test run level attachments
        /// </summary>
        public bool? PublishRunAttachments { get; set; }

        internal Dictionary<string, string> GetProperties(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            var properties = new Dictionary<string, string>();

            if (TestRunner.HasValue)
            {
                properties.Add("type", TestRunner.Value.ToString());
            }
            if (MergeTestResults.HasValue)
            {
                properties.Add("mergeResults", MergeTestResults.ToString().ToLowerInvariant());
            }
            if (!string.IsNullOrWhiteSpace(Platform))
            {
                properties.Add("platform", Platform);
            }
            if (!string.IsNullOrWhiteSpace(Configuration))
            {
                properties.Add("config", Configuration);
            }
            if (!string.IsNullOrWhiteSpace(TestRunTitle))
            {
                properties.Add("runTitle", $"'{TestRunTitle}'");
            }
            if (PublishRunAttachments.HasValue)
            {
                properties.Add("publishRunAttachments", PublishRunAttachments.ToString().ToLowerInvariant());
            }
            if (TestResultsFiles != null && TestResultsFiles.Any())
            {
                properties.Add("resultFiles", string.Join(",", TestResultsFiles.Select(filePath => new FilePath(filePath).MakeAbsolute(environment).FullPath.Replace("/", "\\"))));
            }
            return properties;
        }
    }
}
