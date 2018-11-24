using Cake.Core.IO;

namespace Cake.Common.Diagnostics
{
    /// <summary>
    /// Represents a caller information.
    /// </summary>
    public sealed class ScriptCallerInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptCallerInfo"/> class.
        /// </summary>
        /// <param name="memberName">The member name.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        public ScriptCallerInfo(string memberName, FilePath sourceFilePath, int sourceLineNumber)
        {
            MemberName = memberName;
            SourceFilePath = sourceFilePath;
            SourceLineNumber = sourceLineNumber;
        }

        /// <summary>
        /// Gets the method or property name of the caller to the method.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets the full path of the source file that contains the caller.
        /// </summary>
        public FilePath SourceFilePath { get; }

        /// <summary>
        /// Gets the line number in the source file at which the method is called.
        /// </summary>
        public int SourceLineNumber { get; }

        /// <summary>
        /// Returns a <see cref="System.String" /> containing the full path of the source file and the line number in the source file at which the method is called.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> containing the full path of the source file and the line number in the source file at which the method is called.
        /// </returns>
        public override string ToString() => $"{SourceFilePath}:{SourceLineNumber}";
    }
}
