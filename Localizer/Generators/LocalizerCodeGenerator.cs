using System;
using System.CodeDom;
using System.Runtime.InteropServices;

namespace Localizer
{
    [ Guid ( "86CFFC91-69EF-44B7-83EE-937ED31C9054" ) ]
    public sealed class LocalizerCodeGenerator : BaseLocalizerCodeGenerator
    {
        public const string Name        = nameof ( LocalizerCodeGenerator );
        public const string Description = "Generates a public strongly-typed resource class for localization.";

        protected override MemberAttributes AccessModifier => MemberAttributes.Public | MemberAttributes.Static;
    }
}