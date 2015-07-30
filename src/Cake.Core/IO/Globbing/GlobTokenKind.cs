///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Cake.Core.IO.Globbing
{
    internal enum GlobTokenKind
    {
        Wildcard,
        CharacterWildcard,
        DirectoryWildcard,
        PathSeparator,
        Identifier,
        WindowsRoot,
        EndOfText
    }
}