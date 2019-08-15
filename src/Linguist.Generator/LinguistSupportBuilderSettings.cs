using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Resources;

using Linguist.CodeDom;
using Linguist.Resources;
using Linguist.Resources.Naming;
using Linguist.Resources.Binary;

namespace Linguist.Generator
{
    using static String;

    public class LinguistSupportBuilderSettings
    {
        public string           BaseName           { get; set; }
        public string           Path               { get; set; }
        public string           Namespace          { get; set; }
        public string           ResourcesNamespace { get; set; }
        public string           ResourceSetType    { get; set; }
        public MemberAttributes AccessModifiers    { get; set; }
        public Type             CustomToolType     { get; set; }

        public IResourceNamingStrategy ResourceNamingStrategy            { get; set; } = Resources.Naming.ResourceNamingStrategy.Default;
        public CodeExpression          ResourceNamingStrategyInitializer { get; set; }

        public bool GenerateLocalizerSupport    { get; set; }
        public bool GenerateWPFSupport          { get; set; }
        public bool GenerateXamarinFormsSupport { get; set; }

        internal string         ClassName         { get; private set; }
        internal string         ResourcesBaseName { get; private set; }
        internal TypeAttributes TypeAttributes    { get; private set; }

        internal CodeTypeReference  ResourceManagerType        { get; private set; }
        internal CodeExpression     ResourceManagerInitializer { get; private set; }
        internal CodeTypeReference  LocalizerType              { get; private set; }
        internal CodeExpression     LocalizerInitializer       { get; private set; }

        // TODO: Copy settings...
        internal LinguistSupportBuilderSettings Setup ( CodeDomProvider codeDomProvider )
        {
            codeDomProvider = codeDomProvider ?? throw new ArgumentNullException ( nameof ( codeDomProvider ) );

            BaseName           = BaseName        ?? throw new ArgumentNullException ( nameof ( BaseName        ) );
            Namespace          = IsNullOrEmpty ( Namespace         ) ? null : codeDomProvider.ValidateIdentifier ( Namespace, true );
            ResourcesNamespace = IsNullOrEmpty ( ResourcesNamespace ) ? null : ResourcesNamespace;
            ResourcesBaseName  = ResourcesNamespace != null ? ResourcesNamespace + '.' + BaseName :
                                 Namespace          != null ? Namespace          + '.' + BaseName :
                                                              BaseName;
            ClassName          = codeDomProvider.ValidateBaseName        ( BaseName );
            AccessModifiers    = codeDomProvider.ValidateAccessModifiers ( AccessModifiers );
            TypeAttributes     = AccessModifiers.HasBitMask ( MemberAttributes.Public ) ? TypeAttributes.Public :
                                                                                          TypeAttributes.AutoLayout;

            GenerateLocalizerSupport = GenerateLocalizerSupport | GenerateWPFSupport | GenerateXamarinFormsSupport;

            if ( IsNullOrEmpty ( Path ) )
            {
                var binaryResourceSetType = GenerateLocalizerSupport ? typeof ( BinaryResourceSet ).FullName : null;

                ResourceManagerType        = Code.TypeRef < ResourceManager > ( );
                ResourceManagerInitializer = DefaultManagerInitializer ( ResourcesBaseName, ClassName, binaryResourceSetType );
            }
            else
            {
                var format = AutoDetect.PathFormat ( Path, out var neutralCultureName );

                ResourceManagerType        = Code.TypeRef < FileBasedResourceManager > ( );
                ResourceManagerInitializer = FileBasedManagerInitializer ( ResourcesBaseName, format, neutralCultureName, ResourceSetType );
            }

            if ( GenerateLocalizerSupport )
            {
                LocalizerType        = Code.TypeRef < ResourceManagerLocalizer > ( );
                LocalizerInitializer = DefaultLocalizerInitializer ( ResourceManagerInitializer, ResourceNamingStrategyInitializer );
            }

            return this;
        }

        private static CodeExpression DefaultManagerInitializer ( string baseName, string className, string resourceSetType )
        {
            var initializer = Code.TypeRef < ResourceManager > ( )
                                  .Construct ( Code.Constant ( baseName ),
                                               Code.TypeRef  ( className, default )
                                                   .TypeOf   ( )
                                                   .Property ( nameof ( Type.Assembly ) ) );

            if ( ! IsNullOrEmpty ( resourceSetType ) )
                initializer.Parameters.Add ( Code.TypeOf ( Code.TypeRef ( resourceSetType ) ) );

            return initializer;
        }

        private static CodeExpression FileBasedManagerInitializer ( string baseName, string pathFormat, string neutralCultureName, string resourceSetType )
        {
            var initializer = Code.TypeRef < FileBasedResourceManager > ( )
                                  .Construct ( Code.Constant ( baseName ),
                                               Code.Constant ( pathFormat ),
                                               Code.Constant ( neutralCultureName ) );

            if ( ! IsNullOrEmpty ( resourceSetType ) )
                initializer.Parameters.Add ( Code.TypeOf ( Code.TypeRef ( resourceSetType ) ) );

            return initializer;
        }

        private static CodeExpression DefaultLocalizerInitializer ( CodeExpression resourceManager, CodeExpression resourceNamingStrategy )
        {
            var initializer = Code.TypeRef < ResourceManagerLocalizer > ( )
                                  .Construct ( resourceManager );

            if ( resourceNamingStrategy != null )
                initializer.Parameters.Add ( Code.TypeRef < ResourceManagerPluralizer > ( )
                                                 .Construct ( resourceManager, resourceNamingStrategy ) );

            return initializer;
        }
    }
}