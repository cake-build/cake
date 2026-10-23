#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"

// Opt-in container publishing tests, not part of Run-All-Tests as they require a container engine.
// Run one or more scenarios, i.e.
// --integration-tests-target="Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.Archive"
// --integration-tests-target="Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.LocalRegistry.Podman"
// --integration-tests-target="Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.Registry"

const string ContainerTestRepository = "cake-it-hwapp";
const string ContainerTestTag = "test";
const string ContainerTestRegistryName = "cake-it-registry";
const string ContainerTestRegistry = "localhost:5000";
const string ContainerTestRegistryRepository = "cake-it/hwapp";

FilePath GetContainerTestProject()
    => Paths.Temp.CombineWithFilePath("./Cake.Common/Tools/DotNet/hwapp/hwapp.csproj");

DotNetPublishSettings GetContainerTestPublishSettings()
    => new DotNetPublishSettings
    {
        Container = new DotNetPublishContainerSettings
        {
            OS = DotNetContainerOS.Linux,
            Arch = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture == System.Runtime.InteropServices.Architecture.Arm64
                ? DotNetContainerArchitecture.Arm64
                : DotNetContainerArchitecture.X64
        }
    };

string[] GetContainerToolExecutableNames(string toolName)
    => [toolName, $"{toolName}.exe"];

void PublishAndRunLocalRegistryContainer(DotNetContainerLocalRegistry localRegistry, string toolName)
{
    // Given
    var project = GetContainerTestProject();
    var image = $"{ContainerTestRepository}:{ContainerTestTag}";
    var tool = GetContainerToolExecutableNames(toolName);

    try
    {
        // When
        DotNetPublish(
            project.FullPath,
            GetContainerTestPublishSettings()
                .WithContainerLocalRegistry(localRegistry, ContainerTestRepository, ContainerTestTag));

        Command(tool, out var standardOutput, $"run --rm {image}");

        // Then
        Assert.Contains("Hello World!", standardOutput);
    }
    finally
    {
        Command(tool, out _, out _, $"rmi {image}", settingsCustomization: settings => settings.WithHandleExitCode(_ => true));
    }
}

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.Archive")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // Given
    var project = GetContainerTestProject();
    var archive = Paths.Temp.CombineWithFilePath("./Cake.Common/Tools/DotNet/DotNetPublishContainer/hwapp.tar.gz");
    var settings = GetContainerTestPublishSettings();
    settings.Container = settings.Container with
    {
        Repository = ContainerTestRepository,
        ImageTags = ["1.0.0", "latest"]
    };

    // When
    DotNetPublish(project.FullPath, settings.WithContainerArchive(archive));

    // Then
    Assert.True(System.IO.File.Exists(archive.FullPath), "Path:" + archive.FullPath);

    // The SDK writes an uncompressed tar regardless of extension, so only decompress when gzip magic bytes are present
    var manifests = new System.Text.StringBuilder();
    using (var archiveStream = System.IO.File.OpenRead(archive.FullPath))
    {
        var isGzip = archiveStream.ReadByte() == 0x1f & archiveStream.ReadByte() == 0x8b;
        archiveStream.Position = 0;

        using var tarStream = isGzip
            ? new System.IO.Compression.GZipStream(archiveStream, System.IO.Compression.CompressionMode.Decompress, leaveOpen: true)
            : (System.IO.Stream)archiveStream;
        using var tarReader = new System.Formats.Tar.TarReader(tarStream, leaveOpen: true);

        while (tarReader.GetNextEntry() is { } entry)
        {
            if (entry.Name.TrimStart('.', '/') is "manifest.json" or "index.json" && entry.DataStream is { } dataStream)
            {
                using var reader = new System.IO.StreamReader(dataStream);
                manifests.AppendLine(reader.ReadToEnd());
            }
        }
    }

    var manifest = manifests.ToString();
    Information("Archive manifest: {0}", manifest);
    Assert.Contains($"{ContainerTestRepository}:1.0.0", manifest);
    Assert.Contains($"{ContainerTestRepository}:latest", manifest);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.LocalRegistry.Docker")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() => PublishAndRunLocalRegistryContainer(DotNetContainerLocalRegistry.Docker, "docker"));

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.LocalRegistry.Podman")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() => PublishAndRunLocalRegistryContainer(DotNetContainerLocalRegistry.Podman, "podman"));

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.LocalRegistry.Wslc")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() => PublishAndRunLocalRegistryContainer(DotNetContainerLocalRegistry.Wslc, "wslc"));

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.Registry")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(async () =>
{
    // Given, a throwaway registry only reachable on the loopback interface, nothing is pushed to a public registry
    var registryUri = new Uri($"http://{ContainerTestRegistry}");
    Assert.True(registryUri.IsLoopback, $"Registry {ContainerTestRegistry} must be a loopback address.");

    var tool = new[] { "docker", "podman", "wslc" }
        .Select(GetContainerToolExecutableNames)
        .FirstOrDefault(names => names.Any(name => Context.Tools.Resolve(name) != null))
        ?? throw new Exception("No container engine (docker, podman or wslc) found to host the test registry.");
    Information("Hosting test registry using {0}", tool[0]);

    var project = GetContainerTestProject();
    using var httpClient = new System.Net.Http.HttpClient();

    try
    {
        Command(tool, $"run -d --rm --name {ContainerTestRegistryName} -p 127.0.0.1:5000:5000 registry:2");

        var ready = false;
        for (var attempt = 0; attempt < 30 && !ready; attempt++)
        {
            try
            {
                ready = (await httpClient.GetAsync(new Uri(registryUri, "/v2/"))).IsSuccessStatusCode;
            }
            catch (System.Net.Http.HttpRequestException)
            {
            }

            if (!ready)
            {
                await System.Threading.Tasks.Task.Delay(1000);
            }
        }

        Assert.True(ready, $"Registry {ContainerTestRegistry} didn't become ready.");

        // When
        DotNetPublish(
            project.FullPath,
            GetContainerTestPublishSettings()
                .WithContainerRegistry(ContainerTestRegistry, ContainerTestRegistryRepository, ContainerTestTag));

        // Then
        var tags = await httpClient.GetStringAsync(new Uri(registryUri, $"/v2/{ContainerTestRegistryRepository}/tags/list"));
        Information("Registry tags: {0}", tags);
        Assert.Contains($"\"{ContainerTestTag}\"", tags);
    }
    finally
    {
        Command(tool, out _, out _, $"rm -f {ContainerTestRegistryName}", settingsCustomization: settings => settings.WithHandleExitCode(_ => true));
    }
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.Archive")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.LocalRegistry.Docker")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.LocalRegistry.Podman")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.LocalRegistry.Wslc")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublishContainer.Registry");
