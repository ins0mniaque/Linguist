using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Localizer.CodeDom.Extensions
{
    /// <summary>
    /// Adds support for C# static classes and events by proxying the code generator
    /// text writer and inserting the static access modifier as the code is being written.
    /// Also automatically removes all class constructors from static types that
    /// have been added to support languages without static classes.
    /// </summary>
    public static class CSharpStaticClassSupport
    {
        /// <summary>
        /// Adds support for C# static classes and events and removes constructors from static classes.
        /// </summary>
        public static void AddCSharpStaticClassSupport ( this CodeDomProvider codeDomProvider, CodeCompileUnit codeCompileUnit, ref TextWriter textWriter )
        {
            var staticClasses = codeCompileUnit.Namespaces
                                               .OfType < CodeNamespace > ( )
                                               .SelectMany ( ns   => ns.Types.OfType < CodeTypeDeclaration > ( ) )
                                               .SelectMany ( type => type.WithNestedTypes ( ) )
                                               .SelectMany ( type => GetStaticDefinitionsPaths ( codeDomProvider, type ) )
                                               .Where      ( path => path != null )
                                               .ToArray    ( );

            if ( staticClasses.Length > 0 )
                textWriter = new Proxy ( staticClasses, textWriter );
        }

        /// <summary>
        /// Adds support for C# static classes and events and removes constructors from static classes.
        /// </summary>
        public static void AddCSharpStaticClassSupport ( this CodeDomProvider codeDomProvider, CodeTypeDeclaration codeType, ref TextWriter textWriter )
        {
            var staticDefinitions = codeType.WithNestedTypes ( )
                                            .SelectMany ( type => GetStaticDefinitionsPaths ( codeDomProvider, type ) )
                                            .ToArray    ( );

            if ( staticDefinitions.Length > 0 )
                textWriter = new Proxy ( staticDefinitions, textWriter );
        }

        private class Proxy : TextRewriterProxy
        {
            public Proxy ( string [ ] [ ] staticDefinitions, TextWriter textWriter ) : base ( staticDefinitions, textWriter ) { }

            protected override void OnPathFound ( string [ ] path, IList < string > buffer )
            {
                buffer.Insert ( 1, "static " );
            }
        }

        private static string [ ] [ ] GetStaticDefinitionsPaths ( CodeDomProvider codeDomProvider, CodeTypeDeclaration type )
        {
            var paths = new List < string [ ] > ( );

            var staticClass = GetStaticClassDefinitionPath ( codeDomProvider, type );
            if ( staticClass != null )
                paths.Add ( staticClass );

            foreach ( var @event in type.Members.OfType < CodeMemberEvent > ( ) )
            {
                var staticEvent = GetStaticEventDefinitionPath ( codeDomProvider, @event );
                if ( staticEvent != null )
                    paths.Add ( staticEvent );
            }

            return paths.ToArray ( );
        }

        private static string [ ] GetStaticClassDefinitionPath ( CodeDomProvider codeDomProvider, CodeTypeDeclaration type )
        {
            var isStatic = type.Attributes.HasBitMask ( MemberAttributes.Static );
            var isClass  = type.IsClass && ( type.TypeAttributes & TypeAttributes.ClassSemanticsMask ) == TypeAttributes.Class;

            if ( ! isStatic || ! isClass )
                return null;

            RemoveConstructors ( type );

            var path = new List < string > ( );

            switch ( type.TypeAttributes & TypeAttributes.VisibilityMask )
            {
                case TypeAttributes.Public       :
                case TypeAttributes.NestedPublic :
                    path.Add ( "public " );
                    break;
                case TypeAttributes.NestedPrivate :
                    path.Add ( "private " );
                    break;
                case TypeAttributes.NestedFamily :
                    path.Add ( "protected " );
                    break;
                case TypeAttributes.NotPublic         :
                case TypeAttributes.NestedAssembly    :
                case TypeAttributes.NestedFamANDAssem :
                    path.Add ( "internal " );
                    break;
                case TypeAttributes.NestedFamORAssem :
                    path.Add ( "protected internal " );
                    break;
            }

            if ( type.TypeAttributes.HasBitMask ( TypeAttributes.Sealed ) )
                path.Add ( "sealed " );
            if ( type.TypeAttributes.HasBitMask ( TypeAttributes.Abstract ) )
                path.Add ( "abstract " );
            if ( type.IsPartial )
                path.Add ( "partial " );

            path.Add ( "class " );

            path.Add ( codeDomProvider.CreateEscapedIdentifier ( type.Name ) );

            return path.ToArray ( );
        }

        private static string [ ] GetStaticEventDefinitionPath ( CodeDomProvider codeDomProvider, CodeMemberEvent @event )
        {
            var isStatic = @event.Attributes.HasBitMask ( MemberAttributes.Static ) &&
                           @event.PrivateImplementationType == null;

            if ( ! isStatic )
                return null;

            var path = new List < string > ( );

            switch ( @event.Attributes & MemberAttributes.AccessMask )
            {
                case MemberAttributes.Assembly :
                    path.Add ( "internal " );
                    break;
                case MemberAttributes.FamilyAndAssembly :
                    path.Add ( "internal " );
                    break;
                case MemberAttributes.Family :
                    path.Add ( "protected " );
                    break;
                case MemberAttributes.FamilyOrAssembly :
                    path.Add ( "protected internal " );
                    break;
                case MemberAttributes.Private :
                    path.Add ( "private " );
                    break;
                case MemberAttributes.Public :
                    path.Add ( "public " );
                    break;
            }

            path.Add ( "event " );
            path.Add ( codeDomProvider.GetTypeOutput ( @event.Type ) );
            path.Add ( " " );
            path.Add ( codeDomProvider.CreateEscapedIdentifier ( @event.Name ) );

            return path.ToArray ( );
        }

        private static void RemoveConstructors ( CodeTypeDeclaration type )
        {
            for ( var index = type.Members.Count - 1; index >= 0; index-- )
                if ( type.Members [ index ] is CodeConstructor )
                    type.Members.RemoveAt ( index );
        }
    }
}