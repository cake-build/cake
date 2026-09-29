#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"
#load "./../../utilities/io.cake"

using System.Text;
using System.Threading;

Task("Cake.Core.IO.FileContent.WriteAndReadAllText")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Core.IO.FileContent/WriteAndReadAllText");
    EnsureDirectoryExist(path);
    var file = Context.FileSystem.GetFile(path.CombineWithFilePath("out.txt"));

    // When
    file.WriteAllText("Hello World");
    var result = file.ReadAllText();

    // Then
    Assert.Equal("Hello World", result);
});

Task("Cake.Core.IO.FileContent.AppendAllText")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Core.IO.FileContent/AppendAllText");
    EnsureDirectoryExist(path);
    var file = Context.FileSystem.GetFile(path.CombineWithFilePath("out.txt"));

    // When
    file.WriteAllText("Hello");
    file.AppendAllText(" World");

    // Then
    Assert.Equal("Hello World", file.ReadAllText());
});

Task("Cake.Core.IO.FileContent.WriteAndReadAllBytes")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Core.IO.FileContent/WriteAndReadAllBytes");
    EnsureDirectoryExist(path);
    var file = Context.FileSystem.GetFile(path.CombineWithFilePath("data.bin"));
    var bytes = new byte[] { 1, 2, 3, 4 };

    // When
    file.WriteAllBytes(bytes);
    file.AppendAllBytes(new byte[] { 5 });

    // Then
    Assert.Equal(new byte[] { 1, 2, 3, 4, 5 }, file.ReadAllBytes());
});

Task("Cake.Core.IO.FileContent.WriteAndReadAllLines")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Core.IO.FileContent/WriteAndReadAllLines");
    EnsureDirectoryExist(path);
    var file = Context.FileSystem.GetFile(path.CombineWithFilePath("lines.txt"));

    // When
    file.WriteAllLines(new[] { "one", "two" });
    file.AppendAllLines(new[] { "three" });

    // Then
    Assert.Equal(new[] { "one", "two", "three" }, file.ReadAllLines());
});

Task("Cake.Core.IO.FileContent.ReadLines")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Core.IO.FileContent/ReadLines");
    EnsureDirectoryExist(path);
    var file = Context.FileSystem.GetFile(path.CombineWithFilePath("lines.txt"));
    file.WriteAllLines(new[] { "one", "two" });

    // When
    var result = file.ReadLines().ToArray();

    // Then
    Assert.Equal(new[] { "one", "two" }, result);
});

Task("Cake.Core.IO.FileContent.Encoding")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Core.IO.FileContent/Encoding");
    EnsureDirectoryExist(path);
    var file = Context.FileSystem.GetFile(path.CombineWithFilePath("unicode.txt"));
    const string contents = "Åäö";

    // When
    file.WriteAllText(contents, Encoding.Unicode);

    // Then
    Assert.Equal(contents, file.ReadAllText(Encoding.Unicode));
});

Task("Cake.Core.IO.FileContent.CreateTextOpenTextAppendText")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Core.IO.FileContent/TextStreams");
    EnsureDirectoryExist(path);
    var file = Context.FileSystem.GetFile(path.CombineWithFilePath("out.txt"));

    // When
    using (var writer = file.CreateText())
    {
        writer.Write("Hello");
    }
    using (var writer = file.AppendText())
    {
        writer.Write(" World");
    }
    string result;
    using (var reader = file.OpenText())
    {
        result = reader.ReadToEnd();
    }

    // Then
    Assert.Equal("Hello World", result);
});

Task("Cake.Core.IO.FileContent.Async")
    .Does(async () =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Core.IO.FileContent/Async");
    EnsureDirectoryExist(path);
    var file = Context.FileSystem.GetFile(path.CombineWithFilePath("out.txt"));

    // When
    await file.WriteAllTextAsync("Hello");
    await file.AppendAllTextAsync(" World", CancellationToken.None);
    var result = await file.ReadAllTextAsync();

    var lines = new List<string>();
    await foreach (var line in file.ReadLinesAsync())
    {
        lines.Add(line);
    }

    // Then
    Assert.Equal("Hello World", result);
    Assert.Equal(new[] { "Hello World" }, lines);
});

Task("Cake.Core.IO.FileContent")
    .IsDependentOn("Cake.Core.IO.FileContent.WriteAndReadAllText")
    .IsDependentOn("Cake.Core.IO.FileContent.AppendAllText")
    .IsDependentOn("Cake.Core.IO.FileContent.WriteAndReadAllBytes")
    .IsDependentOn("Cake.Core.IO.FileContent.WriteAndReadAllLines")
    .IsDependentOn("Cake.Core.IO.FileContent.ReadLines")
    .IsDependentOn("Cake.Core.IO.FileContent.Encoding")
    .IsDependentOn("Cake.Core.IO.FileContent.CreateTextOpenTextAppendText")
    .IsDependentOn("Cake.Core.IO.FileContent.Async");
