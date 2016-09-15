#load "./../../utilities/xunit.cake"

var verbosities = Enum.GetValues(typeof(Cake.Core.Diagnostics.Verbosity))
                    .Cast<Cake.Core.Diagnostics.Verbosity>()
                    .ToArray();

var logStringMethods = new [] {
    new { Name = "Error", Action = new Action<string, object[]>(Error)},
    new { Name = "Warning", Action = new Action<string, object[]>(Warning)},
    new { Name = "Information", Action = new Action<string, object[]>(Information)},
    new { Name = "Verbose", Action = new Action<string, object[]>(Verbose)},
    new { Name = "Debug", Action = new Action<string, object[]>(Debug)}
};

var logActionMethods = new [] {
    new { Name = "Error", Action = new Action<LogAction>(Error), Verbosity = Verbosity.Quiet },
    new { Name = "Warning", Action = new Action<LogAction>(Warning), Verbosity = Verbosity.Minimal },
    new { Name = "Information", Action = new Action<LogAction>(Information), Verbosity = Verbosity.Normal },
    new { Name = "Verbose", Action = new Action<LogAction>(Verbose), Verbosity = Verbosity.Verbose },
    new { Name = "Debug", Action = new Action<LogAction>(Debug), Verbosity = Verbosity.Diagnostic }
};

var loggingAliasesTask = Task("Cake.Common.Diagnostics.LoggingAliases");

Array.ForEach(
    verbosities,
    verbosity => Array.ForEach(
                            logStringMethods,
                            logStringMethod => loggingAliasesTask.IsDependentOn(
                                    Task(string.Format("Cake.Common.Diagnostics.LoggingAliases.String.{0}.{1}", verbosity, logStringMethod.Name))
                                        .Does(() =>
                                    {
                                        var originalVerbosity = Context.Log.Verbosity;
                                        try
                                        {
                                            // Given
                                            Context.Log.Verbosity = verbosity;

                                            // When
                                            logStringMethod.Action("Cake.Common.Diagnostics.LoggingAliases.String.{0}.{1}", new object[] { verbosity, logStringMethod.Name });
                                        }
                                        finally
                                        {
                                            Context.Log.Verbosity = originalVerbosity;
                                        }
                                    }).Task.Name
                        ))
);

Array.ForEach(
    verbosities,
    verbosity => Array.ForEach(
                            logActionMethods,
                            logActionMethod => loggingAliasesTask.IsDependentOn(
                                    Task(string.Format("Cake.Common.Diagnostics.LoggingAliases.LogAction.{0}.{1}", verbosity, logActionMethod.Name))
                                        .Does(() =>
                                    {
                                        var originalVerbosity = Context.Log.Verbosity;
                                        try
                                        {
                                            // Given
                                            Context.Log.Verbosity = verbosity;
                                            bool called = false;
                                            LogAction action = entry=>{
                                                called = true;
                                            };

                                            // When
                                            logActionMethod.Action(action);

                                            // Then
                                            Assert.True(called || verbosity<logActionMethod.Verbosity);
                                        }
                                        finally
                                        {
                                            Context.Log.Verbosity = originalVerbosity;
                                        }
                                    }).Task.Name
                        ))
);