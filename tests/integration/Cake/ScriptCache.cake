#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"
using System.Diagnostics;

public class ScriptCacheData
{
    public FilePath ScriptPath { get; }
    public FilePath ScriptCacheAssemblyPath { get; }
    public FilePath ScriptCacheHashPath { get; }
    public (TimeSpan Elapsed, string Hash) CompileResult { get; set; }
    public (TimeSpan Elapsed, string Hash) ExecuteResult { get; set; }
    public (TimeSpan Elapsed, string Hash) ReCompileResult { get; set; }
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

    public (TimeSpan Elapsed, string Hash) TimeCakeExecuteScript() => TimeCakeExecuteScript(args => args);

    public (TimeSpan Elapsed, string Hash) TimeCakeExecuteScript(Func<ProcessArgumentBuilder, ProcessArgumentBuilder> argumentCustomization) =>
        (
            Time(
            () => {
                Settings.ArgumentCustomization = argumentCustomization;
                CakeExecuteScript(
                    ScriptPath,
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
        Settings = new CakeSettings {
                        EnvironmentVariables = new Dictionary<string, string> {
                            { "CAKE_SETTINGS_ENABLESCRIPTCACHE", "true" }
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
    Assert.True(FileExists(data.ScriptCacheAssemblyPath), $"Script Cache Hash Path {data.ScriptCacheHashPath} missing.");
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

Task("Cake.ScriptCache")
    .IsDependentOn("Cake.ScriptCache.Setup")
    .IsDependentOn("Cake.ScriptCache.Compile")
    .IsDependentOn("Cake.ScriptCache.Execute")
    .IsDependentOn("Cake.ScriptCache.ReCompile");