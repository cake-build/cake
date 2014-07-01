namespace Cake.Core.Scripting
{
    public interface IScriptHost : ICakeEngine
    {
        ICakeContext GetContext();
    }
}