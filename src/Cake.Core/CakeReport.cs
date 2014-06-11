using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Core
{
    public sealed class CakeReport : IEnumerable<KeyValuePair<string, TimeSpan>>
    {
        private readonly IDictionary<string, TimeSpan> _report;

        public bool IsEmpty
        {
            get { return _report.Count == 0; }
        }

        public CakeReport()
        {
            _report = new Dictionary<string, TimeSpan>(StringComparer.OrdinalIgnoreCase);
        }

        public void Add(string task, TimeSpan span)
        {
            _report.Add(task, span);
        }

        public IEnumerator<KeyValuePair<string, TimeSpan>> GetEnumerator()
        {
            return _report.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
