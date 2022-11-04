// Utilities
#load "./utilities/paths.cake"
#load "./utilities/xunit.cake"
#load "./utilities/context.cake"

//////////////////////////////////////////////////
// SETUP
//////////////////////////////////////////////////

Setup(setupContext =>
{
    setupContext.Log.Information("Running regular setup.");
});

Setup<ScriptContext>(setupContext =>
{
    // Output information from setup task
    setupContext.Log.Information(
        Verbosity.Quiet,
        "Performing setup initated by {0} ({1} tasks to be executed beginning with {2}, performing {3} build on {4})",
        setupContext.TargetTask?.Name,
        setupContext.TasksToExecute?.Count,
        setupContext.TasksToExecute?.Select(task => task.Name).FirstOrDefault(),
        BuildSystem.Provider,
        setupContext.Environment.Runtime.BuiltFramework
        );

    // Perform artifact cleanup
    CleanDirectory(Paths.Temp);

    // Create a new script context.
    return new ScriptContext(true);
});

Setup<AlternativeScriptContext>(setupContext =>
{
    setupContext.Log.Information("Running setup with alternative context.");
    return new AlternativeScriptContext(true);
});

//////////////////////////////////////////////////
// TESTS
//////////////////////////////////////////////////

Task("Can-Access-Typed-Data")
    .Does<ScriptContext>(data =>
{
    Assert.True(data.Initialized);
});

Task("Can-Access-Typed-Data-On-Alternative-Context")
    .Does<AlternativeScriptContext>(data =>
{
    Assert.True(data.EnginesStarted);
});

Task("Can-Access-Typed-Data-Async")
    .Does<ScriptContext>(async data =>
{
    await System.Threading.Tasks.Task.Delay(0);
});

Task("Can-Access-Typed-Data-With-Context")
    .Does<ScriptContext>((context, data) =>
{
    Assert.True(data.Initialized);
});

Task("Can-Access-Typed-Data-With-Context-Async")
    .Does<ScriptContext>(async (context, data) =>
{
    await System.Threading.Tasks.Task.Delay(0);
});

Task("Can-Access-Typed-Data-WithCriteria-True")
    .WithCriteria<ScriptContext>((context, data) => data.Initialized)
    .Does<ScriptContext>(data =>
{
    Assert.True(data.Initialized);
});

Task("Can-Access-Typed-Data-WithCriteria-False-Message")
    .WithCriteria<ScriptContext>((context, data) => !data.Initialized, "Should only run if not initialized.")
    .Does<ScriptContext>(data =>
{
    Assert.False(data.Initialized);
});

TaskOf<ScriptContext>("Can-Access-Typed-Data-Task")
    .Description("Very typed task")
    .WithCriteria(static (context, data) => true)
    .Does(static (context, data) => context.Information("Initialized: {0}.", data.Initialized))
    .Does(static async (context, data) => await System.Threading.Tasks.Task.CompletedTask)
    .DoesForEach(
        static (data, context) => new [] { data.Initialized },
        static (data, item, context) => context.Information("Initialized: {0}.", data.Initialized)
    )
    .DoesForEach(
        new [] { true, false },
        static (data, item, context) => context.Information("Initialized: {0}.", data.Initialized)
    );


Task("Can-Access-Typed-Data-Task-Of")
    .Of<ScriptContext>()
    .Description("Very typed task")
    .WithCriteria(static (context, data) => true)
    .Does(static (context, data) => context.Information("Initialized: {0}.", data.Initialized))
    .Does(static async (context, data) => await System.Threading.Tasks.Task.CompletedTask)
    .DoesForEach(
        static (data, context) => new [] { data.Initialized },
        static (data, item, context) => context.Information("Initialized: {0}.", data.Initialized)
    )
    .DoesForEach(
        new [] { true, false },
        static (data, item, context) => context.Information("Initialized: {0}.", data.Initialized)
    );

Task("Can-Access-Typed-Data-Finally")
    .Does<ScriptContext>(data =>
{
})
.Finally<ScriptContext>(
    (context, data) => Assert.True(data.Initialized)
);

Task("Can-Access-Typed-Data-Finally-Async")
    .Does<ScriptContext>(async data =>
{
    await System.Threading.Tasks.Task.Delay(0);
}).Finally<ScriptContext>(
    async (context, data) => {
        Assert.True(data.Initialized);
        await System.Threading.Tasks.Task.Delay(0);
    }
);

//////////////////////////////////////////////////
// TARGETS
//////////////////////////////////////////////////

Task("Setup-Tests")
    .IsDependentOn("Can-Access-Typed-Data")
    .IsDependentOn("Can-Access-Typed-Data-Async")
    .IsDependentOn("Can-Access-Typed-Data-With-Context")
    .IsDependentOn("Can-Access-Typed-Data-With-Context-Async")
    .IsDependentOn("Can-Access-Typed-Data-WithCriteria-True")
    .IsDependentOn("Can-Access-Typed-Data-WithCriteria-False-Message")
    .IsDependentOn("Can-Access-Typed-Data-Task")
    .IsDependentOn("Can-Access-Typed-Data-Task-Of")
    .IsDependentOn("Can-Access-Typed-Data-Finally")
    .IsDependentOn("Can-Access-Typed-Data-Finally-Async");
