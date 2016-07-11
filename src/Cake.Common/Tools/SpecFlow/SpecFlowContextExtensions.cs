// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.SpecFlow
{
    internal static class SpecFlowContextExtensions
    {
        internal static ProcessArgumentBuilder GetArguments(this SpecFlowContext context,
            FilePath projectFile,
            ICakeEnvironment environment)
        {
            ProcessArgumentBuilder builder;
            var executable = context.FilePath.ToString();

            if (executable.IndexOf("mstest", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                builder = context.GetMSTestArguments(projectFile, environment);
            }
            else if (executable.IndexOf("nunit", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                builder = context.GetNUnitArguments(projectFile, environment);
            }
            else if (executable.IndexOf("xunit", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                builder = context.GetXUnitArguments(projectFile, environment);
            }
            else
            {
                throw new CakeException(string.Concat("Unsupported tool ", executable, "."));
            }

            return builder;
        }

        internal static ProcessArgumentBuilder GetMSTestArguments(this SpecFlowContext context,
            FilePath projectFile,
            ICakeEnvironment environment)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("mstestexecutionreport");

            // Set the project file
            builder.AppendQuoted(projectFile.MakeAbsolute(environment).FullPath);

            var arguments = context.RenderArguments();

            // Set the test result
            var testResultMatch = Regex.Match(arguments, "\\/resultsfile:\\s*((?:\".+?\"|[^\\s]+))", RegexOptions.IgnoreCase);

            if (testResultMatch.Success && testResultMatch.Groups[1].Success)
            {
                builder.AppendSwitch("/testResult", ":", testResultMatch.Groups[1].Value);
            }
            else
            {
                throw new CakeException("MSTest must contain argument \"/resultsfile:<filename>\"");
            }

            return builder;
        }

        internal static ProcessArgumentBuilder GetNUnitArguments(this SpecFlowContext context,
            FilePath projectFile,
            ICakeEnvironment environment)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("nunitexecutionreport");

            // Set the project file
            builder.AppendQuoted(projectFile.MakeAbsolute(environment).FullPath);

            var arguments = context.RenderArguments();

            // Set the xml test result
            var xmlTestResultMatch = Regex.Match(arguments, "\"--result=([^;]+);format=nunit2", RegexOptions.IgnoreCase);

            if (xmlTestResultMatch.Success && xmlTestResultMatch.Groups[1].Success)
            {
                builder.AppendSwitch("/xmlTestResult", ":", xmlTestResultMatch.Groups[1].Value.Quote());
            }
            else
            {
                throw new CakeException("NUnit3 must contain argument \"--result=<filename>;format=nunit2\"");
            }

            // Set the test output
            var testOutputMatch = Regex.Match(arguments, "\"--out=([^\"]+)\"", RegexOptions.IgnoreCase);

            if (testOutputMatch.Success && testOutputMatch.Groups[1].Success)
            {
                builder.AppendSwitch("/testOutput", ":", testOutputMatch.Groups[1].Value.Quote());
            }
            else
            {
                throw new CakeException("NUnit3 must contain argument \"--out=<filename>\"");
            }

            // Check for labels switch
            var testLabelsMatch = Regex.Match(arguments, "--labels=All", RegexOptions.IgnoreCase);

            if (!testLabelsMatch.Success)
            {
                throw new CakeException("NUnit3 must contain argument \"--labels=All\"");
            }

            return builder;
        }

        internal static ProcessArgumentBuilder GetXUnitArguments(this SpecFlowContext context,
            FilePath projectFile,
            ICakeEnvironment environment)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("nunitexecutionreport");

            // Set the project file
            builder.AppendQuoted(projectFile.MakeAbsolute(environment).FullPath);

            var arguments = RenderArguments(context);

            // Set the xml test result
            var xmlTestResultMatch = Regex.Match(arguments, "-nunit\\s+((?:\".+?\"|.*))", RegexOptions.IgnoreCase);

            if (xmlTestResultMatch.Success && xmlTestResultMatch.Groups[1].Success)
            {
                builder.AppendSwitch("/xmlTestResult", ":", xmlTestResultMatch.Groups[1].Value);
            }
            else
            {
                throw new CakeException("XUnit2 must contain argument \"-nunit <filename>\"");
            }

            return builder;
        }

        private static string RenderArguments(this SpecFlowContext context)
        {
            // The arguments to the target application.
            if (context.Settings == null || context.Settings.Arguments == null)
            {
                throw new CakeException("No arguments were found for tool.");
            }

            var arguments = context.Settings.Arguments.Render();

            if (string.IsNullOrWhiteSpace(arguments))
            {
                throw new CakeException("No arguments were found for tool.");
            }

            return arguments;
        }
    }
}
