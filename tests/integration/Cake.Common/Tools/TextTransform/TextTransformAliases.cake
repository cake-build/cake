#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"

Task("Cake.Common.Tools.TextTransform.TextTransformAliases.TransformTemplate.Setup")
    .Does(() =>
{
    var t4Path = Context.Tools.Resolve("t4") ?? Context.Tools.Resolve("t4.exe");
    if (t4Path == null)
    {
        DotNetCoreTool(null, "tool", "install --tool-path ./tools dotnet-t4 --version \"2.2.0-preview-0020-g990c44075e\" --add-source \"https://pkgs.dev.azure.com/cake-build/Cake/_packaging/cake/nuget/v3/index.json\"");
    }
});

Task("Cake.Common.Tools.TextTransform.TextTransformAliases.TransformTemplate.Properties")
    .IsDependentOn("Cake.Common.Tools.TextTransform.TextTransformAliases.TransformTemplate.Setup")
    .Does(() =>
{
       // Given
    var path = Paths.Resources.Combine("./Cake.Common/Tools/TextTransform");
    var file = path.CombineWithFilePath("./HelloWorld.tt");

    var targetPath = Paths.Temp.Combine("./Cake.Common.Tools.TextTransform.TextTransformAliases/TransformTemplate");
    var targetFile = targetPath.CombineWithFilePath("HelloWorld.txt");
    EnsureDirectoryExist(targetPath);

    var settings = new TextTransformSettings {
                    OutputFile = targetFile,
                    Properties = {
                        ["FirstName"] = "John",
                        ["LastName"] = "Doe"
                    }
                };

    var expect = "Hello John Doe!";

    // When
     TransformTemplate(file, settings);
     var result = System.IO.File.ReadAllText(targetFile.FullPath);
     Assert.Equal(expect, result);
});

Task("Cake.Common.Tools.TextTransform.TextTransformAliases.TransformTemplate.Class")
    .IsDependentOn("Cake.Common.Tools.TextTransform.TextTransformAliases.TransformTemplate.Setup")
    .Does(() =>
{
       // Given
    var path = Paths.Resources.Combine("./Cake.Common/Tools/TextTransform");
    var file = path.CombineWithFilePath("./HelloWorld.tt");

    var targetPath = Paths.Temp.Combine("./Cake.Common.Tools.TextTransform.TextTransformAliases/TransformTemplate");
    var targetFile = targetPath.CombineWithFilePath("HelloWorld.cs");
    EnsureDirectoryExist(targetPath);

    var settings = new TextTransformSettings {
                    OutputFile = targetFile,
                    Class = "HelloWorld"
                };

    var expect = @"public partial class HelloWorld : HelloWorldBase {";

    // When
     TransformTemplate(file, settings);
     var result = System.IO.File.ReadAllLines(targetFile.FullPath).FirstOrDefault(line => line.StartsWith("public partial class HelloWorld"));
     Assert.Equal(expect, result);
});


Task("Cake.Common.Tools.TextTransform.TextTransformAliases")
    .IsDependentOn("Cake.Common.Tools.TextTransform.TextTransformAliases.TransformTemplate.Setup")
    .IsDependentOn("Cake.Common.Tools.TextTransform.TextTransformAliases.TransformTemplate.Properties")
    .IsDependentOn("Cake.Common.Tools.TextTransform.TextTransformAliases.TransformTemplate.Class");
