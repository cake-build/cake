Task("Cake.Common.Build.AzurePipelinesProvider.Commands.Group")
    .Does(() => {
        using (AzurePipelines.Commands.Group("Cake Group"))
        {
            Console.WriteLine("This is inside a group");
        }
});

Task("Cake.Common.Build.AzurePipelinesProvider.Commands.Section")
    .Does(() => {
        AzurePipelines.Commands.Section("Cake Section");
});


var azurePipelinesProviderTask = Task("Cake.Common.Build.AzurePipelinesProvider");

if(AzurePipelines.IsRunningOnAzurePipelines)
{
    azurePipelinesProviderTask
        .IsDependentOn("Cake.Common.Build.AzurePipelinesProvider.Commands.Group")
        .IsDependentOn("Cake.Common.Build.AzurePipelinesProvider.Commands.Section");
}