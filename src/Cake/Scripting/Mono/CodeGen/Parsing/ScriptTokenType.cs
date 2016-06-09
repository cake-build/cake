// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Scripting.Mono.CodeGen.Parsing
{
    internal enum ScriptTokenType
    {
        Word = 0,
        If = 1,
        Else = 2,
        While = 3,
        Switch = 4,
        String = 5,
        Semicolon = 6,
        LeftBrace = 7,
        RightBrace = 8,
        LeftParenthesis = 9,
        RightParenthesis = 10,
        Character = 11
    }
}
