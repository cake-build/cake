// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.DotNet.ProjectModel;

namespace Cake.Frosting.Cli.Project
{
    internal sealed class ProjectBuilder
    {
        public int Build(ProjectContext context)
        {
            try
            {
                Console.WriteLine("Compiling build script ({0})...", context.ProjectFile.Name);

                var command = Command.CreateDotNet("build", new[] { "--configuration", "Release" });
                command.WorkingDirectory(context.ProjectDirectory);

                var writer = new StringWriter();
                var result = command.ForwardStdErr(writer).ForwardStdOut(new StringWriter()).Execute();
                if (result.ExitCode != 0)
                {
                    Console.WriteLine();
                    Console.Error.Write("ERROR: ");
                    Console.Error.WriteLine(writer.ToString().Trim());
                    Console.WriteLine();
                }

                return result.ExitCode;
            }
            catch (CommandUnknownException exception)
            {
                throw new FrostingException("Could not build project since dotnet tool didn't know about the command.", exception);
            }
        }
    }
}
