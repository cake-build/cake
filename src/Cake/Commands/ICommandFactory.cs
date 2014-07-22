namespace Cake.Commands
{
    public interface ICommandFactory
    {
        ICommand CreateBuildCommand();
        ICommand CreateDescriptionCommand();
        ICommand CreateHelpCommand();
        ICommand CreateVersionCommand();
    }
}
