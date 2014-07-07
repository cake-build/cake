namespace Cake.Commands
{
    public interface ICommand
    {
        void Execute(CakeOptions options);
    }
}
