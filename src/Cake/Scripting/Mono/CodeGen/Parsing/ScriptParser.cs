// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Scripting.Mono.CodeGen.Parsing
{
    internal sealed class ScriptParser : IDisposable
    {
        private readonly string _content;
        private readonly ScriptTokenizer _tokenizer;
        private ScriptToken _current;
        private ScriptToken _start;

        public ScriptParser(string content)
        {
            _content = content;
            _tokenizer = new ScriptTokenizer(content);
            _current = _tokenizer.GetNextToken();
        }

        public void Dispose()
        {
            _tokenizer.Dispose();
        }

        public ScriptBlock ParseNext()
        {
            if (_current == null)
            {
                return null;
            }

            _start = _current;

            while (true)
            {
                switch (_current.Type)
                {
                    case ScriptTokenType.Semicolon:
                    {
                        var block = CreateBlock(false);
                        Read(); // Nom nom nom
                        return block;
                    }
                    case ScriptTokenType.LeftParenthesis:
                    {
                        SkipBlock(ScriptTokenType.LeftParenthesis, ScriptTokenType.RightParenthesis);
                        break;
                    }
                    case ScriptTokenType.LeftBrace:
                    {
                        return ParseScope();
                    }
                    case ScriptTokenType.If:
                    {
                        return ParseStatement(ScriptTokenType.If);
                    }
                    case ScriptTokenType.Else:
                    {
                        return ParseStatement(ScriptTokenType.Else);
                    }
                    case ScriptTokenType.While:
                    {
                        return ParseStatement(ScriptTokenType.While);
                    }
                    case ScriptTokenType.Switch:
                    {
                        return ParseStatement(ScriptTokenType.Switch);
                    }
                }

                if (!Read())
                {
                    return CreateBlock(false);
                }
            }
        }

        private ScriptBlock ParseScope()
        {
            SkipBlock(ScriptTokenType.LeftBrace, ScriptTokenType.RightBrace);
            var current = _current;
            if (Read())
            {
                if (_current.Type == ScriptTokenType.Semicolon)
                {
                    var block = CreateBlock(false);
                    Read(); // nom nom
                    return block;
                }
            }
            return CreateBlock(current, true);
        }

        private void SkipBlock(ScriptTokenType start, ScriptTokenType stop)
        {
            if (_current.Type != start)
            {
                throw new InvalidOperationException("Expected block.");
            }

            var nesting = 0;
            while (true)
            {
                if (_current.Type == start)
                {
                    nesting++;
                }
                if (_current.Type == stop)
                {
                    nesting--;
                }
                if (nesting == 0)
                {
                    break;
                }
                if (!Read())
                {
                    break;
                }
            }
        }

        private ScriptBlock ParseStatement(ScriptTokenType type)
        {
            switch (type)
            {
                case ScriptTokenType.If:
                case ScriptTokenType.While:
                {
                    return ParseConditionalScope();
                }
                case ScriptTokenType.Switch:
                {
                    return ParseConditionalScope(requireBraces: true);
                }
                case ScriptTokenType.Else:
                {
                    return ParseElseStatement();
                }
            }
            throw new InvalidOperationException("Unknown statement type.");
        }

        private ScriptBlock ParseElseStatement()
        {
            // Store a reference to the current token.
            var start = _current;
            Read();

            // Is this an else-if statement?
            // If it is, parse the if statement, otherwise parse the scope.
            ScriptBlock scope;
            if (_current.Type == ScriptTokenType.If)
            {
                // Parse "else if" statement.
                scope = ParseStatement(ScriptTokenType.If);
            }
            else
            {
                // Parse scope or statement.
                var nextIsBrace = _current.Type == ScriptTokenType.LeftBrace;
                scope = nextIsBrace ? ParseScope() : ParseNext();
            }

            // Create a new script block using
            // the start token and the parsed scope.
            return CreateBlock(start, scope);
        }

        private ScriptBlock ParseConditionalScope(bool requireBraces = false)
        {
            // Store a reference to the start token.
            var start = _current;
            Read();

            // Skip the parenthesis.
            SkipBlock(ScriptTokenType.LeftParenthesis, ScriptTokenType.RightParenthesis);
            Read();

            // Parse the scope.
            var nextIsBrace = _current.Type == ScriptTokenType.LeftBrace;
            var shouldParseScope = nextIsBrace || requireBraces;
            var scope = shouldParseScope ? ParseScope() : ParseNext();

            // Create a new script block using
            // the start token and the parsed scope.
            return CreateBlock(start, scope);
        }

        private ScriptBlock CreateBlock(bool hasScope)
        {
            return CreateBlock(_current, hasScope);
        }

        private ScriptBlock CreateBlock(ScriptToken current, bool hasScope)
        {
            var content = _content.Substring(_start.Index, current.Index + current.Length - _start.Index);
            return new ScriptBlock(_start.Index, content, hasScope);
        }

        private ScriptBlock CreateBlock(ScriptToken start, ScriptBlock scope)
        {
            var startIndex = start.Index;
            var endIndex = scope.Index + scope.Content.Length;
            var content = _content.Substring(startIndex, endIndex - startIndex);
            return new ScriptBlock(startIndex, content, false);
        }

        private bool Read()
        {
            _current = _tokenizer.GetNextToken();
            return _current != null;
        }
    }
}
