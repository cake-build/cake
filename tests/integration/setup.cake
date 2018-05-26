// Utilities
#load "./utilities/paths.cake"
#load "./utilities/xunit.cake"
#load "./utilities/context.cake"

//////////////////////////////////////////////////
// SETUP
//////////////////////////////////////////////////

Setup<ScriptContext>(ctx =>
{
    CleanDirectory(Paths.Temp);

    // Create a new script context.
    return new ScriptContext(true);
});

//////////////////////////////////////////////////
// TESTS
//////////////////////////////////////////////////

Task("Can-Access-Typed-Data")
    .Does<ScriptContext>(data => 
{
    Assert.True(data.Initialized);
});

Task("Can-Access-Typed-Data-Async")
    .Does<ScriptContext>(async data => 
{
    await System.Threading.Tasks.Task.Delay(0);
});

Task("Can-Access-Typed-Data-With-Context")
    .Does<ScriptContext>((data, context) => 
{
    Assert.True(data.Initialized);
});

Task("Can-Access-Typed-Data-With-Context-Async")
    .Does<ScriptContext>(async (data, context) => 
{
    await System.Threading.Tasks.Task.Delay(0);
});

//////////////////////////////////////////////////
// TARGETS
//////////////////////////////////////////////////

Task("Setup-Tests")
    .IsDependentOn("Can-Access-Typed-Data")
    .IsDependentOn("Can-Access-Typed-Data-Async")
    .IsDependentOn("Can-Access-Typed-Data-With-Context")
    .IsDependentOn("Can-Access-Typed-Data-With-Context-Async");