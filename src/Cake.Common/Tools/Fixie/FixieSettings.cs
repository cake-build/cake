using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cake.Core.IO;

namespace Cake.Common.Tools.Fixie
{
    /// <summary>
    /// Contains settings used by <see cref="FixieRunner" />.
    /// </summary>
    public sealed class FixieSettings
    {
        private readonly ICollection<KeyValuePair<string, string>> _options = new Collection<KeyValuePair<string, string>>();

        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>
        /// The tool path. Defaults to <c>./tools/**/Fixie.Console.exe</c>.
        /// </value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets the file to be used to output NUnit style of XML results.
        /// </summary>
        /// <value>
        /// The name of the file to write NUnit style of results.
        /// </value>
        public FilePath NUnitXml { get; set; }

        /// <summary>
        /// Gets or sets the file to be used to output xUnit style of XML results.
        /// </summary>
        /// <value>
        /// The name of the file to write xUnit style of results.
        /// </value>
        public FilePath XUnitXml { get; set; }

        /// <summary>
        /// Gets or sets the the option to force TeamCity-formatted output on or off.
        /// </summary>
        /// <value>
        /// The value of TeamCity-formatted output. Either "on" or "off".
        /// </value>
        public TeamCityOutput? TeamCity { get; set; }

        /// <summary>
        /// Gets the collection of Options as KeyValuePairs.
        /// </summary>
        /// <value>
        /// The collection of keys and values.
        /// </value>
        public ICollection<KeyValuePair<string, string>> Options
        {
            get { return _options; }
        }

        /// <summary>
        /// Adds a custom Fixie Option to the settings.
        /// </summary>
        /// <param name="key">The key of the Option</param>
        /// <param name="value">The value of the Option</param>
        /// <returns>The instance of <see cref="FixieSettings"/> that the Option was added to.</returns>
        public FixieSettings WithOption(string key, string value)
        {
            _options.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }
    }
}