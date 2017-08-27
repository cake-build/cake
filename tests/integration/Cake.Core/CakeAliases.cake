var cakeCoreCakeAliasesDoesForEachTestFiles = Paths.Resources.Combine("./Cake.Core/testfile*.txt");

var cakeCoreCakeAliasesDoesForEachOkCount = 0;
Task("Cake.Core.CakeAliases.DoesForEach.Ok")
    .DoesForEach(cakeCoreCakeAliasesDoesForEachTestFiles, (file) =>
{
    // Given
    cakeCoreCakeAliasesDoesForEachOkCount++;

    // When
    Assert.StartsWith(file, "Test core file");
})
.Finally(() => {
    // Then
    Assert.Equal(cakeCoreCakeAliasesDoesForEachOkCount, 3);
});

var cakeCoreCakeAliasesDoesForEachNotOkCount = 0;
Task("Cake.Core.CakeAliases.DoesForEach.NotOk")
    .DeferExceptions()
    .DoesForEach(cakeCoreCakeAliasesDoesForEachTestFiles, (file) =>
{
    // Given
    cakeCoreCakeAliasesDoesForEachNotOkCount++;

    // When
    throw new Exception(file);
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
    Assert.StartsWith(file, "Test core file");
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
.Finally(() => {
    // Then
    Assert.Equal(cakeCoreCakeAliasesDoesForEachFuncNotOkCount, 1);
});

Task("Cake.Core.CakeAliases")
    .IsDependentOn("Cake.Core.CakeAliases.DoesForEach.Ok")
    .IsDependentOn("Cake.Core.CakeAliases.DoesForEach.NotOk")
    .IsDependentOn("Cake.Core.CakeAliases.DoesForEach.Func.Ok")
    .IsDependentOn("Cake.Core.CakeAliases.DoesForEach.Func.NotOk");