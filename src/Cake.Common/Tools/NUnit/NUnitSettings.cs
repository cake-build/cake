using Cake.Core.IO;

namespace Cake.Common.Tools.NUnit
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
