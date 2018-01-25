#load "../../resources/Cake.Core/Scripting/file with spaces.cake"
#load "%CAKE_INTEGRATION_TEST_ROOT%/resources/Cake.Core/Scripting/file with spaces.cake"

Task("Cake.Core.Scripting.LoadDirective.WithSpaces")
    .Does(() =>
{
    // Just a place holder to see that the test file gets loaded.
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.LoadDirective")
    .IsDependentOn("Cake.Core.Scripting.LoadDirective.WithSpaces");