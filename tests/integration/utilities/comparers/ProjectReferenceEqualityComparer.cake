public class ProjectReferenceEqualityComparer : IEqualityComparer<ProjectReference>
{
    public static ProjectReferenceEqualityComparer Comparer = new ProjectReferenceEqualityComparer();

    public bool Equals(ProjectReference x, ProjectReference y)
    {
        if (object.ReferenceEquals(x, y))
        {
            return true;
        }
        if (x==null||y==null)
        {
            return false;
        }
        return  PathEqualityComparer.Comparer.Equals(x.FilePath, y.FilePath) &&
                StringComparer.Ordinal.Equals(x.Name, y.Name) &&
                PathEqualityComparer.Comparer.Equals(x.Package, y.Package) &&
                StringComparer.Ordinal.Equals(x.Project, y.Project) &&
                PathEqualityComparer.Comparer.Equals(x.RelativePath , y.RelativePath);
    }

    public int GetHashCode(ProjectReference obj)
    {
        if (obj == null)
            return 0;

        return new [] {
                PathEqualityComparer.Comparer.GetHashCode(obj.FilePath),
                StringComparer.Ordinal.GetHashCode(obj.Name),
                PathEqualityComparer.Comparer.GetHashCode(obj.Package),
                StringComparer.Ordinal.GetHashCode(obj.Project),
                PathEqualityComparer.Comparer.GetHashCode(obj.RelativePath)
        }.GetHashCode();
    }
}