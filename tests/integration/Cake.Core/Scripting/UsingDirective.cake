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
    var useWriter = true;
    using var writer = useWriter
        ? new StringWriter()
        : null;
    using TextReader other = reader;
});

Task("Cake.Core.Scripting.UsingDirective.UsingDisposableMultiLine")
    .Does(() =>
{
    using var writer = 1 == 1
        ? new StringWriter()
        : null;
    using var reader =
        new StringReader("Reader");
    Information("{0}", reader.ReadLine());
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.UsingDirective")
    .IsDependentOn("Cake.Core.Scripting.UsingDirective.UsingStatic")
    .IsDependentOn("Cake.Core.Scripting.UsingDirective.UsingDisposable")
    .IsDependentOn("Cake.Core.Scripting.UsingDirective.UsingDisposableMultiLine");