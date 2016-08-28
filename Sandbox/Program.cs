using Cake.Frosting;

namespace Sandbox
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // Create the host.
            var host = new CakeHostBuilder()
                .UseStartup<Startup>()
                .WithArguments(args)
                .ConfigureServices(services =>
                {
                    // You could also configure services like this.
                    services.UseContext<Settings>();
                })
                .Build();

            // Run the host.
            return host.Run();
        }
    }

    public class Startup : IFrostingStartup
    {
        public void Configure(ICakeServices services)
        {
            services.UseContext<Settings>();
        }
    }
}