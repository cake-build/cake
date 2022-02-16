using static System.Math;

Task("Cake.Core.Scripting.UsingDirective.UsingStatic")
    .Does(() =>
{
    Information(Round(1.1));
});

Task("Cake.Core.Scripting.UsingDirective.UsingDisposable")
    .Does(() =>
{
    // Intentionally not indented
using var @string = new StringReader("String");
    using var reader = new StringReader("Reader");
    Information("{0}{1}", @string.ReadLine(), reader.ReadLine());
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.UsingDirective")
    .IsDependentOn("Cake.Core.Scripting.UsingDirective.UsingStatic")
    .IsDependentOn("Cake.Core.Scripting.UsingDirective.UsingDisposable");