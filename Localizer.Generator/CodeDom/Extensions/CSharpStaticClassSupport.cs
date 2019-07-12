using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Localizer.CodeDom.Extensions
{
    /// <summary>
    /// Adds support for C# static classes by proxying the code generator text writer
    /// and inserting the static access modifier as the code is being written.
    /// Also automatically removes all class constructors from static types that
    /// have been added to support languages without static classes.
    /// </summary>
    public static class CSharpStaticClassSupport
    {
        /// <summary>
        /// Adds support for C# static classes and removes constructors from static classes.
        /// </summary>
        public static void AddCSharpStaticClassSupport ( this CodeDomProvider codeDomProvider, CodeCompileUnit codeCompileUnit, ref TextWriter textWriter )
        {
            var staticClasses = codeCompileUnit.Namespaces
                                               .OfType < CodeNamespace > ( )
                                               .SelectMany ( ns   => ns.Types.OfType < CodeTypeDeclaration > ( ) )
                                               .SelectMany ( type => type.WithNestedTypes ( ) )
                                               .Select     ( type => GetStaticClassDefinitionPath ( codeDomProvider, type ) )
                                               .Where      ( path => path != null )
                                               .ToArray    ( );

            if ( staticClasses.Length > 0 )
                textWriter = new Proxy ( staticClasses, textWriter );
        }

        /// <summary>
        /// Adds support for C# static classes and removes constructors from static classes.
        /// </summary>
        public static void AddCSharpStaticClassSupport ( this CodeDomProvider codeDomProvider, CodeTypeDeclaration codeType, ref TextWriter textWriter )
        {
            var staticClasses = codeType.WithNestedTypes ( )
                                        .Select  ( type => GetStaticClassDefinitionPath ( codeDomProvider, type ) )
                                        .Where   ( path => path != null )
                                        .ToArray ( );

            if ( staticClasses.Length > 0 )
                textWriter = new Proxy ( staticClasses, textWriter );
        }

        private class Proxy : TextWriterProxy
        {
            private readonly List < string > buffer = new List < string > ( );
            private readonly string [ ] [ ]  paths;
            private readonly int    [ ]      indices;

            public Proxy ( string [ ] [ ] staticClasses, TextWriter textWriter ) : base ( textWriter )
            {
                paths   = staticClasses;
                indices = new int [ paths.Length ];

                WriteBufferAndReset ( );
            }

            private void WriteBufferAndReset ( )
            {
                foreach ( var entry in buffer )
                    writer.Write ( entry );

                buffer.Clear ( );

                for ( var index = 0; index < paths.Length; index++ )
                    if ( indices [ index ] != int.MaxValue )
                        indices [ index ] = -1;
            }

            protected override void BeforeWrite ( )
            {
                WriteBufferAndReset ( );
            }

            public override void Write ( string value )
            {
                var buffered = false;

                for ( var staticClass = 0; staticClass < paths.Length; staticClass++ )
                {
                    var path  = paths   [ staticClass ];
                    var index = indices [ staticClass ];

                    if ( index == int.MaxValue )
                        continue;

                    if ( value == path [ index + 1 ] )
                    {
                        indices [ staticClass ] = ++index;

                        if ( index < path.Length - 1 )
                        {
                            buffered = true;
                            continue;
                        }

                        indices [ staticClass ] = int.MaxValue;
                        buffer.Insert ( 1, "static " );

                        buffered = false;
                        break;
                    }
                }

                if ( buffered )
                {
                    buffer.Add ( value );
                    return;
                }

                WriteBufferAndReset ( );

                writer.Write ( value );
            }
        }

        private static string [ ] GetStaticClassDefinitionPath ( CodeDomProvider codeDomProvider, CodeTypeDeclaration type )
        {
            var isStatic = type.Attributes.HasFlag ( MemberAttributes.Static );
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

            if ( type.TypeAttributes.HasFlag ( TypeAttributes.Sealed ) )
                path.Add ( "sealed " );
            if ( type.TypeAttributes.HasFlag ( TypeAttributes.Abstract ) )
                path.Add ( "abstract " );
            if ( type.IsPartial )
                path.Add ( "partial " );

            path.Add ( "class " );

            path.Add ( codeDomProvider.CreateEscapedIdentifier ( type.Name ) );

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