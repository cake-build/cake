using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors;
using Newtonsoft.Json;

namespace Cake.LoadRemote.Module
{
    public sealed class LoadRemoteScriptRunnerExtension : ScriptRunnerExtension
    {
        public LoadRemoteScriptRunnerExtension(IProcessorExtension processorExtension, ICakeContext cakeContext, ICakeEnvironment environment, ICakeLog cakeLog, IScriptProcessor scriptProcessor)
            : base(processorExtension, cakeContext, environment, cakeLog, scriptProcessor)
        {
        }

        public override void DoInstall(IEnumerable<object> values, ref ScriptAnalyzerResult result, IScriptAnalyzerContext scriptAnalyzerContext, DirectoryPath toolsPath)
        {
            var items = values.OfType<PackageReference>().ToList();
            foreach (var packageReference in items)
            {
                var files = ScriptProcessor.InstallPackage(packageReference, ".cake", toolsPath).ToList();
                if (!files.Any())
                {
                    const string format = "Failed to install nuget script '{0}'.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, packageReference.Package);
                    throw new CakeException(message);
                }
                
                var packageDirectory = files.First().Path.GetDirectory();
                ConfigEntity config = GetConfig(packageDirectory);
                files = ReArrange(files, config);
                var keyValues = files.Select(f => new KeyValuePair<PackageReference, FilePath>(packageReference, f.Path));
                RecursiveInstallNugetScripts(ref result, keyValues, scriptAnalyzerContext, toolsPath);
            }
        }

        /// <summary>
        /// Rearrange file list based on config.
        /// </summary>
        /// <param name="files">The files to process.</param>
        /// <param name="config">The config.</param>
        /// <returns>A rearranged file list.</returns>
        private List<IFile> ReArrange(List<IFile> files, ConfigEntity config)
        {
            if (config != null && config.FileOrder.Any())
            {
                var sortedFiles = new List<IFile>();
                foreach (var file in config.FileOrder)
                {
                    var item = files.First(x => x.Path.FullPath.EndsWith(file, StringComparison.OrdinalIgnoreCase));
                    sortedFiles.Add(item);
                }

                return sortedFiles;
            }

            return files;
        }

        /// <summary>
        /// Get the <see cref="ConfigEntity"/>.
        /// </summary>
        /// <param name="path">The directory which contains the config.json file.</param>
        /// <returns>A <see cref="ConfigEntity"/> or null if no file is found or if the file is empty.</returns>
        /// <exception cref="CakeException">Throws if config.json is not in a valid json format.</exception>
        private ConfigEntity GetConfig(DirectoryPath path)
        {
            var configFilePath = path.CombineWithFilePath(new FilePath("config.json"));
            var configFile = CakeContext.FileSystem.GetFile(configFilePath);

            if (configFile.Exists)
            {
                using (var stream = configFile.Open(FileMode.Open, FileAccess.Read))
                using (var reader = new StreamReader(stream))
                {
                    var configValue = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(configValue))
                    {
                        try
                        {
                            var config = JsonConvert.DeserializeObject<ConfigEntity>(configValue);
                            return config;
                        }
                        catch (JsonReaderException ex)
                        {
                            const string format = "[{0}] config.json file is not in a valid json format.";
                            var message = string.Format(CultureInfo.InvariantCulture, format, path.FullPath);
                            throw new CakeException(message, ex);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Install nuget scripts recursively.
        /// </summary>
        /// <param name="result">The current executing <see cref="ScriptAnalyzerResult"/></param>
        /// <param name="scriptImports">The nuget script to install</param>
        /// <param name="scriptAnalyzerContext">The current executing <see cref="IScriptAnalyzerContext"/></param>
        /// <param name="nugetScriptPath">Installation path for nuget scripts, this is the path to tools</param>
        private void RecursiveInstallNugetScripts(ref ScriptAnalyzerResult result,
            IEnumerable<KeyValuePair<PackageReference, FilePath>> scriptImports,
            IScriptAnalyzerContext scriptAnalyzerContext,
            DirectoryPath nugetScriptPath)
        {
            scriptImports = scriptImports.ToList();
            foreach (var item in scriptImports)
            {
                var file = item.Value;

                // analyze and add the file lines to the current script
                scriptAnalyzerContext.Analyze(file);

                // We need to wrap the ScriptAnalyzerResult to not acess the scriptAnalyzerContext directly as that errors out.
                var copyResult = new ScriptAnalyzerResult(scriptAnalyzerContext.Script, scriptAnalyzerContext.Lines);

                // add all tools, addins, namespaces etc from nugetScriptResult to result.
                result.Tools.AddRange(copyResult.Tools);
                result.Addins.AddRange(copyResult.Addins);

                foreach (var usingAliase in copyResult.UsingAliases)
                {
                    result.UsingAliases.Add(usingAliase);
                }

                foreach (var reference in copyResult.References)
                {
                    result.References.Add(reference);
                }

                foreach (var @namespace in copyResult.Namespaces)
                {
                    result.Namespaces.Add(@namespace);
                }

                // Get any childrens to result.
                var childItems = copyResult.ProcessorValues.Get<PackageReference>(ProcessorExtension).ToList();
                if (childItems.Any())
                {
                    DoInstall(childItems, ref result, scriptAnalyzerContext, nugetScriptPath);
                }
            }

            if (scriptImports.Any())
            {
                // Re-arrange parent scripts
                var siblings = scriptImports.Skip(1).Select(x => x.Value.FullPath);
                RearrangeNugetScripts(ref result, scriptImports.First(), siblings);
            }
        }

        /// <summary>
        /// Rearrange the nuget script to its rightfull place
        /// this will also move siblings belonging to the item.
        /// </summary>
        /// <param name="result">The current result</param>
        /// <param name="item">The item to process</param>
        /// <param name="nugetScriptSet">The nuge script siblings to the processed item</param>
        private void RearrangeNugetScripts(ref ScriptAnalyzerResult result, KeyValuePair<PackageReference, FilePath> item, IEnumerable<string> nugetScriptSet)
        {
            var file = item.Value;
            var lineCopy = result.Lines.ToList();
            var lineMarker = lineCopy.FirstOrDefault(x => x.StartsWith("#line") && x.Contains(file.FullPath));
            if (lineMarker != null)
            {
                var startIndex = lineCopy.IndexOf(lineMarker);
                var prevLineDirective = lineCopy.LastOrDefault(x => x.StartsWith("#line") && !x.Contains(file.FullPath) && nugetScriptSet != null && !nugetScriptSet.Any(x.Contains));
                var prevLineDirectiveIndex = lineCopy.IndexOf(prevLineDirective);

                // Read all of the lines belonging to the imported script
                var amountToTake = lineCopy.Count - startIndex;
                var nugetScriptLines = lineCopy.GetRange(startIndex, amountToTake);

                // Remove the copied lines
                lineCopy.RemoveRange(startIndex, amountToTake);

                // Get the nuget script directive declaration index
                var nugetScriptMarker = lineCopy.FirstOrDefault(x =>
                    x.Contains(Constants.DirectiveName) &&
                    x.Contains(item.Key.OriginalString));
                var nugetScriptIndex = lineCopy.IndexOf(nugetScriptMarker) + 1;

                // Performe the actuall move of the imported script
                lineCopy.InsertRange(nugetScriptIndex, nugetScriptLines);

                // Add the previus #line marker back
                var lineDirectiveIndex = nugetScriptIndex + nugetScriptLines.Count;
                if (prevLineDirective != null)
                {
                    var prevLine = prevLineDirective.Split(null);

                    // Calculate the new line number for the previus #line directive. (note: we need to do -1 becuse of line 270 nugetScriptIndex has +1)
                    var calculateFromBegining = lineCopy.Skip(prevLineDirectiveIndex).TakeWhile(x => !x.Equals(lineMarker)).Count() - 1;

                    // Ensure a minimum number 1
                    calculateFromBegining = Math.Max(1, calculateFromBegining);

                    // Set the line number
                    prevLine[1] = calculateFromBegining + string.Empty;

                    // Add a new #line directive with a new line number
                    var newValue = string.Join(" ", prevLine);
                    lineCopy.Insert(lineDirectiveIndex, newValue);

                    // Check if the line directive is the last line and if so remove (im sure there is much better ways to do this but im tired)
                    var lastItem = lineCopy.Last();
                    if (lastItem.Equals(newValue))
                    {
                        lineCopy.RemoveAt(lineCopy.Count - 1);
                    }
                }

                // Persist changes
                result = new ScriptAnalyzerResult(result.Script, lineCopy);
            }
        }
    }
}
