using System;
using System.Collections.Generic;

namespace Cake.Arguments
{
    /// <summary>
    ///     Parses the command line in unix style.
    /// </summary>
    public class UnixCommandParser
    {
        /// <summary>
        ///     Parses the specified commandline.
        /// </summary>
        /// <param name="commandline">The arguments of the command line.</param>
        /// <returns>A List of all key-values found.</returns>
        /// <remarks>
        /// Symbol language
        /// arg_char               ::= [a-zA-Z0-9_-]
        /// arg_single_key         ::=  -&lt;arg_char&gt;
        /// arg_multi_key          ::= --&lt;arg_char&gt;&lt;arg_char&gt;+
        /// arg_key                ::= (&lt;arg_single_key&gt;|&lt;arg_multi_key&gt;)
        /// arg_value_no_quote     ::= [a-zA-Z0-9_][a-zA-Z0-9_-]*
        /// arg_value_single_quote ::= '&lt;escaped_single_quote_string&gt;'
        /// arg_value_multi_quote  ::= "&lt;escaped_multi_quote_string&gt;"
        /// arg_value              ::= (&lt;arg_value_single_quote&gt;|&lt;arg_value_multi_quote&gt;|&lt;arg_value_no_quote&gt;)
        /// argument               ::= (&lt;arg_key&gt;=&lt;arg_value&gt;|&lt;arg_key&gt;)
        /// argument_string        ::= (&lt;argument&gt; )*&lt;argument&gt;
        /// </remarks>
        public List<KeyValuePair<string, string>> Parse(string commandline)
        {
            // If there is no information on the command line then we can safely skip it.
            if (string.IsNullOrWhiteSpace(commandline))
            {
                return new List<KeyValuePair<string, string>>();
            }

            var result = new List<KeyValuePair<string, string>>();
            TokenType mode = TokenType.Nothing;
            int pos = 0;

            var stack = new Stack<Tuple<TokenType, string>>();

            while (pos < commandline.Length)
            {
                var current_char = commandline[pos];
                switch (mode)
                {
                    case TokenType.Nothing:
                    {
                        mode = HandleNothing(current_char, ref mode, ref pos);
                    }
                        break;
                    case TokenType.ArgumentKey:
                    {
                        ProcessStack(stack, result);
                        mode = ParseKey(commandline, stack, ref pos);
                    }
                        break;
                    case TokenType.ArgumentValue:
                    {
                        bool is_single_quoted = current_char == '\'';
                        bool is_double_quoted = current_char == '"';

                        if (is_single_quoted || is_double_quoted)
                        {
                            string argument = ParseArgumentValue(current_char, commandline, ref pos);
                            stack.Push(new Tuple<TokenType, string>(TokenType.ArgumentValue, argument));
                        }
                        else
                        {
                            string argument = ParseArgumentValue(null, commandline, ref pos);
                            stack.Push(new Tuple<TokenType, string>(TokenType.ArgumentValue, argument));
                        }

                        mode = TokenType.Nothing;
                    }
                        break;
                    case TokenType.ArgumentKeyTerminator:
                    case TokenType.WhiteSpace:
                    case TokenType.EOF:
                    {
                        // Unhandeled whitespace is considered nothing.
                        mode = TokenType.Nothing;
                    }
                        break;
                    default:
                    {
                        throw new NotImplementedException("We don't support mode " + mode + " yet");
                    }
                }
            }

            ProcessStack(stack, result);

            return result;
        }

        private static TokenType ParseKey(string commandline, Stack<Tuple<TokenType, string>> stack, ref int pos)
        {
            TokenType currentType = TokenType.ArgumentKey;
            string argument = string.Empty;
            while (currentType == TokenType.ArgumentKey)
            {
                if (pos >= commandline.Length)
                {
                    currentType = TokenType.EOF;
                    break;
                }
                var currentChar = commandline[pos];
                currentType = GetTokenType(currentChar, TokenType.ArgumentKey);
                if (currentType == TokenType.ArgumentKey)
                {
                    argument += currentChar;
                    pos++;
                }
                else if (currentType == TokenType.ArgumentKeyTerminator)
                {
                    pos++;
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(argument))
            {
                stack.Push(new Tuple<TokenType, string>(TokenType.ArgumentKey, argument));
            }

            return currentType;
        }

        private static string ParseArgumentValue(char? quoteCharacter, string commandline, ref int pos)
        {
            string argument = string.Empty;

            // Skip the first character as it is part of the quote.
            if (quoteCharacter != null)
            {
                pos += 1;
            }
            while (true)
            {
                if (quoteCharacter.HasValue)
                {
                    if (pos == commandline.Length)
                    {
                        throw new Exception("Unbalanced quoted argument: Missing quote terminator");
                    }
                }
                else
                {
                    if (pos == commandline.Length || char.IsWhiteSpace(commandline[pos]))
                    {
                        return argument;
                    }
                }

                var currentChar = commandline[pos];
                if (currentChar == '\\')
                {
                    var nextPos = pos + 1;

                    if (nextPos == commandline.Length)
                    {
                        throw new Exception("Unbalanced quoted argument: EOF can't be escaped");
                    }

                    var nextChar = commandline[nextPos];
                    argument += nextChar;
                    pos = nextPos + 1;
                    continue;
                }

                if (quoteCharacter.HasValue && currentChar == quoteCharacter.Value)
                {
                    pos += 1;
                    return argument;
                }

                argument += currentChar;
                pos += 1;
            }
        }

        private static void ProcessStack(Stack<Tuple<TokenType, string>> stack,
            List<KeyValuePair<string, string>> result)
        {
            while (stack.Count != 0)
            {
                var current = stack.Pop();
                switch (current.Item1)
                {
                    case TokenType.ArgumentKey:
                        // Add it directly.
                        result.Add(new KeyValuePair<string, string>(current.Item2, null));
                        break;
                    case TokenType.ArgumentValue:
                        if (stack.Count == 0)
                        {
                            throw new Exception("An \"--key\" should preceed the value.");
                        }

                        var expectedKey = stack.Pop();
                        if (expectedKey.Item1 != TokenType.ArgumentKey)
                        {
                            // In the event another value is precedeeing this value.
                            throw new Exception("An \"--key\" should preceed the value.");
                        }

                        result.Add(new KeyValuePair<string, string>(expectedKey.Item2, current.Item2));
                        break;
                    default:
                        throw new NotImplementedException("Unable to handle the stack");
                }
            }
        }

        private static TokenType HandleNothing(char currentChar, ref TokenType mode, ref int pos)
        {
            var currentType = GetTokenType(currentChar, mode);
            switch (currentType)
            {
                case TokenType.WhiteSpace:
                    pos++;
                    return mode;
                case TokenType.ArgumentKey:
                    mode = TokenType.ArgumentKey;
                    break;
            }

            return currentType;
        }

        private static TokenType GetTokenType(char currentChar, TokenType mode)
        {
            if (char.IsWhiteSpace(currentChar))
            {
                return TokenType.WhiteSpace;
            }

            if (currentChar == '=' && mode == TokenType.ArgumentKey)
            {
                return TokenType.ArgumentKeyTerminator;
            }

            if (currentChar == '-' || mode == TokenType.ArgumentKey)
            {
                return TokenType.ArgumentKey;
            }

            return TokenType.ArgumentValue;
        }

        private enum TokenType
        {
            Nothing,
            WhiteSpace,
            ArgumentKey,
            ArgumentKeyTerminator,
            ArgumentValue,
            EOF,
        }
    }
}