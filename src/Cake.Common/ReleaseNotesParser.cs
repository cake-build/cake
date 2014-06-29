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

        public ReleaseNotes Parse(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            var lines = GetLines(content);
            if (lines.Length > 0)
            {
                if (lines[0].StartsWith("#", StringComparison.OrdinalIgnoreCase))
                {
                    return ParseComplexFormat(lines);
                }
            }
            throw new CakeException("Unknown release notes format.");
        }

        private ReleaseNotes ParseComplexFormat(string[] lines)
        {
            var lineIndex = 0;
            var result = new Dictionary<Version, List<string>>();

            while (true)
            {
                if (lineIndex >= lines.Length)
                {
                    break;
                }

                // Parse header.
                var versionResult = _versionRegex.Match(lines[lineIndex]);
                if(!versionResult.Success)
                {
                    throw new CakeException("Could not parse version from release notes header.");
                }

                // Create release notes.
                var version = Version.Parse(versionResult.Value);
                result.Add(version, new List<string>());

                // Increase the line index.
                lineIndex++;

                // Parse content.
                while (true)
                {
                    // Sanity checks.
                    if (lineIndex >= lines.Length)
                    {
                        break;
                    }
                    if(lines[lineIndex].StartsWith("#", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    // Get the current line.
                    var line = (lines[lineIndex] ?? string.Empty).Trim(new[] { '*' }).Trim();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        result[version].Add(line);
                    }

                    lineIndex++;
                }
            }

            // Get the highest key.
            var highestKey = result.Keys.OrderByDescending(v => v).First();
            if (highestKey == null)
            {
                throw new CakeException("Could not parse release notes.");
            }

            // Return the parsed release notes.
            var notes = result[highestKey];
            return new ReleaseNotes(highestKey.ToString(), notes);
        }

        private static string[] GetLines(string content)
        {
            content = content.NormalizeLineEndings();
            var lines = content.Split(new[] { "\r\n" }, StringSplitOptions.None);
            return lines.Select(l => l.Trim()).ToArray();
        }
    }
}
