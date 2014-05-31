namespace Cake.Core.IO
{
    public interface IProcess
    {
        void WaitForExit();
        int GetExitCode();
    }
}
