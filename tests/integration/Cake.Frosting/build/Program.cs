using Cake.Core;
using Cake.Frosting;
using Microsoft.Extensions.DependencyInjection;

public class Program : IFrostingStartup
{
    public static int Main(string[] args)
        => new CakeHost()
            .UseStartup<Program>()
            .Run(args);

    public void Configure(IServiceCollection services)
    {
        services.UseContext<Context>();
        services.UseLifetime<Lifetime>();
        services.UseTool(new System.Uri("nuget:?package=xunit.runner.console"));
        services.UseWorkingDirectory("..");
    }
}