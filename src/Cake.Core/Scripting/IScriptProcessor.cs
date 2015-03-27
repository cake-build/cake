﻿using Cake.Core.IO;
using System;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script processor.
    /// </summary>
    public interface IScriptProcessor
    {
        /// <summary>
        /// Processes the specified script.
        /// </summary>
        /// <param name="path">The script path.</param>
        /// <param name="context">The context.</param>
        void Process(FilePath path, ScriptProcessorContext context);

        /// <summary>
        /// Processes the specified script from the web.
        /// </summary>
        /// <param name="url">The script's url.</param>
        /// <param name="context">The context.</param>
        void Process(Uri url, ScriptProcessorContext context);
    }
}
