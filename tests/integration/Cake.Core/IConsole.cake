#load "./../../utilities/xunit.cake"
using System.IO;

Task("Cake.Core.IConsole.WriteLine.Literal")
    .Does(context =>
{
    var console = new CakeConsole(context.Environment);

    var output = CaptureStdOut(console, c => c.WriteLine("{0}"));

    Assert.Equal("{0}" + Environment.NewLine, output);
});

Task("Cake.Core.IConsole.WriteLine.Formatted")
    .Does(context =>
{
    var console = new CakeConsole(context.Environment);

    var output = CaptureStdOut(console, c => c.WriteLine("{0}", "x"));

    Assert.Equal("x" + Environment.NewLine, output);
});

Task("Cake.Core.IConsole.Write.Literal")
    .Does(context =>
{
    var console = new CakeConsole(context.Environment);

    var output = CaptureStdOut(console, c => c.Write("{0}"));

    Assert.Equal("{0}", output);
});

Task("Cake.Core.IConsole.Write.Formatted")
    .Does(context =>
{
    var console = new CakeConsole(context.Environment);

    var output = CaptureStdOut(console, c => c.Write("{0}", "x"));

    Assert.Equal("x", output);
});

Task("Cake.Core.IConsole.WriteErrorLine.Literal")
    .Does(context =>
{
    var console = new CakeConsole(context.Environment);

    var output = CaptureStdErr(console, c => c.WriteErrorLine("{0}"));

    Assert.Equal("{0}" + Environment.NewLine, output);
});

Task("Cake.Core.IConsole.WriteErrorLine.Formatted")
    .Does(context =>
{
    var console = new CakeConsole(context.Environment);

    var output = CaptureStdErr(console, c => c.WriteErrorLine("{0}", "x"));

    Assert.Equal("x" + Environment.NewLine, output);
});

Task("Cake.Core.IConsole.WriteError.Literal")
    .Does(context =>
{
    var console = new CakeConsole(context.Environment);

    var output = CaptureStdErr(console, c => c.WriteError("{0}"));

    Assert.Equal("{0}", output);
});

Task("Cake.Core.IConsole.WriteError.Formatted")
    .Does(context =>
{
    var console = new CakeConsole(context.Environment);

    var output = CaptureStdErr(console, c => c.WriteError("{0}", "x"));

    Assert.Equal("x", output);
});

Task("Cake.Core.IConsole")
    .IsDependentOn("Cake.Core.IConsole.WriteLine.Literal")
    .IsDependentOn("Cake.Core.IConsole.WriteLine.Formatted")
    .IsDependentOn("Cake.Core.IConsole.Write.Literal")
    .IsDependentOn("Cake.Core.IConsole.Write.Formatted")
    .IsDependentOn("Cake.Core.IConsole.WriteErrorLine.Literal")
    .IsDependentOn("Cake.Core.IConsole.WriteErrorLine.Formatted")
    .IsDependentOn("Cake.Core.IConsole.WriteError.Literal")
    .IsDependentOn("Cake.Core.IConsole.WriteError.Formatted");

static string CaptureStdOut(CakeConsole console, Action<CakeConsole> write)
{
    var original = Console.Out;
    var writer = new StringWriter();
    try
    {
        Console.SetOut(writer);
        write(console);
        return writer.ToString();
    }
    finally
    {
        Console.SetOut(original);
    }
}

static string CaptureStdErr(CakeConsole console, Action<CakeConsole> write)
{
    var original = Console.Error;
    var writer = new StringWriter();
    try
    {
        Console.SetError(writer);
        write(console);
        return writer.ToString();
    }
    finally
    {
        Console.SetError(original);
    }
}
