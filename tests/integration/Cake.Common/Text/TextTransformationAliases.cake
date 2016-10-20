#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"

Task("Cake.Common.Text.TextTransformationAliases.TransformText.Template")
    .Does(() =>
{
    // Given
    var template = "Hello <%subject%>!";
    var expect = "Hello world!";

    // When
    var result =  TransformText(template)
                    .WithToken("subject", "world")
                    .ToString();

    // Then
    Assert.Equal(expect, result);
});

Task("Cake.Common.Text.TextTransformationAliases.TransformText.CustomPlaceHolder")
    .Does(() =>
{
    // Given
    var template = "Hello {subject}!";
    var expect = "Hello world!";

    // When
    var result =  TransformText(template, "{", "}")
                    .WithToken("subject", "world")
                    .ToString();

    // Then
    Assert.Equal(expect, result);
});

Task("Cake.Common.Text.TextTransformationAliases.TransformTextFile.Template")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Text");
    var file = path.CombineWithFilePath("./template.txt");
    var expect = "Hello world!";

    // When
    var result =  TransformTextFile(file)
                    .WithToken("subject", "world")
                    .ToString();

    // Then
    Assert.Equal(expect, result);
});

Task("Cake.Common.Text.TextTransformationAliases.TransformTextFile.CustomPlaceHolder")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Text");
    var file = path.CombineWithFilePath("./customtemplate.txt");
    var expect = "Hello world!";

    // When
    var result =  TransformTextFile(file, "{", "}")
                    .WithToken("subject", "world")
                    .ToString();

    // Then
    Assert.Equal(expect, result);
});

Task("Cake.Common.Text.TextTransformationAliases")
    .IsDependentOn("Cake.Common.Text.TextTransformationAliases.TransformText.Template")
    .IsDependentOn("Cake.Common.Text.TextTransformationAliases.TransformText.CustomPlaceHolder")
    .IsDependentOn("Cake.Common.Text.TextTransformationAliases.TransformTextFile.Template")
    .IsDependentOn("Cake.Common.Text.TextTransformationAliases.TransformTextFile.CustomPlaceHolder");