using Cake.Frosting;

[IsDependentOn(typeof(Hello))]
[IsDependentOn(typeof(Config))]
public sealed class Default : FrostingTask<Context>
{
}