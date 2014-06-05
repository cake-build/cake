namespace Cake.Core
{
    public interface ICakeArguments
    {
        bool HasArgument(string key);
        string GetArgument(string key);
    }
}
