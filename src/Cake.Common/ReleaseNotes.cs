using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Common
{
    public sealed class ReleaseNotes
    {
        private readonly Version _version;
        private readonly List<string> _notes;

        public Version Version
        {
            get { return _version; }
        }

        public IReadOnlyList<string> Notes
        {
            get { return _notes; }
        }

        public ReleaseNotes(Version version, IEnumerable<string> notes)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }
            _version = version;
            _notes = new List<string>(notes ?? Enumerable.Empty<string>());
        }
    }
}