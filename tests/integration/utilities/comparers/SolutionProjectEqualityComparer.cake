#load "./PathEqualityComparer.cake"

public class SolutionProjectEqualityComparer : IEqualityComparer<SolutionProject>
{
    public static SolutionProjectEqualityComparer Comparer = new SolutionProjectEqualityComparer();

    public bool Equals(SolutionProject x, SolutionProject y)
    {
        if (object.ReferenceEquals(x, y))
        {
            return true;
        }
        if (x==null||y==null)
        {
            return false;
        }
        return  StringComparer.Ordinal.Equals(x.Id, y.Id) &&
                StringComparer.Ordinal.Equals(x.Name, x.Name) &&
                SolutionProjectEqualityComparer.Comparer.Equals(x.Parent, y.Parent) &&
                PathEqualityComparer.Comparer.Equals(x.Path, y.Path) &&
                StringComparer.Ordinal.Equals(x.Type, y.Type);
    }

    public int GetHashCode(SolutionProject obj)
    {
        if (obj==null) return 0;
        return new [] {
            StringComparer.Ordinal.GetHashCode(obj.Id),
            StringComparer.Ordinal.GetHashCode(obj.Name),
            SolutionProjectEqualityComparer.Comparer.GetHashCode(obj.Parent),
            PathEqualityComparer.Comparer.GetHashCode(obj.Path),
            StringComparer.Ordinal.GetHashCode(obj.Type),
        }.GetHashCode();
    }
}