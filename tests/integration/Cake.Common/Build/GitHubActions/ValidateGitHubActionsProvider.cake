#load "./../../../utilities/xunit.cake"

public record BuildData(string GitVersion, string Path, string OS)
{
    public string GitVersionAndOS { get; } = string.Join(
                                                '_',
                                                GitVersion,
                                                OS);
}

Setup(
    context => new BuildData(
                    EnvironmentVariable("GitVersion_MajorMinorPatch") ?? throw new ArgumentNullException("Missing GitVersion Variable.", "GitVersion_MajorMinorPatch"),
                    EnvironmentVariable("PATH") ?? throw new ArgumentNullException("Missing PATH varable.", "PATH"),
                    GitHubActions.Environment.Runner.OS.ToUpper()
                )
);


Task("ValidateEnvironment")
    .DoesForEach<BuildData, string>(
        data => new [] {
            $"CAKE_{data.OS}_NETCOREAPP_6_0_VERSION",
            $"CAKE_{data.OS}_NETCOREAPP_7_0_VERSION",
            $"CAKE_{data.OS}_NETCOREAPP_8_0_VERSION"
        },
        (data, envKey) => Assert.Equal(data.GitVersion, EnvironmentVariable(envKey))
    );

Task("ValidatePath")
    .DoesForEach<BuildData, string>(
        new [] {
            "Cake\\WTool\\Wtools\\Wnet8\\W0",
            "Cake\\WTool\\Wtools\\Wnet7\\W0",
            "Cake\\WTool\\Wtools\\Wnet6\\W0"
        },
        (data, path) => Assert.Matches(path, data.Path)
    );

Task("ValidateVariable")
    .DoesForEach<BuildData, string>(
        () => new [] {
            "CAKE_NETCOREAPP_6_0_VERSION_OS",
            "CAKE_NETCOREAPP_7_0_VERSION_OS",
            "CAKE_NETCOREAPP_8_0_VERSION_OS"
        },
        (data, varKey) => Assert.Equal(data.GitVersionAndOS, Argument<string>(varKey))
    );

Task("Default")
    .IsDependentOn("ValidateEnvironment")
    .IsDependentOn("ValidatePath")
    .IsDependentOn("ValidateVariable");


RunTarget(Argument("target", "Default"));