#load "./PathEqualityComparer.cake"
#load "./ProjectFileEqualityComparer.cake"
#load "./ProjectAssemblyReferenceEqualityComparer.cake"
#load "./ProjectReferenceEqualityComparer.cake"

public class ProjectParserResultEqualityComparer : IEqualityComparer<ProjectParserResult>
{
    public static ProjectParserResultEqualityComparer Comparer = new ProjectParserResultEqualityComparer();

    public bool Equals(ProjectParserResult x, ProjectParserResult y)
    {
        if (object.ReferenceEquals(x, y))
        {
            return true;
        }
        if (x==null||y==null)
        {
            return false;
        }
        return  StringComparer.Ordinal.Equals(x.Configuration, y.Configuration) &&
                StringComparer.Ordinal.Equals(x.Platform, y.Platform) &&
                StringComparer.Ordinal.Equals(x.ProjectGuid, y.ProjectGuid) &&
                StringComparer.Ordinal.Equals(x.OutputType, y.OutputType) &&
                PathEqualityComparer.Comparer.Equals(x.OutputPath, y.OutputPath) &&
                StringComparer.Ordinal.Equals(x.RootNameSpace, y.RootNameSpace) &&
                StringComparer.Ordinal.Equals(x.AssemblyName, y.AssemblyName) &&
                StringComparer.Ordinal.Equals(x.TargetFrameworkVersion, y.TargetFrameworkVersion) &&
                StringComparer.Ordinal.Equals(x.TargetFrameworkProfile, y.TargetFrameworkProfile) &&
                Enumerable.SequenceEqual(x.Files, y.Files, ProjectFileEqualityComparer.Comparer) &&
                Enumerable.SequenceEqual(x.References, y.References, ProjectAssemblyReferenceEqualityComparer.Comparer) &&
                Enumerable.SequenceEqual(x.ProjectReferences, y.ProjectReferences, ProjectReferenceEqualityComparer.Comparer);
    }

    private static  IEnumerable<int> GetHashCodes(IEnumerable<ProjectFile> files)
    {
        if (files == null)
            yield break;

        foreach(var file in files)
        {
            yield return ProjectFileEqualityComparer.Comparer.GetHashCode(file);
        }
    }

    private static  IEnumerable<int> GetHashCodes(IEnumerable<ProjectAssemblyReference> references)
    {
        if (references == null)
            yield break;

        foreach(var reference in references)
        {
            yield return ProjectAssemblyReferenceEqualityComparer.Comparer.GetHashCode(reference);
        }
    }

    private static  IEnumerable<int> GetHashCodes(IEnumerable<ProjectReference> references)
    {
        if (references == null)
            yield break;

        foreach(var reference in references)
        {
            yield return ProjectReferenceEqualityComparer.Comparer.GetHashCode(reference);
        }
    }

    public int GetHashCode(ProjectParserResult obj)
    {
        if (obj == null)
            return 0;

        return new [] {
            StringComparer.Ordinal.GetHashCode(obj.Configuration),
            StringComparer.Ordinal.GetHashCode(obj.Platform),
            StringComparer.Ordinal.GetHashCode(obj.ProjectGuid),
            StringComparer.Ordinal.GetHashCode(obj.OutputType),
            PathEqualityComparer.Comparer.GetHashCode(obj.OutputPath),
            StringComparer.Ordinal.GetHashCode(obj.RootNameSpace),
            StringComparer.Ordinal.GetHashCode(obj.AssemblyName),
            StringComparer.Ordinal.GetHashCode(obj.TargetFrameworkVersion),
            StringComparer.Ordinal.GetHashCode(obj.TargetFrameworkProfile),
        }.Concat(GetHashCodes(obj.Files))
        .Concat(GetHashCodes(obj.References))
        .Concat(GetHashCodes(obj.ProjectReferences))
        .ToArray().GetHashCode();
    }
}