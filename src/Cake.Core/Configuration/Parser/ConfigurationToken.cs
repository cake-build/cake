namespace Cake.Core.Configuration.Parser
{
    internal sealed class ConfigurationToken
    {
        private readonly ConfigurationTokenKind _kind;
        private readonly string _value;

        public ConfigurationTokenKind Kind
        {
            get { return _kind; }
        }

        public string Value
        {
            get { return _value; }
        }

        public ConfigurationToken(ConfigurationTokenKind kind, string value)
        {
            _kind = kind;
            _value = value;
        }
    }
}