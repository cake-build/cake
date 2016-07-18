using System;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Represents a uri directive processor. used to process nuget: url's
    /// </summary>
    public interface IUriDirectiveProcessor
    {
        /// <summary>
        /// Specify the directive name for this processor.
        /// </summary>
        /// <returns>The directive name</returns>
        string GetDirectiveName();

        /// <summary>
        /// Called after the <see cref="UriDirectiveProcessor.Process"/> is done.
        /// </summary>
        /// <param name="context">The <see cref="IScriptAnalyzerContext"/></param>
        /// <param name="uri">The <see cref="Uri"/> result.</param>
        void AddToContext(IScriptAnalyzerContext context, Uri uri);
    }
}