using Cake.Core.IO;

namespace Cake.Common.MSTest
{
    public sealed class MSTestSettings
    {
        public FilePath ToolPath { get; set; }
        public bool NoIsolation { get; set; }

        public MSTestSettings()
        {
            NoIsolation = true;
        }
    }
}