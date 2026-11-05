// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Scripting;
using Spectre.Console;

namespace Cake.Cli;

/// <summary>
/// The script host used for showing task descriptions.
/// </summary>
public class DescriptionScriptHost : ScriptHost
{
    private readonly IAnsiConsole _console;

    /// <summary>
    /// Initializes a new instance of the <see cref="DescriptionScriptHost"/> class.
    /// </summary>
    /// <param name="engine">The engine.</param>
    /// <param name="context">The context.</param>
    /// <param name="console">The console.</param>
    public DescriptionScriptHost(ICakeEngine engine, ICakeContext context, IAnsiConsole console)
        : base(engine, context)
    {
        ArgumentNullException.ThrowIfNull(console);
        _console = console;
    }

    /// <inheritdoc/>
    public override Task<CakeReport> RunTargetAsync(string target)
    {
        PrintTaskDescriptions();

        return System.Threading.Tasks.Task.FromResult<CakeReport>(null);
    }

    /// <inheritdoc/>
    public override Task<CakeReport> RunTargetsAsync(IEnumerable<string> targets)
    {
        PrintTaskDescriptions();

        return System.Threading.Tasks.Task.FromResult<CakeReport>(null);
    }

    private void PrintTaskDescriptions()
    {
        var rowStyle = new Style(ConsoleColor.White);

        var table = new Table().Border(TableBorder.SimpleHeavy);
        table.Width(100);
        table.BorderStyle(Style.Plain.Foreground(ConsoleColor.Green));

        table.AddColumn(new TableColumn(new Text("Task", rowStyle)).PadRight(10));
        table.AddColumn(new TableColumn(new Text("Description", rowStyle)));

        foreach (var task in Tasks)
        {
            table.AddRow(
                new Markup(task.Name.EscapeMarkup(), rowStyle),
                new Markup((task.Description ?? string.Empty).EscapeMarkup(), rowStyle));
        }

        _console.Write(table);
    }
}
