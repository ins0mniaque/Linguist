using System;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace Linguist.CodeDom
{
    public static class Validator
    {
        private const           string   InvalidIdentifier                 = "Identifier '{0}' is not valid.";
        private const           char     InvalidIdentifierTokenReplacement = '_';
        private static readonly char [ ] InvalidIdentifierTokens           = new char [ ]
        {
            ' ', '\x00a0', '.', ',', ';', '|', '~', '@', '#', '%', '^', '&', '*', '+', '-', '/',
            '\\', '<', '>', '?', '[', ']', '(', ')', '{', '}', '"', '\'', ':', '!'
        };

        public static MemberAttributes ValidateAccessModifiers ( this CodeDomProvider codeDomProvider, MemberAttributes accessModifiers )
        {
            if ( accessModifiers.HasBitMask ( MemberAttributes.Public | MemberAttributes.Static ) )
                if ( ! codeDomProvider.Supports ( GeneratorSupport.PublicStaticMembers ) )
                    accessModifiers &= ~MemberAttributes.Static;

            return accessModifiers;
        }

        public static string ValidateBaseName ( this CodeDomProvider codeDomProvider, string baseName )
        {
            var validatedBaseName = baseName;
            if ( !codeDomProvider.IsValidIdentifier ( validatedBaseName ) )
            {
                var validBaseName = codeDomProvider.ValidateIdentifier ( validatedBaseName );
                if ( validBaseName != null )
                    validatedBaseName = validBaseName;
            }

            if ( ! codeDomProvider.IsValidIdentifier ( validatedBaseName ) )
                throw new ArgumentException ( string.Format ( InvalidIdentifier, validatedBaseName ) );

            return validatedBaseName;
        }

        /// <summary>Generates a valid identifier based on the specified input string and code provider.</summary>
        /// <returns>A valid identifier with any invalid tokens replaced with the underscore (_) character, or null if the string still contains invalid characters according to the language specified by the provider parameter.</returns>
        /// <param name="provider">A <see cref="T:System.CodeDom.Compiler.CodeDomProvider"></see> object that specifies the target language to use.</param>
        /// <param name="name">The string to verify and, if necessary, convert to a valid resource name.</param>
        /// <exception cref="T:System.ArgumentNullException">key or provider is null.</exception>
        public static string ValidateIdentifier ( this CodeDomProvider provider, string name, bool isNameSpace = false )
        {
            for ( var index = 0; index < InvalidIdentifierTokens.Length; index++ )
            {
                var invalidToken = InvalidIdentifierTokens [ index ];
                if ( ! isNameSpace || invalidToken != '.' && invalidToken != ':' )
                    name = name.Replace ( invalidToken, InvalidIdentifierTokenReplacement );
            }

            if ( provider.IsValidIdentifier ( name ) )
                return name;

            name = provider.CreateValidIdentifier ( name );
            if ( provider.IsValidIdentifier ( name ) )
                return name;

            name = "_" + name;
            if ( provider.IsValidIdentifier ( name ) )
                return name;

            return null;
        }
    }
}