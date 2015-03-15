using System;
using System.Collections;
using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Contains information about tasks that were executed in a script.
    /// </summary>
    public sealed class CakeReport : IEnumerable<CakeReportEntry>
    {
        private readonly List<CakeReportEntry> _report;

        /// <summary>
        /// Gets a value indicating whether the report is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this report is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get { return _report.Count == 0; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReport"/> class.
        /// </summary>
        public CakeReport()
        {
            _report = new List<CakeReportEntry>();
        }

        /// <summary>
        /// Adds a task result to the report.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="span">The span.</param>
        public void Add(string task, TimeSpan span)
        {
            _report.Add(new CakeReportEntry(task, span));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<CakeReportEntry> GetEnumerator()
        {
            return _report.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
