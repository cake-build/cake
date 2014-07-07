namespace Cake.Core.Scripting
{
    public interface IScriptSessionFactory
    {
        void Initialize();
        IScriptSession CreateSession(IScriptHost host);
    }
}
