#load "./PathEqualityComparer.cake"

public class ProjectAssemblyReferenceEqualityComparer : IEqualityComparer<ProjectAssemblyReference>
{
    public static ProjectAssemblyReferenceEqualityComparer Comparer = new ProjectAssemblyReferenceEqualityComparer();

    public bool Equals(ProjectAssemblyReference x, ProjectAssemblyReference y)
    {
        if (object.ReferenceEquals(x, y))
        {
            return true;
        }
        if (x==null||y==null)
        {
            return false;
        }
        return  StringComparer.Ordinal.Equals(x.Aliases, y.Aliases) &&
                StringComparer.Ordinal.Equals(x.FusionName, y.FusionName) &&
                PathEqualityComparer.Comparer.Equals(x.HintPath, y.HintPath) &&
                StringComparer.Ordinal.Equals(x.Include, y.Include) &&
                StringComparer.Ordinal.Equals(x.Name, y.Name) &&
                Nullable.Equals<bool>(x.Private, y.Private) &&
                Nullable.Equals<bool>(x.SpecificVersion , y.SpecificVersion);
    }

    public int GetHashCode(ProjectAssemblyReference obj)
    {
        if (obj == null)
            return 0;

        return new [] {
            StringComparer.Ordinal.GetHashCode(obj.Aliases),
            StringComparer.Ordinal.GetHashCode(obj.FusionName),
            PathEqualityComparer.Comparer.GetHashCode(obj.HintPath),
            StringComparer.Ordinal.GetHashCode(obj.Include),
            StringComparer.Ordinal.GetHashCode(obj.Name),
            obj.Private.GetHashCode(),
            obj.SpecificVersion.GetHashCode()
        }.GetHashCode();
    }
}