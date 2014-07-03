using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.WiX
{
    public sealed class CandleSettings
    {
        public Architecture? Architecture { get; set; }

        public IDictionary<string, string> Defines { get; set; }

        public IEnumerable<string> Extensions { get; set; }

        public bool FIPS { get; set; }

        public bool NoLogo { get; set; }

        public DirectoryPath OutputDirectory { get; set; }

        public bool Pedantic { get; set; }

        public bool ShowSourceTrace { get; set; }

        public bool Verbose { get; set; }

        public FilePath ToolPath { get; set; }
    }
}
