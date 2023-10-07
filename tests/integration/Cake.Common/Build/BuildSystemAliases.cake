#load "GitHubActions/GitHubActionsProvider.cake"
#load "AzurePipelines/AzurePipelinesProvider.cake"

Task("Cake.Common.Build.BuildSystemAliases.BuildProvider")
    .DoesForEach(
        () => Enum.GetValues(typeof(BuildProvider)).OfType<BuildProvider>(),
        item => {
            Information("{0}: {1}", item, (BuildSystem.Provider & item) == item);
        });


Task("Cake.Common.Build.BuildSystemAliases")
    .IsDependentOn("Cake.Common.Build.BuildSystemAliases.BuildProvider")
    .IsDependentOn("Cake.Common.Build.GitHubActionsProvider")
    .IsDependentOn("Cake.Common.Build.AzurePipelinesProvider");