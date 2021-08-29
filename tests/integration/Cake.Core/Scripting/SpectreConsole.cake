using Spectre.Console;

Task("Cake.Core.Scripting.Spectre.Console.FigletText")
    .Does(() =>
{
    AnsiConsole.Render(
        new FigletText("Cake")
            .LeftAligned()
            .Color(Color.Red));
});

Task("Cake.Core.Scripting.Spectre.Console")
    .IsDependentOn("Cake.Core.Scripting.Spectre.Console.FigletText");
