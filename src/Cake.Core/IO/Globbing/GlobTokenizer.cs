// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System;
using System.Text.RegularExpressions;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobTokenizer
    {
        private readonly string _pattern;
        private readonly Regex _identifierRegex;
        private int _sourceIndex;
        private char _currentCharacter;
        private string _currentContent;
        private GlobTokenKind _currentKind;

        public GlobTokenizer(string pattern)
        {
            _pattern = pattern;
            _sourceIndex = 0;
            _currentContent = string.Empty;
            _currentCharacter = _pattern[_sourceIndex];
            _identifierRegex = new Regex("^[0-9a-zA-Z\\+&(). _-]$", RegexOptions.Compiled);
        }

        public GlobToken Scan()
        {
            _currentContent = string.Empty;
            _currentKind = ScanToken();

            return new GlobToken(_currentKind, _currentContent);
        }

        public GlobToken Peek()
        {
            var index = _sourceIndex;
            var token = Scan();
            _sourceIndex = index;
            _currentCharacter = _pattern[_sourceIndex];
            return token;
        }

        private GlobTokenKind ScanToken()
        {
            if (IsAlphaNumeric(_currentCharacter))
            {
                if (_currentCharacter == '.')
                {
                    TakeCharacter();
                    if (_currentCharacter == '.')
                    {
                        TakeCharacter();
                        return GlobTokenKind.Parent;
                    }
                }
                while (IsAlphaNumeric(_currentCharacter))
                {
                    TakeCharacter();
                }
                return GlobTokenKind.Identifier;
            }

            if (_currentCharacter == '*')
            {
                TakeCharacter();
                if (_currentCharacter == '*')
                {
                    TakeCharacter();
                    return GlobTokenKind.DirectoryWildcard;
                }
                return GlobTokenKind.Wildcard;
            }
            if (_currentCharacter == '?')
            {
                TakeCharacter();
                return GlobTokenKind.CharacterWildcard;
            }
            if (_currentCharacter == '/' || _currentCharacter == '\\')
            {
                TakeCharacter();
                return GlobTokenKind.PathSeparator;
            }
            if (_currentCharacter == ':')
            {
                TakeCharacter();
                return GlobTokenKind.WindowsRoot;
            }
            if (_currentCharacter == '\0')
            {
                return GlobTokenKind.EndOfText;
            }

            throw new NotSupportedException("Unknown token");
        }

        private bool IsAlphaNumeric(char character)
        {
            return _identifierRegex.IsMatch(character.ToString());
        }

        private void TakeCharacter()
        {
            if (_currentCharacter == '\0')
            {
                return;
            }

            _currentContent += _currentCharacter;
            if (_sourceIndex == _pattern.Length - 1)
            {
                _currentCharacter = '\0';
                return;
            }

            _currentCharacter = _pattern[++_sourceIndex];
        }
    }
}
