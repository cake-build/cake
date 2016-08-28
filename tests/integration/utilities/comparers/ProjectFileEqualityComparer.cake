#load "./PathEqualityComparer.cake"

public class ProjectFileEqualityComparer : IEqualityComparer<ProjectFile>
{
    public static ProjectFileEqualityComparer Comparer = new ProjectFileEqualityComparer();

    public bool Equals(ProjectFile x, ProjectFile y)
    {
        if (object.ReferenceEquals(x, y))
        {
            return true;
        }
        if (x==null||y==null)
        {
            return false;
        }
        return  (x.Compile == y.Compile) &&
                PathEqualityComparer.Comparer.Equals(x.FilePath, y.FilePath) &&
                PathEqualityComparer.Comparer.Equals(x.RelativePath, y.RelativePath);
    }

    public int GetHashCode(ProjectFile obj)
    {
        if (obj == null)
            return 0;

        return new [] {
          obj.Compile.GetHashCode(),
          PathEqualityComparer.Comparer.GetHashCode(obj.FilePath),
          PathEqualityComparer.Comparer.GetHashCode(obj.RelativePath)
        }.GetHashCode();
    }
}
