using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class SemVerTests
    {
        public sealed class TheTryParseMethod
        {
            [Fact]
            public void Should_Return_False_If_Version_Is_Null()
            {
                // Given
                string version = null;

                // When
                var result = SemVersion.TryParse(version, out _);

                // Then
                Assert.False(result);
            }

            [Theory]
            [InlineData(1, 2, 3, null, null, "1.2.3")]
            [InlineData(1, 2, 3, "rc001", null, "1.2.3-rc001")]
            [InlineData(1, 2, 3, "rc001", "meta1", "1.2.3-rc001+meta1")]
            public void Should_Return_True_If_Version_Is_Valid(int major, int minor, int patch, string preRelease, string meta, string versionString)
            {
                // Given
                var expect = new SemVersion(major, minor, patch, preRelease, meta);

                // When
                var result = SemVersion.TryParse(versionString, out var parsedSemVersion);

                // Then
                Assert.True(result, nameof(SemVersion.TryParse));
                Assert.Equal(expect, parsedSemVersion);
                Assert.True(expect == parsedSemVersion, "expect == parsedSemVersion");
                Assert.Equal(versionString, parsedSemVersion.VersionString);
            }
        }

        public sealed class Operators
        {
            private const bool ExpectGreaterThanTrue = true;
            private const bool ExpectGreaterThanFalse = false;
            private const bool ExpectLessThanTrue = true;
            private const bool ExpectLessThanFalse = false;
            private const bool ExpectGreaterThanOrEqualTrue = true;
            private const bool ExpectGreaterThanOrEqualFalse = false;
            private const bool ExpectLesserThanOrEqualTrue = true;
            private const bool ExpectLesserThanOrEqualFalse = false;
            private const bool ExpectEqualToTrue = true;
            private const bool ExpectEqualToFalse = false;
            private const bool ExpectNotEqualToTrue = true;
            private const bool ExpectNotEqualToFalse = false;

            [Theory]
            [InlineData("1.2.3", "1.2.3", ExpectGreaterThanFalse, ExpectLessThanFalse, ExpectGreaterThanOrEqualTrue, ExpectLesserThanOrEqualTrue, ExpectEqualToTrue, ExpectNotEqualToFalse)]
            [InlineData("1.2.4", "1.2.3", ExpectGreaterThanTrue, ExpectLessThanFalse, ExpectGreaterThanOrEqualTrue, ExpectLesserThanOrEqualFalse, ExpectEqualToFalse, ExpectNotEqualToTrue)]
            [InlineData("1.2.3", "1.2.4", ExpectGreaterThanFalse, ExpectLessThanTrue, ExpectGreaterThanOrEqualFalse, ExpectLesserThanOrEqualTrue, ExpectEqualToFalse, ExpectNotEqualToTrue)]
            [InlineData("1.2.3-rc001", "1.2.3-rc001", ExpectGreaterThanFalse, ExpectLessThanFalse, ExpectGreaterThanOrEqualTrue, ExpectLesserThanOrEqualTrue, ExpectEqualToTrue, ExpectNotEqualToFalse)]
            [InlineData("1.2.4-rc001", "1.2.3-rc001", ExpectGreaterThanTrue, ExpectLessThanFalse, ExpectGreaterThanOrEqualTrue, ExpectLesserThanOrEqualFalse, ExpectEqualToFalse, ExpectNotEqualToTrue)]
            [InlineData("1.2.3-rc001", "1.2.4-rc001", ExpectGreaterThanFalse, ExpectLessThanTrue, ExpectGreaterThanOrEqualFalse, ExpectLesserThanOrEqualTrue, ExpectEqualToFalse, ExpectNotEqualToTrue)]
            [InlineData("1.2.3-rc001+meta001", "1.2.3-rc001+meta001", ExpectGreaterThanFalse, ExpectLessThanFalse, ExpectGreaterThanOrEqualTrue, ExpectLesserThanOrEqualTrue, ExpectEqualToTrue, ExpectNotEqualToFalse)]
            [InlineData("1.2.4-rc001+meta001", "1.2.3-rc001+meta001", ExpectGreaterThanTrue, ExpectLessThanFalse, ExpectGreaterThanOrEqualTrue, ExpectLesserThanOrEqualFalse, ExpectEqualToFalse, ExpectNotEqualToTrue)]
            [InlineData("1.2.3-rc001+meta001", "1.2.4-rc001+meta001", ExpectGreaterThanFalse, ExpectLessThanTrue, ExpectGreaterThanOrEqualFalse, ExpectLesserThanOrEqualTrue, ExpectEqualToFalse, ExpectNotEqualToTrue)]
            [InlineData("1.2.3", "1.2.3-rc001", ExpectGreaterThanTrue, ExpectLessThanFalse, ExpectGreaterThanOrEqualTrue, ExpectLesserThanOrEqualFalse, ExpectEqualToFalse, ExpectNotEqualToTrue)]
            [InlineData("1.2.3-rc001", "1.2.3", ExpectGreaterThanFalse, ExpectLessThanTrue, ExpectGreaterThanOrEqualFalse, ExpectLesserThanOrEqualTrue, ExpectEqualToFalse, ExpectNotEqualToTrue)]
            [InlineData("1.2.3-rc001", "1.2.3-rc001+meta001", ExpectGreaterThanTrue, ExpectLessThanFalse, ExpectGreaterThanOrEqualTrue, ExpectLesserThanOrEqualFalse, ExpectEqualToFalse, ExpectNotEqualToTrue)]
            [InlineData("1.2.3-rc001+meta001", "1.2.3-rc001", ExpectGreaterThanFalse, ExpectLessThanTrue, ExpectGreaterThanOrEqualFalse, ExpectLesserThanOrEqualTrue, ExpectEqualToFalse, ExpectNotEqualToTrue)]
            public void Should_Return_Expected(string operand1string, string operand2string, bool expectGreaterThan, bool expectLessThan, bool expectGreaterThanOrEqual, bool expectLesserThanOrEqual, bool expectEqualTo, bool expectNotEqualTo)
            {
                // Given
                var expect = new
                {
                    ParsedOperand1 = true,
                    ParsedOperand2 = true,
                    GreaterThan = expectGreaterThan,
                    LessThan = expectLessThan,
                    GreaterThanOrEqual = expectGreaterThanOrEqual,
                    LesserThanOrEqual = expectLesserThanOrEqual,
                    EqualTo = expectEqualTo,
                    NotEqualTo = expectNotEqualTo
                };

                // When
                var result = new
                {
                    ParsedOperand1 = SemVersion.TryParse(operand1string, out var operand1),
                    ParsedOperand2 = SemVersion.TryParse(operand2string, out var operand2),
                    GreaterThan = operand1 > operand2,
                    LessThan = operand1 < operand2,
                    GreaterThanOrEqual = operand1 >= operand2,
                    LesserThanOrEqual = operand1 <= operand2,
                    EqualTo = operand1 == operand2,
                    NotEqualTo = operand1 != operand2,
                };

                // Then
                Assert.Equal(expect, result);
            }

            [Fact]
            public void Should_Be_Able_To_Compare_Equals_Null()
            {
                // Given
                SemVersion semVersion = null;

                // When / Then
                Assert.True(semVersion == null);
            }

            [Fact]
            public void Should_Be_Able_To_Compare_Not_Equals_Null()
            {
                // Given
                SemVersion semVersion = null;

                // When / Then
                Assert.False(semVersion != null);
            }

            [Fact]
            public void Should_Be_Able_To_Compare_GreaterThan_Null()
            {
                // Given
                SemVersion semVersion = null;

                // When / Then
                Assert.False(semVersion > null);
            }

            [Fact]
            public void Should_Be_Able_To_Compare_LessThan_Null()
            {
                // Given
                SemVersion semVersion = null;

                // When / Then
                Assert.False(semVersion < null);
            }

            [Fact]
            public void Should_Be_Able_To_Compare_GreaterEqualThan_Null()
            {
                // Given
                SemVersion semVersion = null;

                // When / Then
                Assert.True(semVersion >= null);
            }

            [Fact]
            public void Should_Be_Able_To_Compare_LessEqualThan_Null()
            {
                // Given
                SemVersion semVersion = null;

                // When / Then
                Assert.True(semVersion <= null);
            }
        }
    }
}
