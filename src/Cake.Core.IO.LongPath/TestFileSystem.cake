#r "bin/Debug/Pri.LongPath.dll"
#r "bin/Debug/Cake.Core.IO.LongPath.dll"
var binDir = Directory("bin/Debug");
var testPaths = new DirectoryPath[]
                    {
                        "so this is a quick and dirty test of handling long paths within Cake.",
                        "using Pri.LongPath for Dotnet",
                        "though it has to be very long path like a namespace",
                        "Somtehing like Cake.Core.IO.LongPath",
                        "would probably put it over the top."
                    };

Action<string, Action> test = (scope, action) =>
{
    try
    {
        Information("Test: {0}", scope);
        action();
        Information("Success!");
    }
    catch(Exception ex)
    {
        Error("Failed {0}", ex);
    }
};

Action testCreateDir = () => {
    DirectoryPath current = binDir;
    foreach(var testPath in testPaths)
    {
        Information("Start processing {0}", testPath);
        current = current.Combine(testPath);
        if (!DirectoryExists(current))
        {
            CreateDirectory(current);
        }
    }
};


test("Create dir using standard .Net", testCreateDir);
test("Globber using standard .Net", ()=>GetFiles(binDir.Path + "/**/*.sln"));

Context.OmgTestFileSystemMethod(new Cake.Core.IO.LongPath.FileSystem());
test("Create dir using Pri.LongPath", testCreateDir);
test("Globber using Pri.LongPath", ()=>GetFiles(binDir.Path + "/**/*.sln"));