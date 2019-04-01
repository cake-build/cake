// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cake.Core.Scripting.CodeGen;
using Xunit;
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace Cake.Core.Tests.Unit.Scripting.CodeGen
{
    public sealed class ParameterEmitterTests
    {
        public enum TestEnum
        {
            None,
            Some,
            All
        }

        [AttributeUsageAttribute(AttributeTargets.Parameter)]
        public sealed class TestParameterAttribute : Attribute
        {
            public TestParameterAttribute(string name)
            {
            }

            public TestParameterAttribute(int number)
            {
            }

            public TestParameterAttribute(TestEnum value)
            {
            }

            public TestParameterAttribute(TestEnum[] values)
            {
            }

            public TestParameterAttribute()
            {
            }

            public string StringProperty { get; set; }

            public int Int32Property { get; set; }

            public TestEnum EnumProperty { get; set; }
        }

        private static class ParameterFixture
        {
            public static void RequiredInt(int arg)
            {
            }

            public static void RequiredDouble(double arg)
            {
            }

            public static void RequiredDecimal(decimal arg)
            {
            }

            public static void RequiredLong(long arg)
            {
            }

            public static void RequiredShort(short arg)
            {
            }

            public static void RequiredSByte(sbyte arg)
            {
            }

            public static void RequiredByte(byte arg)
            {
            }

            public static void RequiredULong(ulong arg)
            {
            }

            public static void RequiredUShort(ushort arg)
            {
            }

            public static void RequiredUInt(uint arg)
            {
            }

            public static void RequiredFloat(float arg)
            {
            }

            public static void RequiredEnum(TestEnum arg)
            {
            }

            public static void RequiredBool(bool arg)
            {
            }

            public static void RequiredString(string arg)
            {
            }

            public static void RequiredObject(object arg)
            {
            }

            public static void RequiredDateTime(DateTime arg)
            {
            }

            public static void RequiredInterface(IDisposable arg)
            {
            }

            public static void RequiredClass(CakeContext arg)
            {
            }

            public static void RequiredType(Type type)
            {
            }

            public static void RequiredGenericEnumerable(System.Collections.Generic.IEnumerable<string> items)
            {
            }

            public static void OptionalInt(int arg = 1)
            {
            }

            public static void OptionalDouble(double arg = 1)
            {
            }

            public static void OptionalDecimal(decimal arg = 1)
            {
            }

            public static void OptionalLong(long arg = 1)
            {
            }

            public static void OptionalShort(short arg = 1)
            {
            }

            public static void OptionalSByte(sbyte arg = 1)
            {
            }

            public static void OptionalByte(byte arg = 1)
            {
            }

            public static void OptionalULong(ulong arg = 1)
            {
            }

            public static void OptionalUShort(ushort arg = 1)
            {
            }

            public static void OptionalUInt(uint arg = 1)
            {
            }

            public static void OptionalFloat(float arg = 1)
            {
            }

            public static void OptionalEnum(TestEnum arg = TestEnum.All)
            {
            }

            public static void OptionalBool(bool arg = true)
            {
            }

            public static void OptionalString(string arg = "value")
            {
            }

            public static void OptionalObject(object arg = null)
            {
            }

            public static void OptionalInterface(IDisposable arg = null)
            {
            }

            public static void OptionalClass(CakeContext arg = null)
            {
            }

            public static void OptionalType(Type type = null)
            {
            }

            public static void OptionalGenericEnumerable(System.Collections.Generic.IEnumerable<string> items = null)
            {
            }

            public static void RequiredNullableInt(int? arg)
            {
            }

            public static void RequiredNullableDouble(double? arg)
            {
            }

            public static void RequiredNullableDecimal(decimal? arg)
            {
            }

            public static void RequiredNullableLong(long? arg)
            {
            }

            public static void RequiredNullableShort(short? arg)
            {
            }

            public static void RequiredNullableSByte(sbyte? arg)
            {
            }

            public static void RequiredNullableByte(byte? arg)
            {
            }

            public static void RequiredNullableULong(ulong? arg)
            {
            }

            public static void RequiredNullableUShort(ushort? arg)
            {
            }

            public static void RequiredNullableUInt(uint? arg)
            {
            }

            public static void RequiredNullableFloat(float? arg)
            {
            }

            public static void RequiredNullableEnum(TestEnum? arg)
            {
            }

            public static void RequiredNullableBool(bool? arg)
            {
            }

            public static void RequiredNullableDateTime(DateTime? arg)
            {
            }

            public static void OptionalNullableIntWithNullDefault(int? arg = null)
            {
            }

            public static void OptionalNullableDoubleWithNullDefault(double? arg = null)
            {
            }

            public static void OptionalNullableDecimalWithNullDefault(decimal? arg = null)
            {
            }

            public static void OptionalNullableLongWithNullDefault(long? arg = null)
            {
            }

            public static void OptionalNullableShortWithNullDefault(short? arg = null)
            {
            }

            public static void OptionalNullableSByteWithNullDefault(sbyte? arg = null)
            {
            }

            public static void OptionalNullableByteWithNullDefault(byte? arg = null)
            {
            }

            public static void OptionalNullableULongWithNullDefault(ulong? arg = null)
            {
            }

            public static void OptionalNullableUShortWithNullDefault(ushort? arg = null)
            {
            }

            public static void OptionalNullableUIntWithNullDefault(uint? arg = null)
            {
            }

            public static void OptionalNullableFloatWithNullDefault(float? arg = null)
            {
            }

            public static void OptionalNullableEnumWithNullDefault(TestEnum? arg = null)
            {
            }

            public static void OptionalNullableBoolWithNullDefault(bool? arg = null)
            {
            }

            public static void OptionalNullableDateTimeWithNullDefault(DateTime? arg = null)
            {
            }

            public static void OptionalNullableIntWithNonNullDefault(int? arg = 1)
            {
            }

            public static void OptionalNullableDoubleWithNonNullDefault(double? arg = 1)
            {
            }

            public static void OptionalNullableDecimalWithNonNullDefault(decimal? arg = 1)
            {
            }

            public static void OptionalNullableLongWithNonNullDefault(long? arg = 1)
            {
            }

            public static void OptionalNullableShortWithNonNullDefault(short? arg = 1)
            {
            }

            public static void OptionalNullableSByteWithNonNullDefault(sbyte? arg = 1)
            {
            }

            public static void OptionalNullableByteWithNonNullDefault(byte? arg = 1)
            {
            }

            public static void OptionalNullableULongWithNonNullDefault(ulong? arg = 1)
            {
            }

            public static void OptionalNullableUShortWithNonNullDefault(ushort? arg = 1)
            {
            }

            public static void OptionalNullableUIntWithNonNullDefault(uint? arg = 1)
            {
            }

            public static void OptionalNullableFloatWithNonNullDefault(float? arg = 1)
            {
            }

            public static void OptionalNullableEnumWithNonNullDefault(TestEnum? arg = TestEnum.Some)
            {
            }

            public static void OptionalNullableBoolWithNonNullDefault(bool? arg = true)
            {
            }

            public static void RequiredIntKeyword(int @new)
            {
            }

            public static void RequiredNullableIntKeyword(int? @new)
            {
            }

            public static void OptionalIntKeywordWithNonNullDefault(int @new = 1)
            {
            }

            public static void OptionalNullableIntKeywordWithNullDefault(int? @new = null)
            {
            }

            public static void OutputParameterInterface(out IDisposable arg)
            {
                arg = null;
            }

            public static void OutputParameterInt32(out int arg)
            {
                arg = 0;
            }

            public static void OptionalStringWithAttribute([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
            {
            }

            public static void OptionalInt32WithAttribute([CallerLineNumber] int sourceLineNumber = 0)
            {
            }

            public static void RequiredStringWithCustomAttributeCalledWithInt32CtorParameter([TestParameter(19)] string value)
            {
            }

            public static void RequiredStringWithCustomAttributeCalledWithStringCtorParameter([TestParameter("test")] string value)
            {
            }

            public static void RequiredStringWithCustomAttributeCalledWithEnumCtorParameter([TestParameter(TestEnum.All)] string value)
            {
            }

            public static void RequiredStringWithCustomAttributeCalledWithArrayCtorParameter([TestParameter(new[] { TestEnum.All, TestEnum.Some })] string value)
            {
            }

            public static void RequiredStringWithCustomAttributeCalledWithInt32NamedArgument([TestParameter(Int32Property = 19)] string value)
            {
            }

            public static void RequiredStringWithCustomAttributeCalledWithStringNamedArgument([TestParameter(StringProperty = "test")] string value)
            {
            }

            public static void RequiredStringWithCustomAttributeCalledWithEnumNamedArgument([TestParameter(EnumProperty = TestEnum.All)] string value)
            {
            }
        }

        [Theory]
        [InlineData("RequiredInt", "System.Int32 arg")]
        [InlineData("RequiredDouble", "System.Double arg")]
        [InlineData("RequiredDecimal", "System.Decimal arg")]
        [InlineData("RequiredLong", "System.Int64 arg")]
        [InlineData("RequiredShort", "System.Int16 arg")]
        [InlineData("RequiredSByte", "System.SByte arg")]
        [InlineData("RequiredByte", "System.Byte arg")]
        [InlineData("RequiredULong", "System.UInt64 arg")]
        [InlineData("RequiredUShort", "System.UInt16 arg")]
        [InlineData("RequiredUInt", "System.UInt32 arg")]
        [InlineData("RequiredFloat", "System.Single arg")]
        [InlineData("RequiredEnum", "Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum arg")]
        [InlineData("RequiredBool", "System.Boolean arg")]
        [InlineData("RequiredString", "System.String arg")]
        [InlineData("RequiredObject", "System.Object arg")]
        [InlineData("RequiredDateTime", "System.DateTime arg")]
        [InlineData("RequiredInterface", "System.IDisposable arg")]
        [InlineData("RequiredClass", "Cake.Core.CakeContext arg")]
        [InlineData("RequiredType", "System.Type type")]
        [InlineData("RequiredGenericEnumerable", "System.Collections.Generic.IEnumerable<System.String> items")]
        [InlineData("OptionalInt", "System.Int32 arg = (System.Int32)1")]
        [InlineData("OptionalDouble", "System.Double arg = (System.Double)1")]
        [InlineData("OptionalDecimal", "System.Decimal arg = (System.Decimal)1")]
        [InlineData("OptionalLong", "System.Int64 arg = (System.Int64)1")]
        [InlineData("OptionalShort", "System.Int16 arg = (System.Int16)1")]
        [InlineData("OptionalSByte", "System.SByte arg = (System.SByte)1")]
        [InlineData("OptionalByte", "System.Byte arg = (System.Byte)1")]
        [InlineData("OptionalULong", "System.UInt64 arg = (System.UInt64)1")]
        [InlineData("OptionalUShort", "System.UInt16 arg = (System.UInt16)1")]
        [InlineData("OptionalUInt", "System.UInt32 arg = (System.UInt32)1")]
        [InlineData("OptionalFloat", "System.Single arg = (System.Single)1")]
        [InlineData("OptionalEnum", "Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum arg = (Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum)2")]
        [InlineData("OptionalBool", "System.Boolean arg = true")]
        [InlineData("OptionalString", "System.String arg = \"value\"")]
        [InlineData("OptionalObject", "System.Object arg = null")]
        [InlineData("OptionalInterface", "System.IDisposable arg = null")]
        [InlineData("OptionalClass", "Cake.Core.CakeContext arg = null")]
        [InlineData("OptionalType", "System.Type type = null")]
        [InlineData("OptionalGenericEnumerable", "System.Collections.Generic.IEnumerable<System.String> items = null")]
        [InlineData("RequiredNullableInt", "System.Nullable<System.Int32> arg")]
        [InlineData("RequiredNullableDouble", "System.Nullable<System.Double> arg")]
        [InlineData("RequiredNullableDecimal", "System.Nullable<System.Decimal> arg")]
        [InlineData("RequiredNullableLong", "System.Nullable<System.Int64> arg")]
        [InlineData("RequiredNullableShort", "System.Nullable<System.Int16> arg")]
        [InlineData("RequiredNullableSByte", "System.Nullable<System.SByte> arg")]
        [InlineData("RequiredNullableByte", "System.Nullable<System.Byte> arg")]
        [InlineData("RequiredNullableULong", "System.Nullable<System.UInt64> arg")]
        [InlineData("RequiredNullableUShort", "System.Nullable<System.UInt16> arg")]
        [InlineData("RequiredNullableUInt", "System.Nullable<System.UInt32> arg")]
        [InlineData("RequiredNullableFloat", "System.Nullable<System.Single> arg")]
        [InlineData("RequiredNullableEnum", "System.Nullable<Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum> arg")]
        [InlineData("RequiredNullableBool", "System.Nullable<System.Boolean> arg")]
        [InlineData("RequiredNullableDateTime", "System.Nullable<System.DateTime> arg")]
        [InlineData("OptionalNullableIntWithNullDefault", "System.Nullable<System.Int32> arg = null")]
        [InlineData("OptionalNullableDoubleWithNullDefault", "System.Nullable<System.Double> arg = null")]
        [InlineData("OptionalNullableDecimalWithNullDefault", "System.Nullable<System.Decimal> arg = null")]
        [InlineData("OptionalNullableLongWithNullDefault", "System.Nullable<System.Int64> arg = null")]
        [InlineData("OptionalNullableShortWithNullDefault", "System.Nullable<System.Int16> arg = null")]
        [InlineData("OptionalNullableSByteWithNullDefault", "System.Nullable<System.SByte> arg = null")]
        [InlineData("OptionalNullableByteWithNullDefault", "System.Nullable<System.Byte> arg = null")]
        [InlineData("OptionalNullableULongWithNullDefault", "System.Nullable<System.UInt64> arg = null")]
        [InlineData("OptionalNullableUShortWithNullDefault", "System.Nullable<System.UInt16> arg = null")]
        [InlineData("OptionalNullableUIntWithNullDefault", "System.Nullable<System.UInt32> arg = null")]
        [InlineData("OptionalNullableFloatWithNullDefault", "System.Nullable<System.Single> arg = null")]
        [InlineData("OptionalNullableEnumWithNullDefault", "System.Nullable<Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum> arg = null")]
        [InlineData("OptionalNullableBoolWithNullDefault", "System.Nullable<System.Boolean> arg = null")]
        [InlineData("OptionalNullableDateTimeWithNullDefault", "System.Nullable<System.DateTime> arg = null")]
        [InlineData("OptionalNullableIntWithNonNullDefault", "System.Nullable<System.Int32> arg = (System.Int32)1")]
        [InlineData("OptionalNullableDoubleWithNonNullDefault", "System.Nullable<System.Double> arg = (System.Double)1")]
        [InlineData("OptionalNullableDecimalWithNonNullDefault", "System.Nullable<System.Decimal> arg = (System.Decimal)1")]
        [InlineData("OptionalNullableLongWithNonNullDefault", "System.Nullable<System.Int64> arg = (System.Int64)1")]
        [InlineData("OptionalNullableShortWithNonNullDefault", "System.Nullable<System.Int16> arg = (System.Int16)1")]
        [InlineData("OptionalNullableSByteWithNonNullDefault", "System.Nullable<System.SByte> arg = (System.SByte)1")]
        [InlineData("OptionalNullableByteWithNonNullDefault", "System.Nullable<System.Byte> arg = (System.Byte)1")]
        [InlineData("OptionalNullableULongWithNonNullDefault", "System.Nullable<System.UInt64> arg = (System.UInt64)1")]
        [InlineData("OptionalNullableUShortWithNonNullDefault", "System.Nullable<System.UInt16> arg = (System.UInt16)1")]
        [InlineData("OptionalNullableUIntWithNonNullDefault", "System.Nullable<System.UInt32> arg = (System.UInt32)1")]
        [InlineData("OptionalNullableFloatWithNonNullDefault", "System.Nullable<System.Single> arg = (System.Single)1")]
        [InlineData("OptionalNullableEnumWithNonNullDefault", "System.Nullable<Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum> arg = (Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum)1")]
        [InlineData("OptionalNullableBoolWithNonNullDefault", "System.Nullable<System.Boolean> arg = (System.Boolean)true")]
        [InlineData("RequiredIntKeyword", "System.Int32 @new")]
        [InlineData("RequiredNullableIntKeyword", "System.Nullable<System.Int32> @new")]
        [InlineData("OptionalIntKeywordWithNonNullDefault", "System.Int32 @new = (System.Int32)1")]
        [InlineData("OptionalNullableIntKeywordWithNullDefault", "System.Nullable<System.Int32> @new = null")]
        [InlineData("OutputParameterInterface", "out System.IDisposable arg")]
        [InlineData("OutputParameterInt32", "out System.Int32 arg")]
        [InlineData("OptionalStringWithAttribute", "[System.Runtime.CompilerServices.CallerMemberName] System.String memberName = \"\"")]
        [InlineData("OptionalInt32WithAttribute", "[System.Runtime.CompilerServices.CallerLineNumber] System.Int32 sourceLineNumber = (System.Int32)0")]
        [InlineData("RequiredStringWithCustomAttributeCalledWithInt32CtorParameter", "[Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestParameter((System.Int32)19)] System.String value")]
        [InlineData("RequiredStringWithCustomAttributeCalledWithStringCtorParameter", "[Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestParameter(\"test\")] System.String value")]
        [InlineData("RequiredStringWithCustomAttributeCalledWithEnumCtorParameter", "[Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestParameter((Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum)2)] System.String value")]
        [InlineData("RequiredStringWithCustomAttributeCalledWithArrayCtorParameter", "[Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestParameter(new Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum[2] { (Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum)2, (Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum)1 })] System.String value")]
        [InlineData("RequiredStringWithCustomAttributeCalledWithInt32NamedArgument", "[Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestParameter(Int32Property = (System.Int32)19)] System.String value")]
        [InlineData("RequiredStringWithCustomAttributeCalledWithStringNamedArgument", "[Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestParameter(StringProperty = \"test\")] System.String value")]
        [InlineData("RequiredStringWithCustomAttributeCalledWithEnumNamedArgument", "[Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestParameter(EnumProperty = (Cake.Core.Tests.Unit.Scripting.CodeGen.ParameterEmitterTests.TestEnum)2)] System.String value")]
        public void Should_Return_Correct_Generated_Code_For_Method_Parameters(string methodName, string expected)
        {
            // Given
            var method = typeof(ParameterFixture).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            var parameter = method.GetParameters().FirstOrDefault();

            // When
            var result = ParameterEmitter.Emit(parameter, true);

            // Then
            Assert.Equal(expected, result);
        }
    }
}
