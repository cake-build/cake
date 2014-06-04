namespace Cake.Diagnostics.Formatting
{
    internal sealed class LiteralToken : FormatToken
    {
        private readonly string _text;

        public string Text
        {
            get { return _text; }
        }

        public LiteralToken(string text)
        {
            _text = text;
        }

        public override string Render(object[] args)
        {
            return _text;
        }
    }
}