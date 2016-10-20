#load "./SolutionProjectEqualityComparer.cake"

public class SolutionParserResultEqualityComparer : IEqualityComparer<SolutionParserResult>
{
    public static SolutionParserResultEqualityComparer Comparer = new SolutionParserResultEqualityComparer();

    public bool Equals(SolutionParserResult x, SolutionParserResult y)
    {
        if (object.ReferenceEquals(x, y))
        {
            return true;
        }
        if (x==null||y==null)
        {
            return false;
        }
        return  StringComparer.Ordinal.Equals(x.Version, y.Version) &&
                StringComparer.Ordinal.Equals(x.VisualStudioVersion, y.VisualStudioVersion) &&
                StringComparer.Ordinal.Equals(x.MinimumVisualStudioVersion, y.MinimumVisualStudioVersion) &&
                Enumerable.SequenceEqual(x.Projects, y.Projects, SolutionProjectEqualityComparer.Comparer);
    }

    private static  IEnumerable<int> GetHashCodes(IEnumerable<SolutionProject> projects)
    {
        if (projects == null)
            yield break;

        foreach(var project in projects)
        {
            yield return SolutionProjectEqualityComparer.Comparer.GetHashCode(project);
        }
    }

    public int GetHashCode(SolutionParserResult obj)
    {
        if (obj == null)
            return 0;

        return new [] {
            StringComparer.Ordinal.GetHashCode(obj.Version),
            StringComparer.Ordinal.GetHashCode(obj.VisualStudioVersion),
            StringComparer.Ordinal.GetHashCode(obj.MinimumVisualStudioVersion),
        }.Concat(GetHashCodes(obj.Projects))
        .ToArray().GetHashCode();
    }
}