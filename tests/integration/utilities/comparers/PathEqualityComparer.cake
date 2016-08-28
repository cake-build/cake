public class PathEqualityComparer : IEqualityComparer<Cake.Core.IO.Path>, IEqualityComparer<string>
{
    public static PathEqualityComparer Comparer = new PathEqualityComparer();
    private static StringComparer stringComparer { get; set; }

    public bool Equals(Cake.Core.IO.Path x, Cake.Core.IO.Path y)
    {
        if (object.ReferenceEquals(x, y))
        {
            return true;
        }
        if (x==null||y==null)
        {
            return false;
        }
        return  stringComparer.Equals(x.FullPath, y.FullPath);
    }

    public bool Equals(string x, string y)
    {
        if (object.ReferenceEquals(x, y))
        {
            return true;
        }
        if (x==null||y==null)
        {
            return false;
        }
        return  stringComparer.Equals(x, y);
    }

    public int GetHashCode(Cake.Core.IO.Path obj)
    {
        if (obj==null) return 0;
        return stringComparer.GetHashCode(obj.FullPath);
    }

    public int GetHashCode(string obj)
    {
        if (obj==null) return 0;
        return stringComparer.GetHashCode(obj);
    }


    public static void Initialize(ICakeContext context)
    {
        stringComparer = context.IsRunningOnWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
    }
}

PathEqualityComparer.Initialize(Context);