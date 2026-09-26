static void UpdatePortableRidGraph(ICakeContext context)
{
    var globalJson = context.FileSystem.GetFile("./global.json");
    var graph = context.FileSystem.GetFile("./src/Cake.NuGet/PortableRuntimeIdentifierGraph.json");
    var stamp = context.FileSystem.GetFile("./src/Cake.NuGet/PortableRuntimeIdentifierGraph.stamp");

    var hash = context.CalculateFileHash(globalJson.Path).ToHex();
    var previous = stamp.Exists
        ? stamp.ReadLines(System.Text.Encoding.UTF8).FirstOrDefault()?.Trim()
        : null;

    if (string.Equals(hash, previous, StringComparison.OrdinalIgnoreCase) && graph.Exists)
    {
        context.Information("Portable RID graph matches global.json ({0}).", hash);
        return;
    }

    context.Command(
        ["dotnet", "dotnet.exe"],
        out var sdkVersionOutput,
        new ProcessArgumentBuilder().Append("--version"));
    var sdkVersion = sdkVersionOutput.Trim();

    context.Command(
        ["dotnet", "dotnet.exe"],
        out var listSdksOutput,
        new ProcessArgumentBuilder().Append("--list-sdks"));

    var sdkLine = listSdksOutput
        .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
        .Select(line => line.Trim())
        .FirstOrDefault(line => line.StartsWith(sdkVersion + " ", StringComparison.Ordinal));
    if (string.IsNullOrEmpty(sdkLine))
    {
        throw new CakeException($"Could not locate SDK {sdkVersion} in 'dotnet --list-sdks'.");
    }

    var rootStart = sdkLine.IndexOf('[') + 1;
    var rootEnd = sdkLine.LastIndexOf(']');
    if (rootStart <= 0 || rootEnd <= rootStart)
    {
        throw new CakeException($"Could not parse SDK root from '{sdkLine}'.");
    }

    var sdkRoot = sdkLine.Substring(rootStart, rootEnd - rootStart);
    var source = context.FileSystem.GetFile(
        new DirectoryPath(sdkRoot)
            .Combine(sdkVersion)
            .CombineWithFilePath("PortableRuntimeIdentifierGraph.json"));
    if (!source.Exists)
    {
        throw new CakeException($"SDK {sdkVersion} has no PortableRuntimeIdentifierGraph.json at '{source.Path}'.");
    }

    context.EnsureDirectoryExists(graph.Path.GetDirectory());
    source.Copy(graph.Path, overwrite: true);
    using (var stream = stamp.OpenWrite())
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(hash + "\n");
        stream.Write(bytes, 0, bytes.Length);
    }

    context.Information("Updated portable RID graph from SDK {0}.", sdkVersion);

    if (context.BuildSystem().GitHubActions.IsRunningOnGitHubActions)
    {
        throw new CakeException(
            "global.json changed. Commit src/Cake.NuGet/PortableRuntimeIdentifierGraph.json and PortableRuntimeIdentifierGraph.stamp.");
    }
}
