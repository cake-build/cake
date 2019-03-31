Task("Cake.Core.Scripting.LoadDirective.Globber.Throws")
    .Does(() =>
{
    throw new InvalidOperationException();
});

loadDirectiveTask
    .IsDependentOn("Cake.Core.Scripting.LoadDirective.Globber.Throws");