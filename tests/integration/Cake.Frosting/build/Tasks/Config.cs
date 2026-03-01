using Cake.Common.Diagnostics;
using Cake.Frosting;
using Xunit;

[TaskName(nameof(Config))]
public sealed class Config : FrostingTask<Context>
{
    private const string IntegrationTestEnvironment = "IntegrationTest_Environment";
    private const string IntegrationTestIniFile = "IntegrationTest_IniFile";
    private const string IntegrationTestArgument = "IntegrationTest_Argument";

    public override void Run(Context context)
    {
        // Given / When
        var result = IntegrationTest.FromContext(context);
        context.Verbose(logAction=>logAction("IntegrationTest: {0}", result));

        // Then
        Assert.Equal(IntegrationTest.Expected, result);
    }

    public record IntegrationTest(string Environment, string IniFile, string Argument)
    {
        public static IntegrationTest Expected {get; } = new IntegrationTest(
            bool.TrueString,
            bool.TrueString,
            bool.TrueString
            );
        public static IntegrationTest FromContext(Context context)
        {
            return new IntegrationTest(
                context.Configuration.GetValue(IntegrationTestEnvironment),
                context.Configuration.GetValue(IntegrationTestIniFile),
                context.Configuration.GetValue(IntegrationTestArgument)
            );
        }
    }
}