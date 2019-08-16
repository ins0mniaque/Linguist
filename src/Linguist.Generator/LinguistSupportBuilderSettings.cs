using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Resources;

using Linguist.CodeDom;
using Linguist.CodeDom.Fluent;
using Linguist.Resources;
using Linguist.Resources.Naming;
using Linguist.Resources.Binary;

namespace Linguist.Generator
{
    using static String;
    using static MemberNames;

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

        // TODO: Clone settings
        //       CodeDomProvider.Supports ( GeneratorSupport.* ) ) checks
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

                ResourceManagerType        = Code.Type < ResourceManager > ( );
                ResourceManagerInitializer = DefaultManagerInitializer ( ResourcesBaseName, ClassName, binaryResourceSetType );
            }
            else
            {
                var format = AutoDetect.PathFormat ( Path, out var neutralCultureName );

                ResourceManagerType        = Code.Type < FileBasedResourceManager > ( );
                ResourceManagerInitializer = FileBasedManagerInitializer ( ResourcesBaseName, format, neutralCultureName, ResourceSetType );
            }

            if ( GenerateLocalizerSupport )
            {
                LocalizerType        = Code.Type < ResourceManagerLocalizer > ( );
                LocalizerInitializer = DefaultLocalizerInitializer ( ResourceNamingStrategyInitializer );
            }

            return this;
        }

        private static CodeExpression DefaultManagerInitializer ( string baseName, string className, string resourceSetType )
        {
            var initializer = Code.Type < ResourceManager > ( )
                                  .Construct ( Code.Constant ( baseName ),
                                               Code.Type     ( className ).Local ( )
                                                   .TypeOf   ( )
                                                   .Property ( nameof ( Type.Assembly ) ) );

            if ( ! IsNullOrEmpty ( resourceSetType ) )
                initializer.Parameters.Add ( Code.TypeOf ( Code.Type ( resourceSetType ) ) );

            return initializer;
        }

        private static CodeExpression FileBasedManagerInitializer ( string baseName, string pathFormat, string neutralCultureName, string resourceSetType )
        {
            var initializer = Code.Type < FileBasedResourceManager > ( )
                                  .Construct ( Code.Constant ( baseName ),
                                               Code.Constant ( pathFormat ),
                                               Code.Constant ( neutralCultureName ) );

            if ( ! IsNullOrEmpty ( resourceSetType ) )
                initializer.Parameters.Add ( Code.TypeOf ( Code.Type ( resourceSetType ) ) );

            return initializer;
        }

        private static CodeExpression DefaultLocalizerInitializer ( CodeExpression resourceNamingStrategy )
        {
            return DefaultLocalizerInitializer ( Code.Static ( ).Property ( ResourceManagerPropertyName ), resourceNamingStrategy );
        }

        private static CodeExpression DefaultLocalizerInitializer ( CodeExpression resourceManager, CodeExpression resourceNamingStrategy )
        {
            var initializer = Code.Type < ResourceManagerLocalizer > ( )
                                  .Construct ( resourceManager );

            if ( resourceNamingStrategy != null )
                initializer.Parameters.Add ( Code.Type < ResourceManagerPluralizer > ( )
                                                 .Construct ( resourceManager, resourceNamingStrategy ) );

            return initializer;
        }
    }
}