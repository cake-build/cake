#load "./../../utilities/xunit.cake"

//////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.Paths.DivideOperator.CombineDirectoryPathWithFilePath")
    .Does(() =>
{
    // When
    var path = Directory("./") / File("test.file");

    // Then
    Assert.Equal($"./test.file", path);
});

Task("Cake.Common.IO.Paths.DivideOperator.CombineDirectoryPathWithDirectoryPath")
    .Does(() =>
{
    // When
    var path = Directory("./") / Directory("temp");

    // Then
    Assert.Equal($"./temp", path);
});

Task("Cake.Common.IO.Paths.DivideOperator.CombineDirectoryPathWithDirectoryPathAndFilePath")
    .Does(() =>
{
    // When
    var path = Directory("./") / Directory("temp") / File("test.file");

    // Then
    Assert.Equal($"./temp/test.file", path);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.Paths")
    .IsDependentOn("Cake.Common.IO.Paths.DivideOperator.CombineDirectoryPathWithFilePath")
    .IsDependentOn("Cake.Common.IO.Paths.DivideOperator.CombineDirectoryPathWithDirectoryPath")
    .IsDependentOn("Cake.Common.IO.Paths.DivideOperator.CombineDirectoryPathWithDirectoryPathAndFilePath");
