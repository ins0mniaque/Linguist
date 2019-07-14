using System.CodeDom;
using System.CodeDom.Compiler;

namespace Localizer.Generator
{
    public delegate CodeExpression ResourceGetter ( CodeExpression resourceManager, string name, CodeExpression culture );

    public class Resource : CompilerError
    {
        public Resource ( CodeTypeReference type, ResourceGetter getter, string comment = null )
        {
            Type    = type;
            Getter  = getter;
            Comment = comment;
        }

        public CodeTypeReference Type    { get; }
        public ResourceGetter    Getter  { get; }
        public string            Comment { get; }
    }
}