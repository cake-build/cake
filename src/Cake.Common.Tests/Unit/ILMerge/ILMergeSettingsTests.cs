using Cake.Common.ILMerge;
using Xunit;

namespace Cake.Common.Tests.Unit.ILMerge
{
    public sealed class ILMergeSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Internalize_To_False_By_Default()
            {
                // Given, When
                var settings = new ILMergeSettings();

                // Then
                Assert.False(settings.Internalize);
            }

            [Fact]
            public void Should_Set_Target_Kind_To_Default_By_Default()
            {
                // Given, When
                var settings = new ILMergeSettings();

                // Then
                Assert.Equal(TargetKind.Default, settings.TargetKind);
            }
        }
    }
}
