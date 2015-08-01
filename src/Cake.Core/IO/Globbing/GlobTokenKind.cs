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