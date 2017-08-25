// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobTokenizer
    {
        private readonly Dictionary<string, Func<GlobTokenKind>> _tokenKindChars = new Dictionary<string, Func<GlobTokenKind>>();
        private readonly Lazy<Queue<GlobToken>> _tokens;
        private string _remainingPattern;
        private string _currentContent;

        public GlobTokenizer(string pattern)
        {
            _remainingPattern = pattern;
            _tokens = new Lazy<Queue<GlobToken>>(QueueTokens);

            _tokenKindChars.Add("?",  () => GlobTokenKind.CharacterWildcard);
            _tokenKindChars.Add("*",  () => GlobTokenKind.Wildcard);
            _tokenKindChars.Add("**", () => GlobTokenKind.DirectoryWildcard);
            _tokenKindChars.Add("/",  () => GlobTokenKind.PathSeparator);
            _tokenKindChars.Add(@"\", () => GlobTokenKind.PathSeparator);
            _tokenKindChars.Add(":",  () => GlobTokenKind.WindowsRoot);
            _tokenKindChars.Add(".", HandlePeriod);
            _tokenKindChars.Add("..", () => GlobTokenKind.Parent);
            _tokenKindChars.Add("\0", () => GlobTokenKind.EndOfText);
        }

        /// <summary>
        /// Gets the next token from the pattern.
        /// </summary>
        /// <returns>The next GlobToken.</returns>
        public GlobToken Scan()
        {
            if (_tokens.Value.Count == 0)
            {
                return null;
            }

            return _tokens.Value.Dequeue();
        }

        /// <summary>
        /// Peeks the next token from the pattern.
        /// </summary>
        /// <returns>The peek'd GlobToken.</returns>
        public GlobToken Peek()
        {
            return _tokens.Value.Peek();
        }

        /// <summary>
        /// Loads the tokens into the token queue.
        /// </summary>
        /// <returns>A queue of tokens representing the pattern.</returns>
        private Queue<GlobToken> QueueTokens()
        {
            var tokenQueue = new Queue<GlobToken>();

            while (_remainingPattern.Length > 0)
            {
                GlobTokenKind tokenKind = GetGlobTokenKindAndTrimRemainingPattern();

                if (tokenKind != GlobTokenKind.Identifier)
                {
                    tokenQueue.Enqueue(new GlobToken(tokenKind, string.Empty));
                    continue;
                }

                while (tokenKind == GlobTokenKind.Identifier)
                {
                    if (!string.IsNullOrEmpty(_remainingPattern))
                    {
                        tokenKind = GetGlobTokenKindAndTrimRemainingPattern();
                    }
                    else
                    {
                        break;
                    }
                }

                // We know we've got at least one Identifer, so queue it with the contents read
                tokenQueue.Enqueue(new GlobToken(GlobTokenKind.Identifier, _currentContent));
                _currentContent = string.Empty;

                // If we quit the while loop due to hitting a new token (rather than end of string), queue it.
                if (tokenKind != GlobTokenKind.Identifier)
                {
                    tokenQueue.Enqueue(new GlobToken(tokenKind, string.Empty));
                }
            }

            return tokenQueue;
        }

        /// <summary>
        /// Searches the private dictionary for a set of tokens matching the current (and future) character position(s).
        /// Performs a greedy match of the keys in the dictionary against the remaining pattern.
        /// </summary>
        /// <returns>The GlobTokenKind associated with the mathing entry, or GlobTokenKind.Identifier if none were found. </returns>
        private GlobTokenKind GetGlobTokenKindAndTrimRemainingPattern()
        {
            int numberOfCharsToRemove;
            GlobTokenKind tokenKind;

            var matches = _tokenKindChars.Where(pair => _remainingPattern.IndexOf(pair.Key, StringComparison.Ordinal) == 0);
            var greediestMatch = matches.OrderByDescending(pair => pair.Key.Length).FirstOrDefault();

            if (greediestMatch.Key == null)
            {
                tokenKind = GlobTokenKind.Identifier;
                numberOfCharsToRemove = 1;
            }
            else
            {
                tokenKind = greediestMatch.Value();
                numberOfCharsToRemove = greediestMatch.Key.Length;
            }

            if (tokenKind == GlobTokenKind.Identifier)
            {
                _currentContent += _remainingPattern.Substring(0, numberOfCharsToRemove);
            }

            _remainingPattern = _remainingPattern.Substring(numberOfCharsToRemove);
            return tokenKind;
        }

        /// <summary>
        /// Determine what GlobTokenKind a period represents given the context
        /// </summary>
        /// <returns>The appropriate GlobTokenKind depending on the next character</returns>
        private GlobTokenKind HandlePeriod()
        {
            if (_remainingPattern.Length > 1)
            {
                var nextCharacter = _remainingPattern[1];
                if (nextCharacter == '\\' || nextCharacter == '/')
                {
                    return GlobTokenKind.Current;
                }
            }

            return GlobTokenKind.Identifier;
        }
    }
}