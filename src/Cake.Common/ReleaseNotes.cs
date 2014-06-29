using System.Collections.Generic;
using System.Linq;

namespace Cake.Common
{
    public sealed class ReleaseNotes
    {
        private readonly string _version;
        private readonly List<string> _notes;

        public string Version
        {
            get { return _version; }
        }

        public IReadOnlyList<string> Notes
        {
            get { return _notes; }
        }

        public ReleaseNotes(string version, IEnumerable<string> notes)
        {
            _version = version;
            _notes = new List<string>(notes ?? Enumerable.Empty<string>());
        }
    }
}