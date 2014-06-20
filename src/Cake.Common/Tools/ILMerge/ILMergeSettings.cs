using Cake.Core.IO;

namespace Cake.Common.Tools.ILMerge
{
    public sealed class ILMergeSettings
    {
        public bool Internalize { get; set; }
        public TargetKind TargetKind { get; set; }
        public FilePath ToolPath { get; set; }

        public ILMergeSettings()
        {
            Internalize = false;
            TargetKind = TargetKind.Default;
        }
    }
}
