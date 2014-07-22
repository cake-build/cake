using System.Collections.Generic;

namespace Cake.Arguments
{
    public interface IArgumentParser
    {
        CakeOptions Parse(IEnumerable<string> args);
    }
}