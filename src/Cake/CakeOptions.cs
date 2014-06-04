using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake
{
    public sealed class CakeOptions
    {
        public Verbosity Verbosity { get; set; }
        public FilePath Script { get; set; }

        public CakeOptions()
        {
            Verbosity = Verbosity.Normal;
        }
    }
}
