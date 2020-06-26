using System;
using System.Collections.Generic;

namespace Cake.Core.Diagnostics.Formatting
{
    internal class CharReader
    {
        private readonly char[] chars;
        private readonly int length;
        private int position;

        public CharReader(string s)
        {
            chars = s?.ToCharArray() ?? throw new ArgumentNullException(nameof(s));
            length = chars.Length;
            position = 0;
        }

        public int Read()
        {
            if (position == length)
            {
                return -1;
            }

            return chars[position++];
        }

        public int Peek()
        {
            if (position == length)
            {
                return -1;
            }
            return chars[position];
        }

        public IEnumerable<char> Peek(int count)
        {
            if (position == length)
            {
                yield break;
            }

            for (var index = position; index < length && count > 0; index++, count--)
            {
                yield return chars[index];
            }
        }
    }
}