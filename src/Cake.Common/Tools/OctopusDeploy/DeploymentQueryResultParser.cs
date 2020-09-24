// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Parses the Console Output of the Octo.exe call when called with list-deployments.
    /// </summary>
    public class DeploymentQueryResultParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeploymentQueryResultParser"/> class.
        /// </summary>
        public DeploymentQueryResultParser()
        {
        }

        /// <summary>
        /// Parse the results from The Deployment Query.
        /// </summary>
        /// <param name="output">Console Output from the Run Process.</param>
        /// <returns>A collection of Octopus deployments.</returns>
        public IEnumerable<OctopusDeployment> ParseResults(IEnumerable<string> output)
        {
            var results = new List<OctopusDeployment>();
            if (output == null)
            {
                return results;
            }
            var l = new List<string>(output);
            if (l.Count < 10)
            {
                return results;
            }
            /* yes, this is fragile and I'm open to better ideas,
             * however, i can't ask the tool to format as something serialized
             * dump the status lines */
            l.RemoveRange(0, 10);
            while (l.Count > 0)
            {
                var nibSize = l.Count >= 9 ? 9 : l.Count;
                var interesting = l.GetRange(0, nibSize);
                var deployment = ParseSet(interesting);
                if (deployment != null)
                {
                    results.Add(deployment);
                }
                l.RemoveRange(0, nibSize);
            }
            return results;
        }

        /// <summary>
        /// Parses a set of lines from the output.
        /// </summary>
        /// <param name="lineSet">A set of lines to parse.</param>
        /// <returns>an OctopusDeployment or null.</returns>
        protected virtual OctopusDeployment ParseSet(List<string> lineSet)
        {
            // the ninth line is blank, so 8 is technically ok, but w/e
            if (lineSet.Count < 8)
            {
                return null;
            }
            var d = new OctopusDeployment()
            {
                ProjectName = getValueFromLine("Project", lineSet[0]),
                Environment = getValueFromLine("Environment", lineSet[1]),
                Channel = getValueFromLine("Channel", lineSet[2]),
                Created = DateTimeOffset.Parse(getValueFromLine("Created", lineSet[3])),
                Version = getValueFromLine("Version", lineSet[4]),
                Assembled = DateTimeOffset.Parse(getValueFromLine("Assembled", lineSet[5])),
                PackageVersions = getValueFromLine("Package Version", lineSet[6]),
                ReleaseNotesHtml = getValueFromLine("Release Notes", lineSet[7])
            };
            return d;
        }

        private string getValueFromLine(string propName, string line)
        {
            /* this could probably accept a string[] lines instead, and hunt
             * but direct index is faster and can still validate. */
            if (!line.Contains(propName))
            {
                throw new Exception(string.Format("Could not parse Deployment - Expected: '{0}', was '{1}'", propName, line));
            }
            return line.Split(new[] { ':' }, 2)[1].Trim();
        }
    }
}