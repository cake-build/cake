using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.Extensions;

namespace Cake.Common
{
    public sealed class ReleaseNotesParser
    {
        private readonly Regex _versionRegex;

        public ReleaseNotesParser()
        {
            _versionRegex = new Regex(@"([0-9]+\.)+[0-9]+");
        }

        public IReadOnlyList<ReleaseNotes> Parse(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            var lines = content.SplitLines();
            if (lines.Length > 0)
            {
                var line = lines[0].Trim();

                if (line.StartsWith("#", StringComparison.OrdinalIgnoreCase))
                {
                    return ParseComplexFormat(lines);
                }

                if (line.StartsWith("*", StringComparison.OrdinalIgnoreCase))
                {
                    return ParseSimpleFormat(lines);
                }
            }

            throw new CakeException("Unknown release notes format.");
        }

        private IReadOnlyList<ReleaseNotes> ParseComplexFormat(string[] lines)
        {
            var lineIndex = 0;
            var result = new List<ReleaseNotes>();

            while (true)
            {
                if (lineIndex >= lines.Length)
                {
                    break;
                }

                // Parse header.
                var versionResult = _versionRegex.Match(lines[lineIndex]);
                if (!versionResult.Success)
                {
                    throw new CakeException("Could not parse version from release notes header.");
                }

                // Create release notes.
                var version = Version.Parse(versionResult.Value);

                // Increase the line index.
                lineIndex++;

                // Parse content.
                var notes = new List<string>();
                while (true)
                {
                    // Sanity checks.
                    if (lineIndex >= lines.Length)
                    {
                        break;
                    }
                    if (lines[lineIndex].StartsWith("#", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    // Get the current line.
                    var line = (lines[lineIndex] ?? string.Empty).Trim(new[] { '*' }).Trim();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        notes.Add(line);
                    }

                    lineIndex++;
                }

                result.Add(new ReleaseNotes(version, notes));
            }

            return result.OrderByDescending(x => x.Version).ToArray();
        }

        private IReadOnlyList<ReleaseNotes> ParseSimpleFormat(string[] lines)
        {
            var lineIndex = 0;
            var result = new List<ReleaseNotes>();

            while (true)
            {
                if (lineIndex >= lines.Length)
                {
                    break;
                }

                // Trim the current line.
                var line = (lines[lineIndex] ?? string.Empty).Trim(new[] { '*', ' ' });
                if (string.IsNullOrWhiteSpace(line))
                {
                    lineIndex++;
                    continue;
                }
                
                // Parse header.
                var versionResult = _versionRegex.Match(line);
                if (!versionResult.Success)
                {
                    throw new CakeException("Could not parse version from release notes header.");
                }

                var version = Version.Parse(versionResult.Value);

                // Parse the description.
                line = line.Substring(versionResult.Length).Trim(new[] { '-', ' ' });

                // Add the release notes to the result.
                result.Add(new ReleaseNotes(version, new[] { line }));

                lineIndex++;
            }

            return result.OrderByDescending(x => x.Version).ToArray();
        }
    }
}
