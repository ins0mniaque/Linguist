using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;

namespace Linguist.CodeDom.Extensions
{
    /// <summary>
    /// Adds support for Visual Basic modules by marking static classes as Module and
    /// nested static classes as NotInheritable with an empty private constructor.
    /// Also automatically removes all class constructors from static classes that
    /// have been added to support languages without static classes.
    /// </summary>
    public static class VBModuleSupport
    {
        /// <summary>
        /// Adds support for Visual Basic modules.
        /// </summary>
        public static void AddVBModuleSupport ( this CodeDomProvider codeDomProvider, CodeCompileUnit codeCompileUnit )
        {
            var types = codeCompileUnit.Namespaces
                                       .OfType < CodeNamespace > ( )
                                       .SelectMany ( ns   => ns.Types.OfType < CodeTypeDeclaration > ( ) )
                                       .SelectMany ( type => type.WithNestedTypes ( ) );

            foreach ( var type in types )
                FixStaticClassDefinition ( type );
        }

        /// <summary>
        /// Adds support for Visual Basic modules.
        /// </summary>
        public static void AddVBModuleSupport ( this CodeDomProvider codeDomProvider, CodeTypeDeclaration codeType )
        {
            foreach ( var type in codeType.WithNestedTypes ( ) )
                FixStaticClassDefinition ( type );
        }

        private static void FixStaticClassDefinition ( CodeTypeDeclaration type )
        {
            var isStatic = type.Attributes.HasBitMask ( MemberAttributes.Static );
            var isClass  = type.IsClass && ( type.TypeAttributes & TypeAttributes.ClassSemanticsMask ) == TypeAttributes.Class;

            if ( ! isStatic || ! isClass )
                return;

            RemoveConstructors ( type );

            var isNested = type.TypeAttributes.HasBitMask ( TypeAttributes.NestedPublic      ) ||
                           type.TypeAttributes.HasBitMask ( TypeAttributes.NestedPrivate     ) ||
                           type.TypeAttributes.HasBitMask ( TypeAttributes.NestedFamily      ) ||
                           type.TypeAttributes.HasBitMask ( TypeAttributes.NestedAssembly    ) ||
                           type.TypeAttributes.HasBitMask ( TypeAttributes.NestedFamANDAssem ) ||
                           type.TypeAttributes.HasBitMask ( TypeAttributes.NestedFamORAssem  );

            if ( isNested )
            {
                type.TypeAttributes |= TypeAttributes.Sealed;
                type.Members.Add ( new CodeConstructor ( ) { Attributes = MemberAttributes.Private } );
            }
            else
                type.UserData.Add ( "Module", true );
        }

        private static void RemoveConstructors ( CodeTypeDeclaration type )
        {
            for ( var index = type.Members.Count - 1; index >= 0; index-- )
                if ( type.Members [ index ] is CodeConstructor )
                    type.Members.RemoveAt ( index );
        }
    }
}