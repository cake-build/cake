using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Scripting;

namespace Cake.Scripting.Mono
{
    /// <summary>
    /// Mono script host proxy.
    /// </summary>
    public class MonoScriptHostProxy
    {
        public static IScriptHost ScriptHost { get; set; }
    }
}
