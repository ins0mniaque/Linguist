using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Linguist
{
    public class TemplateEngine : Dictionary < string, string >
    {
        private static readonly Regex engine = new Regex ( @"\{(.+?)\}", RegexOptions.Compiled );

        public static bool HasArguments ( string template )
        {
            return ! string.IsNullOrEmpty ( template ) && engine.IsMatch ( template );
        }

        public TemplateEngine ( string template ) : base ( StringComparer.InvariantCultureIgnoreCase )
        {
            Template = template;
        }

        public string Template { get; set; }

        public override string ToString ( )
        {
            return engine.Replace ( Template, ReplaceArgument );
        }

        private string ReplaceArgument ( Match match )
        {
            var name = match.Groups [ 1 ].Value;
            if ( TryGetValue ( name, out var value ) )
                return value;

            return match.Value;
        }
    }
}