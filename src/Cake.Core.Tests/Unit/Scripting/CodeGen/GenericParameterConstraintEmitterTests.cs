// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.Scripting.CodeGen;
using Xunit;
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
namespace Cake.Core.Tests.Unit.Scripting.CodeGen
{
    public sealed class GenericParameterConstraintEmitterTests
    {
        // ReSharper disable once ClassNeverInstantiated.Local
        private class FakeClass
        {
        }

        private interface IFakeInterface
        {
        }

        private static class GenericParameterConstraintEmitterFixture
        {
            public static TResult Test_TResult_No_Constraints<TResult>()
            {
                throw new NotImplementedException();
            }

            public static TResult Test_TResult_class<TResult>() where TResult : class
            {
                throw new NotImplementedException();
            }

            public static TResult Test_TResult_class_new<TResult>() where TResult : class, new()
            {
                throw new NotImplementedException();
            }

            public static TResult Test_TResult_struct<TResult>() where TResult : struct
            {
                throw new NotImplementedException();
            }

            public static TResult Test_TResult_struct_and_IFakeInterface<TResult>() where TResult : struct, IFakeInterface
            {
                throw new NotImplementedException();
            }

            public static TOut Test_TOut_TIn_NoConstraints<TIn, TOut>(TIn obj)
            {
                throw new NotImplementedException();
            }

            public static TOut Test_TOut_TIn_TIn_is_FakeClass<TIn, TOut>(TIn obj) where TIn : FakeClass
            {
                throw new NotImplementedException();
            }

            public static TOut Test_TOut_TIn_TIn_is_IFakeInterface<TIn, TOut>(TIn obj) where TIn : IFakeInterface
            {
                throw new NotImplementedException();
            }

            public static TOut Test_TOut_TIn_TIn_is_IFakeInterface_and_DefaultCtor<TIn, TOut>(TIn obj) where TIn : IFakeInterface, new()
            {
                throw new NotImplementedException();
            }

            public static TOut Test_TOut_TIn_TIn_is_FakeClass_and_DefaultCtor<TIn, TOut>(TIn obj) where TIn : FakeClass, new()
            {
                throw new NotImplementedException();
            }

            public static TOut Test_TOut_TIn_TIn_is_FakeClass_and_IFakeInterface<TIn, TOut>(TIn obj) where TIn : FakeClass, IFakeInterface
            {
                throw new NotImplementedException();
            }

            public static TOut Test_TOut_TIn_TIn_is_FakeClass_and_IFakeInterface_and_DefaultCtor<TIn, TOut>(TIn obj) where TIn : FakeClass, IFakeInterface, new()
            {
                throw new NotImplementedException();
            }

            public static TOut Test_TOut_TIn_TIn_is_FakeClass_and_IFakeInterface_and_TOut_IsIFakeInterface<TIn, TOut>(TIn obj)
                where TIn : FakeClass, IFakeInterface
                where TOut : IFakeInterface
            {
                throw new NotImplementedException();
            }

            public static void Test_TIn_TIn_is_NestedFakeClass_and_NestedFakeInterface<TIn>(TIn obj) where TIn : Cake.Core.Tests.Unit.Scripting.CodeGen.GenericConstraintFakes.FakeClass, Cake.Core.Tests.Unit.Scripting.CodeGen.GenericConstraintFakes.IFakeInterface
            {
                throw new NotImplementedException();
            }

            public static void Test_TIn_TIn_Is_IEnumerable_int<TIn>(TIn obj)
                where TIn : IEnumerable<int>
            {
                throw new NotImplementedException();
            }
        }

        [Theory]
        [InlineData("Test_TResult_No_Constraints", "")]
        [InlineData("Test_TResult_class", "where TResult : class")]
        [InlineData("Test_TResult_class_new", "where TResult : class, new()")]
        [InlineData("Test_TResult_struct", "where TResult : struct")]
        [InlineData("Test_TResult_struct_and_IFakeInterface", "where TResult : struct, Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.IFakeInterface")]
        [InlineData("Test_TOut_TIn_NoConstraints", "")]
        [InlineData("Test_TOut_TIn_TIn_is_FakeClass", "where TIn : Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.FakeClass")]
        [InlineData("Test_TOut_TIn_TIn_is_IFakeInterface_and_DefaultCtor", "where TIn : Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.IFakeInterface, new()")]
        [InlineData("Test_TOut_TIn_TIn_is_FakeClass_and_DefaultCtor", "where TIn : Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.FakeClass, new()")]
        [InlineData("Test_TOut_TIn_TIn_is_FakeClass_and_IFakeInterface", "where TIn : Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.FakeClass, Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.IFakeInterface")]
        [InlineData("Test_TOut_TIn_TIn_is_FakeClass_and_IFakeInterface_and_DefaultCtor", "where TIn : Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.FakeClass, Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.IFakeInterface, new()")]
        [InlineData("Test_TOut_TIn_TIn_is_FakeClass_and_IFakeInterface_and_TOut_IsIFakeInterface", "where TIn : Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.FakeClass, Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.IFakeInterface\r\nwhere TOut : Cake.Core.Tests.Unit.Scripting.CodeGen.GenericParameterConstraintEmitterTests.IFakeInterface")]
        [InlineData("Test_TIn_TIn_is_NestedFakeClass_and_NestedFakeInterface", "where TIn : Cake.Core.Tests.Unit.Scripting.CodeGen.GenericConstraintFakes.FakeClass, Cake.Core.Tests.Unit.Scripting.CodeGen.GenericConstraintFakes.IFakeInterface")]
        [InlineData("Test_TIn_TIn_Is_IEnumerable_int", "where TIn : System.Collections.Generic.IEnumerable<System.Int32>")]
        public void Should_Return_Correct_Generated_Code_For_Generic_Method_Parameter_Constraints(string methodName, string expected)
        {
            // Given
            var method = typeof(GenericParameterConstraintEmitterFixture).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

            // When
            var result = GenericParameterConstraintEmitter.Emit(method);

            // Then
            Assert.Equal(expected.NormalizeLineEndings(), result.NormalizeLineEndings());
        }
    }
}
