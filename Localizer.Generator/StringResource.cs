namespace Localizer.Generator
{
    public class StringResource : Resource
    {
        public StringResource ( string type, string value, string method, bool castToType, string comment = null ) : base ( type, method, castToType, comment )
        {
            Value = value;
        }

        public string Value { get; }
    }
}