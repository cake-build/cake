using Cake.Core.Annotations;
using Cake.Core.Composition;

[assembly: CakeModule(typeof(MyServiceModule))]

public interface IMyService
{
    string DoSomething();
}

public sealed class MyService : IMyService
{
    public string DoSomething()
    {
        return "Hello from MyService";
    }
}

public sealed class MyServiceModule : ICakeModule
{
    public void Register(ICakeContainerRegistrar registrar)
    {
        registrar.RegisterType<MyService>().As<IMyService>().Singleton();
    }
}
