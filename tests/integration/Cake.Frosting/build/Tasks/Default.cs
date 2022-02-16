using Cake.Frosting;

[IsDependentOn(typeof(Hello))]
public sealed class Default : FrostingTask<Context>
{
}