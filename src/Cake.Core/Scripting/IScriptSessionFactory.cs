namespace Cake.Core.Scripting
{
    public interface IScriptSessionFactory
    {
        IScriptSession CreateSession(IScriptHost host);
    }
}
