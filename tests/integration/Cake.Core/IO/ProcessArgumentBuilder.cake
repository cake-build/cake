#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"
using System.Text;

Task("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.Spaces")
    .Does(context =>
{
    var received = EchoProcessArguments(context, args => args
        .AppendQuoted("My Folder"));

    Assert.Equal(new[] { "My Folder" }, received);
});

Task("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.TrailingBackslash")
    .Does(context =>
{
    var received = EchoProcessArguments(context, args => args
        .AppendQuoted(@"C:\My Folder\"));

    Assert.Equal(new[] { @"C:\My Folder\" }, received);
});

Task("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.EmbeddedQuotes")
    .Does(context =>
{
    var received = EchoProcessArguments(context, args => args
        .AppendQuoted("say \"hello\""));

    Assert.Equal(new[] { "say \"hello\"" }, received);
});

Task("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.BackslashBeforeQuote")
    .Does(context =>
{
    var received = EchoProcessArguments(context, args => args
        .AppendQuoted("\\\\\""));

    Assert.Equal(new[] { "\\\\\"" }, received);
});

Task("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.Empty")
    .Does(context =>
{
    var received = EchoProcessArguments(context, args => args
        .AppendQuoted(string.Empty));

    Assert.Equal(new[] { string.Empty }, received);
});

Task("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.Multiple")
    .Does(context =>
{
    var received = EchoProcessArguments(context, args => args
        .AppendQuoted("My Folder")
        .AppendQuoted(@"C:\My Folder\")
        .AppendQuoted("say \"hello\"")
        .AppendQuoted(string.Empty));

    Assert.Equal(
        new[] { "My Folder", @"C:\My Folder\", "say \"hello\"", string.Empty },
        received);
});

Task("Cake.Core.IO.ProcessArgumentBuilder.AppendSwitchQuoted.Separated")
    .Does(context =>
{
    var received = EchoProcessArguments(context, args => args
        .AppendSwitchQuoted("--input", "My Folder"));

    Assert.Equal(new[] { "--input", "My Folder" }, received);
});

Task("Cake.Core.IO.ProcessArgumentBuilder.AppendSwitchQuoted.Glued")
    .Does(context =>
{
    var received = EchoProcessArguments(context, args => args
        .AppendSwitchQuoted("-o", "=", @"C:\My Folder\"));

    Assert.Equal(new[] { @"-o=C:\My Folder\" }, received);
});

Task("Cake.Core.IO.ProcessArgumentBuilder")
    .IsDependentOn("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.Spaces")
    .IsDependentOn("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.TrailingBackslash")
    .IsDependentOn("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.EmbeddedQuotes")
    .IsDependentOn("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.BackslashBeforeQuote")
    .IsDependentOn("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.Empty")
    .IsDependentOn("Cake.Core.IO.ProcessArgumentBuilder.AppendQuoted.Multiple")
    .IsDependentOn("Cake.Core.IO.ProcessArgumentBuilder.AppendSwitchQuoted.Separated")
    .IsDependentOn("Cake.Core.IO.ProcessArgumentBuilder.AppendSwitchQuoted.Glued");

static FilePath EnsureEchoArgsBuilt(ICakeContext context)
{
    var fileName = context.Tools.Resolve("dotnet.exe")
                    ?? context.Tools.Resolve("dotnet");
    var project = Paths.Resources.Combine("Cake.Core/IO/EchoArgs").CombineWithFilePath("EchoArgs.csproj");
    var outputDir = Paths.Temp.Combine("EchoArgs");
    var dll = outputDir.CombineWithFilePath("EchoArgs.dll");

    if (!context.FileExists(dll))
    {
        var buildExit = context.StartProcess(
            fileName,
            new ProcessSettings
            {
                Arguments = new ProcessArgumentBuilder()
                    .Append("build")
                    .AppendQuoted(project.FullPath)
                    .Append("-o")
                    .AppendQuoted(outputDir.FullPath)
                    .Append("-v")
                    .Append("q")
            });
        Assert.Equal(0, buildExit);
    }

    return dll;
}

static string[] EchoProcessArguments(ICakeContext context, Action<ProcessArgumentBuilder> appendArgs)
{
    var fileName = context.Tools.Resolve("dotnet.exe")
                    ?? context.Tools.Resolve("dotnet");
    var dll = EnsureEchoArgsBuilt(context);
    var arguments = new ProcessArgumentBuilder()
        .Append("exec")
        .AppendQuoted(dll.FullPath);
    appendArgs(arguments);

    IEnumerable<string> redirectedStandardOutput;
    var exitCode = context.StartProcess(
        fileName,
        new ProcessSettings
        {
            Arguments = arguments,
            RedirectStandardOutput = true
        },
        out redirectedStandardOutput);

    Assert.Equal(0, exitCode);

    return redirectedStandardOutput
        .Select(line => Encoding.UTF8.GetString(Convert.FromBase64String(line ?? string.Empty)))
        .ToArray();
}
