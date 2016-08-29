#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"

var expectedHashes = new Dictionary<HashAlgorithm, string> {
    { HashAlgorithm.MD5, "319c85697b47366e0c7d88f7ed7a1daa" },
    { HashAlgorithm.SHA256, "cee056ac444df70539cf14f7125741bbd2d09578a30a5d181f734e8d8e81e6df" },
    { HashAlgorithm.SHA512, "efebb6e3f3bc9113b88e93d48ef599fc1ce2c6d6e16baa99ea562664e53679bb9f12d1262e3d820d6441d5d220e0063e5f717a4e744d265c7c590ab153eeda3d" }
};

var securityAliasesTask =  Task("Cake.Common.Security.SecurityAliases")
    .IsDependentOn(
        Task("Cake.Common.Security.SecurityAliases.CalculateFileHash")
            .Does(()=>{
                // Given
                var path = Paths.Resources.Combine("./Cake.Common/Security");
                var file = path.CombineWithFilePath("./testfile.txt");
                string expect;
                expect = expectedHashes.TryGetValue(HashAlgorithm.SHA256, out expect)
                            ? expect
                            : null;

                // When
                var result = CalculateFileHash(file).ToHex();

                // Then
                Assert.Equal(expect, result);
                })
            .Task.Name
        );

Array.ForEach(
    Enum.GetValues(typeof(HashAlgorithm))
        .Cast<HashAlgorithm>()
        .ToArray(),
        hashAlgorithm => securityAliasesTask.IsDependentOn(
            Task(string.Format("Cake.Common.Security.SecurityAliases.CalculateFileHash.{0:F}", hashAlgorithm))
                .Does(() =>{
                        // Given
                        var path = Paths.Resources.Combine("./Cake.Common/Security");
                        var file = path.CombineWithFilePath("./testfile.txt");
                        string expect;
                        expect = expectedHashes.TryGetValue(hashAlgorithm, out expect)
                                    ? expect
                                    : null;

                        // When
                        var result =  CalculateFileHash(file, hashAlgorithm).ToHex();

                        // Then
                        Assert.Equal(expect, result);
                })
                .Task.Name
        )
);
