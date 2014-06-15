using Cake.Core.IO;

namespace Cake.Common.NUnit
{
    public sealed class NUnitSettings
    {
        public FilePath ToolPath { get; set; }
        public bool ShadowCopy { get; set; }

        public NUnitSettings()
        {
            ShadowCopy = true;
        }
    }
}
