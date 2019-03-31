Task("Cake.Core.Scripting.LoadDirective.Globber.NoThrow")
    .Does(() =>
{
});

loadDirectiveTask
    .IsDependentOn("Cake.Core.Scripting.LoadDirective.Globber.NoThrow");