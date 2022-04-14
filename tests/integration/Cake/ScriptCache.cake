#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"
using System.Diagnostics;

public class ScriptCacheData
{
    public FilePath ScriptPath { get; }
    public FilePath ScriptCacheAssemblyPath { get; }
    public FilePath ScriptCacheHashPath { get; }
    public FilePath ConfigScriptPath { get; }
    public DirectoryPath ConfigScriptCachePath { get; }
    public FilePath ConfigScriptCacheAssemblyPath { get; }
    public FilePath ConfigScriptCacheHashPath { get; }
    public (TimeSpan Elapsed, string Hash) CompileResult { get; set; }
    public (TimeSpan Elapsed, string Hash) ExecuteResult { get; set; }
    public (TimeSpan Elapsed, string Hash) ReCompileResult { get; set; }
    public (TimeSpan Elapsed, string Hash) ConfigCompileResult { get; set; }
    public CakeSettings Settings { get; }
    private Action<FilePath, CakeSettings> CakeExecuteScript { get; }
    private Func<FilePath, FileHash> CalculateFileHash { get; }

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
            CalculateFileHash(ScriptCacheAssemblyPath).ToHex()
        );

    public ScriptCacheData(
        DirectoryPath scriptDirectoryPath,
        Action<FilePath, CakeSettings> cakeExecuteScript,
        Func<FilePath, FileHash> calculateFileHash
        )
    {
        ScriptPath = scriptDirectoryPath.CombineWithFilePath("build.cake");
        var cacheDirectoryPath = scriptDirectoryPath.Combine("tools").Combine("cache");
        ScriptCacheAssemblyPath = cacheDirectoryPath.CombineWithFilePath("build.cake.dll");
        ScriptCacheHashPath = cacheDirectoryPath.CombineWithFilePath("build.cake.hash");
        var configScriptDirectoryPath = scriptDirectoryPath.Combine("Config");
        ConfigScriptPath = configScriptDirectoryPath.CombineWithFilePath("build.cake");
        var configCacheRootPath = configScriptDirectoryPath.Combine("CacheRootPath");
        ConfigScriptCachePath = configCacheRootPath.Combine("cake-build").Combine("CacheLeafPath");
        ConfigScriptCacheAssemblyPath = ConfigScriptCachePath.CombineWithFilePath("build.cake.dll");
        ConfigScriptCacheHashPath = ConfigScriptCachePath.CombineWithFilePath("build.cake.hash");
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
                    Paths.Temp
                        .Combine("./Cake/ScriptCache"),
                    context.CakeExecuteScript,
                    context.CalculateFileHash
    ));

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
    Assert.True(FileExists(data.ScriptCacheAssemblyPath), $"Script Cache Assembly Path {data.ScriptCacheAssemblyPath} missing.");
    Assert.True(FileExists(data.ScriptCacheHashPath), $"Script Cache Hash Path {data.ScriptCacheHashPath} missing.");
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
        Assert.True(FileExists(data.ConfigScriptCacheAssemblyPath), $"Script Cache Assembly Path {data.ConfigScriptCacheAssemblyPath} missing.");
        Assert.True(FileExists(data.ConfigScriptCacheHashPath), $"Script Cache Hash Path {data.ConfigScriptCacheHashPath} missing.");
    });

Task("Cake.ScriptCache")
    .IsDependentOn("Cake.ScriptCache.Setup")
    .IsDependentOn("Cake.ScriptCache.Compile")
    .IsDependentOn("Cake.ScriptCache.Execute")
    .IsDependentOn("Cake.ScriptCache.ReCompile")
    .IsDependentOn("Cake.ScriptCache.Config");