using System.CodeDom;

namespace Linguist.Generator
{
    public class StringResource : Resource
    {
        public StringResource ( CodeTypeReference type, string value, ResourceGetter getter, string comment = null ) : base ( type, getter, comment )
        {
            Value = value;
        }

        public string Value { get; }
    }
}