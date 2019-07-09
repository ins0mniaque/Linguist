using System.CodeDom.Compiler;

namespace Localizer.Generator
{
    public class Resource : CompilerError
    {
        public Resource ( string type, string method, bool castToType, string comment = null )
        {
            Type       = type;
            Method     = method;
            CastToType = castToType;
            Comment    = comment;
        }

        public string Type       { get; }
        public string Method     { get; }
        public bool   CastToType { get; }
        public string Comment    { get; }
    }
}