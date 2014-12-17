using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.SignTool.Sign
{
    /// <summary>
    /// Contains functionality related to signing assemblies with PFX certificate
    /// </summary>
    [CakeAliasCategoryAttribute("Signing")]
    public static class SignToolSignAliases
    {
        /// <summary>
        /// Signs the specified assembly
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assembly">The target assembly.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Sign(this ICakeContext context, string assembly, SignToolSignSettings settings)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            Sign(context, new FilePath(assembly), settings);
        }

        /// <summary>
        /// Signs the specified assembly
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assembly">The target assembly.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Sign(this ICakeContext context, FilePath assembly, SignToolSignSettings settings)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            var paths = new[] {assembly};
            Sign(context, paths, settings);
        }

        /// <summary>
        /// Signs the specified assemblies
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The target assembly.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Sign(this ICakeContext context, IEnumerable<string> assemblies, SignToolSignSettings settings)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            var paths = assemblies.Select(p => new FilePath(p));
            Sign(context, paths, settings);
        }

        /// <summary>
        /// Signs the specified assemblies
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The target assembly.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Sign(this ICakeContext context, IEnumerable<FilePath> assemblies, SignToolSignSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var runner = new SignToolSignRunner(context.FileSystem, context.Environment, context.ProcessRunner);            
            Array.ForEach(
                assemblies.ToArray(),
                assembly=>runner.Run(assembly, settings)
                );
        }
    }
}
