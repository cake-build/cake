var cakeCoreCakeAliasesDoesForEachTestFiles = GetFiles($"{Paths.Resources}/Cake.Core/testfile*.txt")
                                                .Select(file=>System.IO.File.ReadAllText(file.FullPath))
                                                .ToArray();

var cakeCoreCakeAliasesDoesForEachOkCount = 0;
Task("Cake.Core.CakeAliases.DoesForEach.Ok")
    .DoesForEach(cakeCoreCakeAliasesDoesForEachTestFiles, (file) =>
{
    // Given
    cakeCoreCakeAliasesDoesForEachOkCount++;

    // When
    Assert.StartsWith("Test core file", file);
})
.Finally(() => {
    // Then
    Assert.Equal(cakeCoreCakeAliasesDoesForEachOkCount, 3);
});

var cakeCoreCakeAliasesDoesForEachNotOkCount = 0;
Task("Cake.Core.CakeAliases.DoesForEach.NotOk")
    .DeferOnError()
    .DoesForEach(cakeCoreCakeAliasesDoesForEachTestFiles, (file) =>
{
    // Given
    cakeCoreCakeAliasesDoesForEachNotOkCount++;

    // When
    throw new Exception(file);
})
.OnError(exception =>
{
    Assert.Equal(3, (exception as AggregateException)?.InnerExceptions?.Count);
})
.Finally(() => {
    // Then
    Assert.Equal(cakeCoreCakeAliasesDoesForEachNotOkCount, 3);
});

var cakeCoreCakeAliasesDoesForEachFuncOkCount = 0;
Task("Cake.Core.CakeAliases.DoesForEach.Func.Ok")
    .DoesForEach(() => cakeCoreCakeAliasesDoesForEachTestFiles, (file) =>
{
    // Given
    cakeCoreCakeAliasesDoesForEachFuncOkCount++;

    // When
    Assert.StartsWith("Test core file", file);
})
.Finally(() => {
    // Then
    Assert.Equal(cakeCoreCakeAliasesDoesForEachFuncOkCount, 3);
});

var cakeCoreCakeAliasesDoesForEachFuncNotOkCount = 0;
Task("Cake.Core.CakeAliases.DoesForEach.Func.NotOk")
    .DoesForEach(() => cakeCoreCakeAliasesDoesForEachTestFiles, (file) =>
{
    // Given
    cakeCoreCakeAliasesDoesForEachFuncNotOkCount++;

    // When
    throw new Exception(file);
})
.OnError(exception =>
{
    Assert.Null(exception as AggregateException);
})
.Finally(() => {
    // Then
    Assert.Equal(cakeCoreCakeAliasesDoesForEachFuncNotOkCount, 1);
});

var cakeCoreCakeAliasesOnErrorAsync = 0;
Task("Cake.Core.CakeAliases.OnError.Async")
.Does(() => {
    // Given
    throw new CakeException("Async Error");
})
.OnError(async exception => {
    // When
    await System.Threading.Tasks.Task.CompletedTask;
    cakeCoreCakeAliasesOnErrorAsync++;
    })
.Finally(() => {
    // Then
    Assert.Equal(cakeCoreCakeAliasesOnErrorAsync, 1);
});

var cakeCoreCakeAliasesOnErrorSync = 0;
Task("Cake.Core.CakeAliases.OnError.Sync")
.Does(() => {
    // Given
    throw new CakeException("Sync Error");
})
.OnError(exception => {
    // When
    cakeCoreCakeAliasesOnErrorSync++;
    })
.Finally(() => {
    // Then
    Assert.Equal(cakeCoreCakeAliasesOnErrorSync, 1);
});


Task("Cake.Core.CakeAliases")
    .IsDependentOn("Cake.Core.CakeAliases.DoesForEach.Ok")
    .IsDependentOn("Cake.Core.CakeAliases.DoesForEach.NotOk")
    .IsDependentOn("Cake.Core.CakeAliases.DoesForEach.Func.Ok")
    .IsDependentOn("Cake.Core.CakeAliases.DoesForEach.Func.NotOk")
    .IsDependentOn("Cake.Core.CakeAliases.OnError.Async")
    .IsDependentOn("Cake.Core.CakeAliases.OnError.Sync");