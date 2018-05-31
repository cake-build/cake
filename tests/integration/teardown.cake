// Utilities
#load "./utilities/paths.cake"
#load "./utilities/xunit.cake"
#load "./utilities/context.cake"

//////////////////////////////////////////////////
// TEARDOWN
//////////////////////////////////////////////////

Teardown(teardownContext => 
{
    teardownContext.Log.Information("Running regular teardown.");
});

Teardown<ScriptContext>((teardownContext, data) => 
{
    Assert.NotNull(data);
    Assert.True(data.Initialized);
});

Teardown<AlternativeScriptContext>((teardownContext, data) => 
{
    Assert.NotNull(data);
    Assert.True(data.EnginesStarted);
});