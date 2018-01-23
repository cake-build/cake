// Utilities
#load "./utilities/paths.cake"
#load "./utilities/xunit.cake"
#load "./utilities/context.cake"

//////////////////////////////////////////////////
// TEARDOWN
//////////////////////////////////////////////////

Teardown<ScriptContext>((context, data) => 
{
    Assert.NotNull(data);
    Assert.True(data.Initialized);
});