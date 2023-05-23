#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"
using System.Diagnostics;

public class ScriptCacheData
{
    public FilePath ScriptPath { get; }
    public GlobPattern ScriptCacheAssemblyPattern { get; }
    public FilePath ConfigScriptPath { get; }
    public DirectoryPath ConfigScriptCachePath { get; }
    public GlobPattern ConfigScriptCacheAssemblyPattern { get; }
    public (TimeSpan Elapsed, string Hash) CompileResult { get; set; }
    public (TimeSpan Elapsed, string Hash) ExecuteResult { get; set; }
    public (TimeSpan Elapsed, string Hash) ReCompileResult { get; set; }
    public (TimeSpan Elapsed, string Hash) ConfigCompileResult { get; set; }
    public CakeSettings Settings { get; }
    private Action<FilePath, CakeSettings> CakeExecuteScript { get; }
    private Func<GlobPattern, FileHash> CalculateFileHash { get; }

    public TimeSpan Time(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            action();
        }
        finally
        {
            stopwatch.Stop();
        }
        return stopwatch.Elapsed;
    }

    public (TimeSpan Elapsed, string Hash) TimeCakeExecuteScript(FilePath scriptPath = null)
        => TimeCakeExecuteScript(args => args, scriptPath);

    public (TimeSpan Elapsed, string Hash) TimeCakeExecuteScript(Func<ProcessArgumentBuilder, ProcessArgumentBuilder> argumentCustomization, FilePath scriptPath = null) =>
        (
            Time(
            () => {
                Settings.ArgumentCustomization = argumentCustomization;
                CakeExecuteScript(
                    scriptPath ?? ScriptPath,
                    Settings);
            }),
            CalculateFileHash(ScriptCacheAssemblyPattern).ToHex()
        );

    public ScriptCacheData(
        DirectoryPath scriptDirectoryPath,
        Action<FilePath, CakeSettings> cakeExecuteScript,
        Func<GlobPattern, FileHash> calculateFileHash
        )
    {
        var configScriptDirectoryPath = scriptDirectoryPath.Combine("Config");
        var configCacheRootPath = configScriptDirectoryPath.Combine("CacheRootPath");

        ScriptPath = scriptDirectoryPath
                        .CombineWithFilePath("build.cake");
        ScriptCacheAssemblyPattern = scriptDirectoryPath
                                        .Combine("tools")
                                        .Combine("cache")
                                        .CombineWithFilePath($"build.BuildScriptHost.*.dll")
                                        .FullPath;

        ConfigScriptPath = configScriptDirectoryPath
                                .CombineWithFilePath("build.cake");
        ConfigScriptCachePath = configCacheRootPath
                                    .Combine("cake-build")
                                    .Combine("CacheLeafPath");
        ConfigScriptCacheAssemblyPattern = ConfigScriptCachePath
                                            .CombineWithFilePath($"build.BuildScriptHost.*.dll")
                                            .FullPath;

        Settings = new CakeSettings {
                        EnvironmentVariables = new Dictionary<string, string> {
                            { "CAKE_SETTINGS_ENABLESCRIPTCACHE", "true" },
                            { "TEST_ROOT_PATH", configCacheRootPath.FullPath },
                            { "TEST_LEAF_PATH", "CacheLeafPath" }
                        },
                        Verbosity = Verbosity.Quiet
                    };
        CakeExecuteScript = cakeExecuteScript;
        CalculateFileHash = calculateFileHash;
    }
}

Setup(context =>
    new ScriptCacheData(
            Paths
                .Temp
                .Combine("./Cake/ScriptCache"),
            context.CakeExecuteScript,
            globberPattern => context.CalculateFileHash(context.GetFiles(globberPattern).OrderByDescending(file => System.IO.File.GetLastWriteTime(file.FullPath)).FirstOrDefault())
        )
    );

Task("Cake.ScriptCache.Setup")
    .Does(() =>
{
    var sourcePath = Paths.Resources.Combine("./Cake/ScriptCache");
    var targetPath = Paths.Temp.Combine("./Cake/ScriptCache");
    EnsureDirectoryExists(targetPath.Combine("../").Collapse());
    if (DirectoryExists(targetPath))
    {
        DeleteDirectory(
            targetPath,
                new DeleteDirectorySettings {
                Recursive = true,
                Force = true
            });
    }
    CopyDirectory(sourcePath, targetPath);
});

Task("Cake.ScriptCache.Compile")
    .IsDependentOn("Cake.ScriptCache.Setup")
    .Does<ScriptCacheData>((context, data) =>
{
    // Given / When
    data.CompileResult = data.TimeCakeExecuteScript();

    // Then
    var count = GetFiles(data.ScriptCacheAssemblyPattern).Count();
    Assert.True(1 == count, $"Script Cache Assembly Path {data.ScriptCacheAssemblyPattern.Pattern} expected 1 got {count}.");
});

var scriptCacheExecute =  Task("Cake.ScriptCache.Execute");
for(var index = 1; index <= 5; index++)
{
    scriptCacheExecute.IsDependentOn(
        Task($"Cake.ScriptCache.Execute.{index}")
            .Does<ScriptCacheData>((context, data) =>
        {
            // Given / When
            data.ExecuteResult = data.TimeCakeExecuteScript();

            // Then
            Assert.True(data.CompileResult.Elapsed > data.ExecuteResult.Elapsed, $"Compile time {data.CompileResult.Elapsed} should be greater than execute time  {data.ExecuteResult.Elapsed}.");
            Assert.Equal(data.CompileResult.Hash, data.ExecuteResult.Hash);
        })
    );
}

Task("Cake.ScriptCache.ReCompile")
    .IsDependentOn("Cake.ScriptCache.Execute")
    .Does<ScriptCacheData>((context, data) => {
        // Given / When
        data.ReCompileResult = data.TimeCakeExecuteScript(args => args.Append("--invalidate-script-cache"));

        // Then
        Assert.True(data.ReCompileResult.Elapsed> data.ExecuteResult.Elapsed, $"ReCompileTime time {data.ReCompileResult.Elapsed} should be greater than execute time  {data.ExecuteResult.Elapsed}.");
        Assert.NotEqual(data.CompileResult.Hash , data.ReCompileResult.Hash);
    });

Task("Cake.ScriptCache.Config")
    .Does<ScriptCacheData>((context, data) => {
        // Given / When
        data.ConfigCompileResult = data.TimeCakeExecuteScript(data.ConfigScriptPath);

        // Then
        var count = GetFiles(data.ConfigScriptCacheAssemblyPattern).Count();
        Assert.True(1 == count, $"Script Cache Assembly Path {data.ConfigScriptCacheAssemblyPattern.Pattern} expected 1 got {count}.");
    });

Task("Cake.ScriptCache")
    .IsDependentOn("Cake.ScriptCache.Setup")
    .IsDependentOn("Cake.ScriptCache.Compile")
    .IsDependentOn("Cake.ScriptCache.Execute")
    .IsDependentOn("Cake.ScriptCache.ReCompile")
    .IsDependentOn("Cake.ScriptCache.Config");