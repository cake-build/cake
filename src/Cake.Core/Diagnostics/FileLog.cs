using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Cake.Core.IO;

using File = Cake.Core.IO.File;

namespace Cake.Core.Diagnostics
{
    /// <summary>
    ///     Logger that logs to a specified file
    /// </summary>
    public sealed class FileLog : ICakeLog
    {
        private static readonly IEnumerable<LogLevel> _allLevels = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>();

        private readonly HashSet<LogLevel> _logLevels;

        private readonly object _lock;

        private readonly StreamWriter _fileStream;

        private readonly LogEntryFormatterAction _formatter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileLog" /> class that will write to the specified file, with the
        ///     possibility of filtering out certain <see cref="LogLevel" />
        /// </summary>
        /// <param name="path">The file where log entries will be placed</param>
        /// <param name="levels">The <see cref="LogLevel" /> levels to log, if empty all log levels will be captured.</param>
        public FileLog(FilePath path, params LogLevel[] levels)
            : this(path, DefaultFormatter, levels)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileLog" /> class that will write to the specified file, with the
        ///     possibility of filtering out certain <see cref="LogLevel" /> and with the ability to have a custom format
        /// </summary>
        /// <param name="path">The file where log entries will be placed</param>
        /// <param name="entryFormatter">
        ///     A <see cref="LogEntryFormatterAction" /> delegate that will serve to format the log
        ///     entries in the specified file
        /// </param>
        /// <param name="levels">The <see cref="LogLevel" /> levels to log, if empty all log levels will be captured.</param>
        public FileLog(FilePath path, LogEntryFormatterAction entryFormatter, params LogLevel[] levels)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (entryFormatter == null)
            {
                throw new ArgumentNullException("entryFormatter");
            }

            File file = new File(path);
            _fileStream = new StreamWriter(file.Open(FileMode.Append, FileAccess.Write, FileShare.Read));

            _lock = new object();

            IEnumerable<LogLevel> captureLevels = levels;
            if (levels == null || levels.Length == 0)
            {
                captureLevels = _allLevels;
            }

            _logLevels = new HashSet<LogLevel>(captureLevels);

            _formatter = entryFormatter;

            Verbosity = Verbosity.Normal;
        }

        /// <summary>
        ///     Gets or sets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        ///     Writes the text representation of the specified array of objects to the
        ///     log using the specified verbosity, log level and format information.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The log level.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            if (verbosity > Verbosity || !_logLevels.Contains(level))
            {
                return;
            }

            string entry = _formatter(verbosity, level, string.Format(format, args));

            lock (_lock)
            {
                _fileStream.WriteLine(entry);
            }
        }

        private static string DefaultFormatter(Verbosity verbosity, LogLevel level, string message, params object[] args)
        {
            string prefix;

            switch (level)
            {
                case LogLevel.Fatal:
                    prefix = "[FATAL] ";
                    break;
                case LogLevel.Error:
                    prefix = "[ERROR] ";
                    break;
                case LogLevel.Warning:
                    prefix = "[WARNING] ";
                    break;
                default:
                    prefix = string.Empty;
                    break;
            }

            return string.Concat(string.Format("[{0:T}]  {1}", DateTime.Now, prefix), string.Format(message, args));
        }
    }
}