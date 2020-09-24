// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Description of code coverage information to publish to Azure Pipelines.
    /// </summary>
    public class AzurePipelinesPublishCodeCoverageData
    {
        /// <summary>
        /// Gets or sets the tool from which code coverage results are generated.
        /// </summary>
        public AzurePipelinesCodeCoverageToolType? CodeCoverageTool { get; set; }

        /// <summary>
        /// Gets or sets the path of the summary file containing code coverage statistics, such as line, method, and class coverage.
        /// </summary>
        public FilePath SummaryFileLocation { get; set; }

        /// <summary>
        /// Gets or sets the path of the code coverage HTML report directory. The report directory is published for later viewing as an artifact of the build.
        /// </summary>
        public DirectoryPath ReportDirectory { get; set; }

        /// <summary>
        /// Gets or sets the file paths for any additional code coverage files to be published as artifacts of the build.
        /// </summary>
        public FilePath[] AdditionalCodeCoverageFiles { get; set; }

        internal Dictionary<string, string> GetProperties(ICakeEnvironment environment, FilePath summaryFilePath = null)
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

            if (summaryFilePath != null)
            {
                properties.Add("summaryfile",
                    summaryFilePath
                        .MakeAbsolute(environment)
                        .FullPath
                        .Replace(summaryFilePath.Separator, System.IO.Path.DirectorySeparatorChar));
            }

            if (SummaryFileLocation != null && summaryFilePath == null)
            {
                properties.Add("summaryfile",
                    SummaryFileLocation
                        .MakeAbsolute(environment)
                        .FullPath
                        .Replace(SummaryFileLocation.Separator, System.IO.Path.DirectorySeparatorChar));
            }

            if (ReportDirectory != null)
            {
                properties.Add("reportdirectory",
                    ReportDirectory
                        .MakeAbsolute(environment)
                        .FullPath
                        .Replace(ReportDirectory.Separator, System.IO.Path.DirectorySeparatorChar));
            }

            if (AdditionalCodeCoverageFiles != null && AdditionalCodeCoverageFiles.Any())
            {
                properties.Add("additionalcodecoveragefiles",
                    string.Join(",",
                        AdditionalCodeCoverageFiles
                            .Select(filePath =>
                                filePath
                                    .MakeAbsolute(environment)
                                    .FullPath
                                    .Replace(filePath.Separator, System.IO.Path.DirectorySeparatorChar))));
            }

            return properties;
        }
    }
}
