#load "./../../../../utilities/xunit.cake"
#load "./../../../../utilities/paths.cake"

Task("Cake.Common.Solution.Project.XmlDoc.XmlDocAliases.ParseXmlDocExampleCode")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Solution/Project/XmlDoc");
    var file = path.CombineWithFilePath("./Common.XML");
    var expect = new XmlDocExampleCode(
            name: "M:Common.CommonClass.CommonMethod",
            code: "            CommonClass.CommonMethod();"
        );

    // When
    var result = ParseXmlDocExampleCode(file).SingleOrDefault();

    // Then
    Assert.NotNull(result);
    Assert.Equal(expect.Name, result.Name);
    Assert.Equal(expect.Code, result.Code);
});

Task("Cake.Common.Solution.Project.XmlDoc.XmlDocAliases.ParseXmlDocFilesExampleCode")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Solution/Project/XmlDoc");
    var expectFirst = new XmlDocExampleCode(
            name: "M:Common.CommonClass.CommonMethod",
            code: "            CommonClass.CommonMethod();"
        );
    var expectLast = new XmlDocExampleCode(
            name: "M:Core.CoreClass.CoreMethod",
            code: "            CoreClass.CoreMethod();"
        );

    // When
    var result = ParseXmlDocFilesExampleCode(path.FullPath + "/**/*.XML")
                    .OrderBy(code=>code.Name)
                    .ToArray();
    var first = result.FirstOrDefault();
    var last = result.LastOrDefault();

    // Then
    Assert.NotNull(first);
    Assert.NotNull(last);
    Assert.NotEqual(first, last);
    Assert.Equal(2, result.Length);
    Assert.Equal(expectFirst.Name, first.Name);
    Assert.Equal(expectFirst.Code, first.Code);
    Assert.Equal(expectLast.Name, last.Name);
    Assert.Equal(expectLast.Code, last.Code);
});

Task("Cake.Common.Solution.Project.XmlDoc.XmlDocAliases")
    .IsDependentOn("Cake.Common.Solution.Project.XmlDoc.XmlDocAliases.ParseXmlDocExampleCode")
    .IsDependentOn("Cake.Common.Solution.Project.XmlDoc.XmlDocAliases.ParseXmlDocFilesExampleCode");
