namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobToken
    {
        private readonly GlobTokenKind _kind;
        private readonly string _value;

        public GlobTokenKind Kind
        {
            get { return _kind; }
        }

        public string Value
        {
            get { return _value; }
        }

        public GlobToken(GlobTokenKind kind, string value)
        {
            _kind = kind;
            _value = value;
        }
    }
}