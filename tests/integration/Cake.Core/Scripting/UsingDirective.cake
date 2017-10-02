using static System.Math;

Task("Cake.Core.Scripting.UsingDirective.UsingStatic")
    .Does(() =>
{
    Information(Round(1.1));
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.UsingDirective")
    .IsDependentOn("Cake.Core.Scripting.UsingDirective.UsingStatic");