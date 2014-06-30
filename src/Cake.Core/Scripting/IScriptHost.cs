namespace Cake.Core.Scripting
{
    public interface IScriptHost : ICakeContext
    {
        ICakeContext GetContext();
    }
}