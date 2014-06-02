using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Tests.Fixtures;
using NSubstitute;
using Xunit;
using Cake.Core.Extensions;

namespace Cake.Tests.Extensions
{
    public sealed class CakeTaskExtensionsTests
    {
        public sealed class TheWithCriteriaMethod
        {
            public sealed class ThatAcceptsFuncLambdaWithoutContext
            {
                [Fact]
                public void Should_Evaluate_Criteria()
                {
                    // Given
                    var result = new List<string>();
                    var engine = new CakeEngineFixture().CreateEngine();
                    engine.Task("A").Does(x => result.Add("A"));
                    engine.Task("B").IsDependentOn("A").WithCriteria(() => false).Does(x => result.Add("B"));
                    engine.Task("C").IsDependentOn("B").Does(x => result.Add("C"));

                    // When
                    engine.Run("C");

                    // Then
                    Assert.Equal(2, result.Count);
                    Assert.Equal("A", result[0]);
                    Assert.Equal("C", result[1]);
                }      
            }

            public sealed class ThatAcceptsBoolean
            {
                [Fact]
                public void Should_Evaluate_Criteria()
                {
                    // Given
                    var result = new List<string>();
                    var engine = new CakeEngineFixture().CreateEngine();
                    engine.Task("A").Does(x => result.Add("A"));
                    engine.Task("B").IsDependentOn("A").WithCriteria(false).Does(x => result.Add("B"));
                    engine.Task("C").IsDependentOn("B").Does(x => result.Add("C"));

                    // When
                    engine.Run("C");

                    // Then
                    Assert.Equal(2, result.Count);
                    Assert.Equal("A", result[0]);
                    Assert.Equal("C", result[1]);
                }
            }
        }
    }
}
