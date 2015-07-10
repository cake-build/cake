using System;
using System.IO;

namespace Cake.Scripting.Mono.CodeGen.Parsing
{
    internal sealed class CodeBlockParser
    {
        private readonly string _code;
        private readonly StringReader _reader;
        private int _position;
        private int _start;
        private char _current;

        public CodeBlockParser(string code)
        {
            _code = code;
            _reader = new StringReader(_code);
            _current = (char)_reader.Peek();
            _position = -1;
        }

        public CodeBlock GetBlock()
        {
            if (!Accept())
            {
                return null;
            }

            while (true)
            {
                if (!char.IsWhiteSpace(_current))
                {
                    break;
                }
                if (!Accept())
                {
                    return null;
                }
            }

            _start = _position;

            while (true)
            {
                switch (_current)
                {
                    case ';':
                        return CreateBlock(false);
                    case '(':
                        SkipBlock('(', ')');
                        break;
                    case '{':
                        SkipBlock('{', '}');
                        return CreateBlock(true);
                }

                if (!Accept())
                {
                    return CreateBlock(false);
                }
            }
        }

        private void SkipBlock(char start, char stop)
        {
            var count = 0;
            while (true)
            {
                if (_current == -1)
                {
                    throw new InvalidOperationException("Could not parse script code.");
                }
                if (_current == start)
                {
                    count++;
                }
                if (_current == stop)
                {
                    count--;
                }
                if (count == 0)
                {
                    break;
                }
                Accept();
            }
        }

        private bool Accept()
        {
            if (_reader.Peek() == -1)
            {
                return false;
            }

            _current = (char)_reader.Read();
            _position += 1;

            return true;
        }

        private CodeBlock CreateBlock(bool hasScope)
        {
            var block = new CodeBlock();
            block.HasScope = hasScope;
            block.Start = _start;
            block.End = _position;
            block.Content = _code.Substring(_start, _position - _start + 1);
            _start = _position;
            return block;
        }
    }
}
