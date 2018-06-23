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
    /// Providers TF Build agent for publishing code coverage results
    /// </summary>
    public class TFBuildPublishCodeCoverageData
    {
        /// <summary>
        /// Gets or Sets the tool from which code coverage results are generated.
        /// </summary>
        public TFCodeCoverageToolType? CodeCoverageTool { get; set; }

        /// <summary>
        /// Gets or Sets the path ath of the summary file containing code coverage statistics, such as line, method, and class coverage.
        /// </summary>
        public string SummaryFileLocation { get; set; }

        /// <summary>
        /// Gets or Sets the Path of the code coverage HTML report directory. The report directory is published for later viewing as an artifact of the build.
        /// </summary>
        public string ReportDirectory { get; set; }

        /// <summary>
        /// Gets or Sets the file paths for any additional code coverage files to be published as artifacts of the build.
        /// </summary>
        public string[] AdditionalCodeCoverageFiles { get; set; }

        internal Dictionary<string, string> GetProperties(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            var properties = new Dictionary<string, string>();

            if (CodeCoverageTool.HasValue)
            {
                properties.Add("codecoveragetool", CodeCoverageTool.Value.ToString());
            }
            if (!string.IsNullOrWhiteSpace(SummaryFileLocation))
            {
                properties.Add("summaryfile", new FilePath(SummaryFileLocation).MakeAbsolute(environment).FullPath.Replace("/", "\\"));
            }
            if (!string.IsNullOrWhiteSpace(ReportDirectory))
            {
                properties.Add("reportdirectory", new DirectoryPath(ReportDirectory).MakeAbsolute(environment).FullPath.Replace("/", "\\"));
            }
            if (AdditionalCodeCoverageFiles != null && AdditionalCodeCoverageFiles.Any())
            {
                properties.Add("additionalcodecoveragefiles", string.Join(",", AdditionalCodeCoverageFiles.Select(filePath => new FilePath(filePath).MakeAbsolute(environment).FullPath.Replace("/", "\\"))));
            }
            return properties;
        }
    }
}
