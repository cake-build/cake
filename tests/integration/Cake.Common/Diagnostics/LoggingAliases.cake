#load "./../../utilities/xunit.cake"

var verbosities = Enum.GetValues(typeof(Cake.Core.Diagnostics.Verbosity))
                    .Cast<Cake.Core.Diagnostics.Verbosity>()
                    .ToArray();

var logStringFormatMethods = new [] {
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

var logStringMethods = new [] {
    new { Name = "Error", Action = new Action<string>(Error)},
    new { Name = "Warning", Action = new Action<string>(Warning)},
    new { Name = "Information", Action = new Action<string>(Information)},
    new { Name = "Verbose", Action = new Action<string>(Verbose)},
    new { Name = "Debug", Action = new Action<string>(Debug)}
};

var logObjectMethods = new [] {
    new { Name = "Error", Action = new Action<object>(Error)},
    new { Name = "Warning", Action = new Action<object>(Warning)},
    new { Name = "Information", Action = new Action<object>(Information)},
    new { Name = "Verbose", Action = new Action<object>(Verbose)},
    new { Name = "Debug", Action = new Action<object>(Debug)}
};

var loggingAliasesTask = Task("Cake.Common.Diagnostics.LoggingAliases")
                            .IsDependentOn("Cake.Common.Diagnostics.LoggingAliases.GetCallerInfo");

Array.ForEach(
    verbosities,
    verbosity => Array.ForEach(
                            logStringFormatMethods,
                            logStringFormatMethod => loggingAliasesTask.IsDependentOn(
                                    Task(string.Format("Cake.Common.Diagnostics.LoggingAliases.StringFormat.{0}.{1}", verbosity, logStringFormatMethod.Name))
                                        .Does(() =>
                                    {
                                        var originalVerbosity = Context.Log.Verbosity;
                                        try
                                        {
                                            // Given
                                            Context.Log.Verbosity = verbosity;

                                            // When
                                            logStringFormatMethod.Action("Cake.Common.Diagnostics.LoggingAliases.StringFormat.{0}.{1}", new object[] { verbosity, logStringFormatMethod.Name });
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
                                            logStringMethod.Action(string.Format("Cake.Common.Diagnostics.LoggingAliases.String.{0}.{1}: {2}", verbosity, logStringMethod.Name, "{Bon}jour"));
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
                            logObjectMethods,
                            logObjectMethod => loggingAliasesTask.IsDependentOn(
                                    Task(string.Format("Cake.Common.Diagnostics.LoggingAliases.Object.{0}.{1}", verbosity, logObjectMethod.Name))
                                        .Does(() =>
                                    {
                                        var originalVerbosity = Context.Log.Verbosity;
                                        try
                                        {
                                            // Given
                                            Context.Log.Verbosity = verbosity;

                                            // When
                                            logObjectMethod.Action(new { Test = "Cake.Common.Diagnostics.LoggingAliases.Object", Verbosity = verbosity, Name = logObjectMethod.Name });
                                        }
                                        finally
                                        {
                                            Context.Log.Verbosity = originalVerbosity;
                                        }
                                    }).Task.Name
                        ))
);

Task("Cake.Common.Diagnostics.LoggingAliases.GetCallerInfo")
    .Does( ()=> {
    // Given
    ScriptCallerInfo callerInfoDSL;
    ScriptCallerInfo callerInfo;

    // When
    CallerInfoTest(out callerInfoDSL, out callerInfo);

    // Then
    Assert.Equal(callerInfo.MemberName, callerInfoDSL.MemberName);
    Assert.Equal(callerInfo.SourceFilePath.FullPath, callerInfoDSL.SourceFilePath.FullPath);
    Assert.Equal(callerInfo.SourceLineNumber, callerInfoDSL.SourceLineNumber);
});

public void CallerInfoTest(out ScriptCallerInfo callerInfoDSL, out ScriptCallerInfo callerInfo)
{
    callerInfoDSL = GetCallerInfo(); callerInfo = Context.GetCallerInfo();
}