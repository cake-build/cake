using System;
using System.Runtime.CompilerServices;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Diagnostics
{
    /// <summary>
    /// Contains functionality related to diagnostics.
    /// </summary>
    [CakeAliasCategory("Diagnostics")]
    public static class ScriptCallerAliases
    {
        /// <summary>
        /// Performs script caller information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="memberName">The member name.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>A <see cref="ScriptCallerInfo"/> instance representing the caller information.</returns>
        /// <example>
        /// <code>
        /// var caller = GetCallerInfo();
        /// Information("Called from {0} at {1}:{2}",
        ///     caller.MemberName,
        ///     caller.SourceFilePath,
        ///     caller.SourceLineNumber);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static ScriptCallerInfo GetCallerInfo(
            this ICakeContext context,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            ArgumentNullException.ThrowIfNull(context);

            return new ScriptCallerInfo(memberName, sourceFilePath, sourceLineNumber);
        }
    }
}
