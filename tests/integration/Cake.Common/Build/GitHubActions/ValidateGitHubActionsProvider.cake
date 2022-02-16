#load "./../../../utilities/xunit.cake"

public record BuildData(string GitVersion, string Path, string OS);

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
            $"CAKE_{data.OS}_NETCOREAPP_3_1_VERSION",
            $"CAKE_{data.OS}_NETCOREAPP_5_0_VERSION",
            $"CAKE_{data.OS}_NETCOREAPP_6_0_VERSION"
        },
        (data, envKey) => Assert.Equal(data.GitVersion, EnvironmentVariable(envKey))
    );

Task("ValidatePath")
    .DoesForEach<BuildData, string>(
        new [] {
            "Cake\\WTool\\Wtools\\Wnet6\\W0",
            "Cake\\WTool\\Wtools\\Wnet5\\W0",
            "Cake\\WTool\\Wtools\\Wnetcoreapp3\\W1"
        },
        (data, path) => Assert.Matches(path, data.Path)
    );

Task("Default")
    .IsDependentOn("ValidateEnvironment")
    .IsDependentOn("ValidatePath");


RunTarget(Argument("target", "Default"));