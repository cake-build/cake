﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Cake.Core.Diagnostics;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// The processor extension finder.
    /// </summary>
    public sealed class ProcessorExtensionFinder : IProcessorExtensionFinder
    {
        private readonly ICakeLog _log;
        private readonly ICakeEnvironment _cakeEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorExtensionFinder"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="environment">The <see cref="ICakeEnvironment"/>.</param>
        public ProcessorExtensionFinder(ICakeLog log, ICakeEnvironment environment)
        {
            _log = log;
            _cakeEnvironment = environment;
        }

        /// <summary>
        /// Finds <see cref="IProcessorExtension"/> in the provided assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to find <see cref="IProcessorExtension"/> in.</param>
        /// <returns>The <see cref="IProcessorExtension"/> that were found.</returns>
        public IReadOnlyList<IProcessorExtension> FindProcessorExtensions(IEnumerable<Assembly> assemblies)
        {
            var result = new List<IProcessorExtension>();
            if (assemblies != null)
            {
                foreach (var reference in assemblies)
                {
                    try
                    {
                        foreach (var type in reference.DefinedTypes)
                        {
                            if (!type.IsAbstract && type.IsAssignableFrom(typeof(IProcessorExtension)))
                            {
                                // Create a instance of the processor extension.
                                var instance = (IProcessorExtension)Activator.CreateInstance(typeof(IProcessorExtension), _cakeEnvironment);
                                result.Add(instance);
                            }
                        }
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        HashSet<string> notFound = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        foreach (Exception loaderException in ex.LoaderExceptions)
                        {
                            _log.Debug(loaderException.Message);
                            FileNotFoundException fileNotFoundException = loaderException as FileNotFoundException;
                            if (fileNotFoundException != null)
                            {
                                if (!notFound.Contains(fileNotFoundException.FileName))
                                {
                                    notFound.Add(fileNotFoundException.FileName);
                                }

                                if (!string.IsNullOrEmpty(fileNotFoundException.FusionLog))
                                {
                                    _log.Debug("Fusion Log:");
                                    _log.Debug(fileNotFoundException.FusionLog);
                                }
                            }
                            _log.Debug(string.Empty);
                        }

                        foreach (var file in notFound)
                        {
                            _log.Warning("Could not load {0} (missing {1})", reference.Location, file);
                        }
                    }
                    catch (FileNotFoundException ex)
                    {
                        _log.Warning("Could not load {0} (missing {1}))", reference.Location, ex.FileName);
                    }
                }
            }
            return result;
        }
    }
}
