using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Cake.Core;
using Cake.Core.IO;
using Microsoft.VisualStudio.SolutionPersistence.Serializer;
using Path = System.IO.Path;

namespace Cake.Common.Solution;

/// <summary>
/// The slnx file parser.
/// </summary>
public class SlnxParser
{
    /// <summary>
    /// Parses Slnx file.
    /// </summary>
    /// <param name="solutionPath">The solution path.</param>
    /// <param name="fileSystem">The file system.</param>
    /// <returns>Parsed slnx.</returns>
    public static SolutionParserResult Parse(FilePath solutionPath, IFileSystem fileSystem)
    {
        var file = fileSystem.GetFile(solutionPath);
        if (!file.Exists)
        {
            var message = string.Format(
                CultureInfo.InvariantCulture,
                "Solution file '{0}' does not exist.",
                solutionPath.FullPath);
            throw new CakeException(message);
        }

        var serializer = SolutionSerializers.GetSerializerByMoniker(".slnx");
        if (serializer == null)
        {
            throw new CakeException("No serializer available for .slnx format.");
        }

        using var stream = file.OpenRead();
        var task = serializer.OpenAsync(solutionPath.FullPath, CancellationToken.None);
        task.Wait();

        var solution = task.Result;

        var projects = solution?.SolutionProjects?
            .Select(p => new SolutionProject(
                Guid.NewGuid().ToString(),
                Path.GetFileNameWithoutExtension(p.FilePath),
                new FilePath(p.FilePath),
                "slnx"))
            .ToList();

        return new SolutionParserResult(
            version: "slnx",
            visualStudioVersion: "17.0",
            minimumVisualStudioVersion: "17.0",
            projects: projects.AsReadOnly());
    }
}