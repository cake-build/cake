public static class Paths
{
    public static DirectoryPath TestRoot { get; private set; }
    public static DirectoryPath Temp { get; private set; }
    public static DirectoryPath Resources { get; private set; }
    public static FilePath CakeCore { get; private set; }
    public static FilePath CakeTool { get; private set; }

    public static void Initialize(ICakeContext context)
    {
        TestRoot = new DirectoryPath("./").MakeAbsolute(context.Environment);
        Temp = new DirectoryPath("./temp").MakeAbsolute(context.Environment);
        Resources = new DirectoryPath("./resources").MakeAbsolute(context.Environment);
        CakeCore = context.Environment.ApplicationRoot.CombineWithFilePath("Cake.Core.dll");
        CakeTool = context.Environment.ApplicationRoot.CombineWithFilePath("Cake.dll");
    }
}

Paths.Initialize(Context);