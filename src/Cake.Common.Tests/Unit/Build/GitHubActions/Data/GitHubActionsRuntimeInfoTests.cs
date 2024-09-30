using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitHubActions.Data
{
    public sealed class GitHubActionsRuntimeInfoTests
    {
        public sealed class TheIsRuntimeAvailableProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.IsRuntimeAvailable;

                // Then
                Assert.Equal(true, result);
            }
        }

        public sealed class TheTokenProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.Token;

                // Then
                Assert.Equal(
                    "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ikh5cTROQVRBanNucUM3bWRydEFoaHJDUjJfUSJ9.eyJuYW1laWQiOiJkZGRkZGRkZC1kZGRkLWRkZGQtZGRkZC1kZGRkZGRkZGRkZGQiLCJzY3AiOiJBY3Rpb25zLkdlbmVyaWNSZWFkOjAwMDAwMDAwLTAwMDAtMDAwMC0wMDAwLTAwMDAwMDAwMDAwMCBBY3Rpb25zLlJlc3VsdHM6YjllMjgxNTMtY2EyMC00Yjg2LTkxZGQtMDllOGY2NDRlZmRmOjFkODQ5YTQ1LTJmMzAtNWZiYi0zMjI2LWI3MzBhMTdhOTNhZiBBY3Rpb25zLlVwbG9hZEFydGlmYWN0czowMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAvMTpCdWlsZC9CdWlsZC8xNiBMb2NhdGlvblNlcnZpY2UuQ29ubmVjdCBSZWFkQW5kVXBkYXRlQnVpbGRCeVVyaTowMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAvMTpCdWlsZC9CdWlsZC8xNiIsIklkZW50aXR5VHlwZUNsYWltIjoiU3lzdGVtOlNlcnZpY2VJZGVudGl0eSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3NpZCI6IkRERERERERELUREREQtRERERC1ERERELURERERERERERERERCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcHJpbWFyeXNpZCI6ImRkZGRkZGRkLWRkZGQtZGRkZC1kZGRkLWRkZGRkZGRkZGRkZCIsImF1aSI6ImUyMTI4OTY1LThlY2EtNDgxYy1hODhkLWJmOTFlZDg3Y2RiNSIsInNpZCI6ImMwNmVjY2E0LWY3ZjUtNGY4Mi1iM2IxLTJhYjM0M2Y4Mjg3NCIsImFjIjoiW3tcIlNjb3BlXCI6XCJyZWZzL2hlYWRzL21haW5cIixcIlBlcm1pc3Npb25cIjozfV0iLCJhY3NsIjoiMTAiLCJvcmNoaWQiOiJiOWUyODE1My1jYTIwLTRiODYtOTFkZC0wOWU4ZjY0NGVmZGYuYnVpbGQudWJ1bnR1LWxhdGVzdCIsImlzcyI6InZzdG9rZW4uYWN0aW9ucy5naXRodWJ1c2VyY29udGVudC5jb20iLCJhdWQiOiJ2c3Rva2VuLmFjdGlvbnMuZ2l0aHVidXNlcmNvbnRlbnQuY29tfHZzbzo0M2YwNTdkMC0wODAzLTRkOTEtOTRhMS1mOGViMTAzZGYxMWYiLCJuYmYiOjE3Mjc1NDQzOTIsImV4cCI6MTcyNzU2NzE5Mn0.sUTvwxD-NlbAhQJB7cIInovd9qDkFHWcwOiiQAlHCsjpRBCEUWb3tWfOmCEpn8It4FWkaSszjMd8oecBEMlyEUtk6Cm6l1AqCUnIT13B48c_2sjhjWz-UDNMt94nzYH2ulC8mBcV_kSEIHJUvOnFKrFMKEdg6axAjLCx4la9MOklVq2ehx6DC12qbUNpTELJGeWz_JvKHWexyfN1qJgUw3y4ritZDJF3HLTpb5IJS7sQmFZVB7F2P6DF-1iaCBX5hgA9KfiwWXw6oTkKd6aOEyJpcBe0b87V_-fVTivOUS-ABE5XN6TCLZSmt7X6qwTPeSoLKgQGx1h_tHwubGDjtQ",
                    result);
            }
        }

        public sealed class TheUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.Url;

                // Then
                Assert.Equal("https://pipelines.actions.githubusercontent.com/ip0FyYnZXxdEOcOwPHkRsZJd2x6G5XoT486UsAb0/", result);
            }
        }

        public sealed class TheEnvPathProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.EnvPath.FullPath;

                // Then
                Assert.Equal("/opt/github.env", result);
            }
        }

        public sealed class TheSystemPathProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.SystemPath.FullPath;

                // Then
                Assert.Equal("/opt/github.path", result);
            }
        }
    }
}