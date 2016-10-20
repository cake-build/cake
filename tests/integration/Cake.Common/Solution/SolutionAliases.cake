#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"
#load "./../../utilities/comparers/SolutionParserResultEqualityComparer.cake"
#load "./../../utilities/comparers/SolutionProjectEqualityComparer.cake"

Task("Cake.Common.Solution.SolutionAliases.ParseSolution")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Solution");
    var file = path.CombineWithFilePath("./Solution.sln");
    var expect = new SolutionParserResult(
        version: "Format Version 12.00",
        visualStudioVersion: "14.0.25420.1",
        minimumVisualStudioVersion: "10.0.40219.1",
        projects: new System.Collections.ObjectModel.ReadOnlyCollection<SolutionProject>(new [] {
                new SolutionProject(
                    id: "{4147F50A-8743-4393-A414-EFC707D4BA2C}",
                    name: "Project",
                    path: path.CombineWithFilePath("Project/Project.csproj"),
                    type: "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"
                ),
                new SolutionProject(
                    id: "{A1F3F53D-317B-451E-B616-DE4C408BF740}",
                    name: "ReferenceProject",
                    path: path.CombineWithFilePath("ReferenceProject/ReferenceProject.csproj"),
                    type: "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"
                )
            })
        );

    // When
    var result = ParseSolution(file);

    // Then
    Assert.NotNull(result);
    Assert.Equal(expect.Projects, result.Projects, SolutionProjectEqualityComparer.Comparer);
    Assert.Equal(expect, result, SolutionParserResultEqualityComparer.Comparer);
});


Task("Cake.Common.Solution.SolutionAliases")
    .IsDependentOn("Cake.Common.Solution.SolutionAliases.ParseSolution");