using System.Collections.Generic;

namespace Cake.Core.IO
{
    public interface IGlobber
    {
        IEnumerable<Path> Match(string pattern);
    }
}
