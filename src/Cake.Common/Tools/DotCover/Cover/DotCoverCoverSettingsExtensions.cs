// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotCover.Cover
{
    /// <summary>
    /// Contains extensions for <see cref="DotCoverCoverSettings"/>.
    /// </summary>
    public static class DotCoverCoverSettingsExtensions
    {
        /// <summary>
        /// Sets the JSON report output path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="outputPath">The JSON report output path.</param>
        /// <returns>The same <see cref="DotCoverCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverCoverSettings WithJsonReportOutput(this DotCoverCoverSettings settings, FilePath outputPath)
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.JsonReportOutput = outputPath;
            return settings;
        }

        /// <summary>
        /// Sets the JSON report covering tests scope.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="scope">The granularity for including covering tests in JSON reports.</param>
        /// <returns>The same <see cref="DotCoverCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverCoverSettings WithJsonReportCoveringTestsScope(this DotCoverCoverSettings settings, DotCoverReportScope scope)
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.JsonReportCoveringTestsScope = scope;
            return settings;
        }

        /// <summary>
        /// Sets the XML report output path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="outputPath">The XML report output path.</param>
        /// <returns>The same <see cref="DotCoverCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverCoverSettings WithXmlReportOutput(this DotCoverCoverSettings settings, FilePath outputPath)
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.XmlReportOutput = outputPath;
            return settings;
        }

        /// <summary>
        /// Sets the XML report covering tests scope.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="scope">The granularity for including covering tests in XML reports.</param>
        /// <returns>The same <see cref="DotCoverCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverCoverSettings WithXmlReportCoveringTestsScope(this DotCoverCoverSettings settings, DotCoverReportScope scope)
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.XmlReportCoveringTestsScope = scope;
            return settings;
        }

        /// <summary>
        /// Sets the temporary directory for files.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="directory">The temporary directory path.</param>
        /// <returns>The same <see cref="DotCoverCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverCoverSettings WithTemporaryDirectory(this DotCoverCoverSettings settings, DirectoryPath directory)
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.TemporaryDirectory = directory;
            return settings;
        }

        /// <summary>
        /// Enables control of the coverage session using the profiler API.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="useApi">Whether to use the API.</param>
        /// <returns>The same <see cref="DotCoverCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverCoverSettings WithUseApi(this DotCoverCoverSettings settings, bool useApi = true)
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.UseApi = useApi;
            return settings;
        }

        /// <summary>
        /// Disables loading of NGen images during coverage.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="noNGen">Whether to disable NGen.</param>
        /// <returns>The same <see cref="DotCoverCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverCoverSettings WithNoNGen(this DotCoverCoverSettings settings, bool noNGen = true)
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.NoNGen = noNGen;
            return settings;
        }

        /// <summary>
        /// Configures whether to use legacy command syntax.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="useLegacySyntax">Whether to use legacy syntax. Default is false (new syntax).</param>
        /// <returns>The same <see cref="DotCoverCoverSettings"/> instance so that multiple calls can be chained.</returns>
        public static DotCoverCoverSettings WithLegacySyntax(this DotCoverCoverSettings settings, bool useLegacySyntax = true)
        {
            ArgumentNullException.ThrowIfNull(settings);
            settings.UseLegacySyntax = useLegacySyntax;
            return settings;
        }
    }
}