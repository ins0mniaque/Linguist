using System;
using System.CodeDom;
using System.CodeDom.Compiler;
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

    public class ResourceTypeSettings
    {
        public string           ClassName       { get; set; }
        public string           Namespace       { get; set; }
        public MemberAttributes AccessModifiers { get; set; } = MemberAttributes.Assembly | MemberAttributes.Static;
        public Type             CustomToolType  { get; set; }

        public bool GenerateWPFSupport          { get; set; }
        public bool GenerateXamarinFormsSupport { get; set; }

        public IResourceNamingStrategy ResourceNamingStrategy            { get; set; } = Resources.Naming.ResourceNamingStrategy.Default;
        public CodeExpression          ResourceNamingStrategyInitializer { get; set; }
        public CodeTypeReference       ResourceManagerType               { get; set; }
        public CodeExpression          ResourceManagerInitializer        { get; set; }
        public CodeTypeReference       LocalizerType                     { get; set; }
        public CodeExpression          LocalizerInitializer              { get; set; }

        public void ConfigureWithoutLocalizer ( string manifestResourceBaseName, string resourceSetType = null )
        {
            ResourceManagerType        = Code.Type < ResourceManager > ( );
            ResourceManagerInitializer = ConstructResourceManager ( manifestResourceBaseName, resourceSetType );
        }

        public void ConfigureResourceManager ( string manifestResourceBaseName, string resourceSetType = null )
        {
            resourceSetType = resourceSetType ?? typeof ( BinaryResourceSet ).FullName;

            ResourceManagerType        = Code.Type < ResourceManager > ( );
            ResourceManagerInitializer = ConstructResourceManager ( manifestResourceBaseName, resourceSetType );

            LocalizerType        = Code.Type < ResourceManagerLocalizer > ( );
            LocalizerInitializer = ConstructLocalizer ( ResourceNamingStrategyInitializer );
        }

        public void ConfigureFileBasedResourceManager ( string baseName, string path, string resourceSetType = null )
        {
            resourceSetType = resourceSetType ?? AutoDetect.ResourceSetType ( path ).FullName;

            var pathFormat = AutoDetect.PathFormat ( path, out var neutralCultureName );

            ResourceManagerType        = Code.Type < FileBasedResourceManager > ( );
            ResourceManagerInitializer = ResourceManagerType.Construct ( Code.Constant ( baseName ),
                                                                         Code.Constant ( pathFormat ),
                                                                         Code.Constant ( neutralCultureName ),
                                                                         Code.TypeOf   ( Code.Type ( resourceSetType ) ) );

            LocalizerType        = Code.Type < ResourceManagerLocalizer > ( );
            LocalizerInitializer = ConstructLocalizer ( ResourceNamingStrategyInitializer );
        }

        private CodeObjectCreateExpression ConstructResourceManager ( string manifestResourceBaseName, string resourceSetType = null )
        {
            var resourceManager = Code.Type < ResourceManager > ( )
                                      .Construct ( Code.Constant ( manifestResourceBaseName ),
                                                   Code.Type     ( ClassName ).Local ( )
                                                       .TypeOf   ( )
                                                       .Property ( nameof ( Type.Assembly ) ) );

            if ( ! IsNullOrEmpty ( resourceSetType ) )
                resourceManager.Parameters.Add ( Code.TypeOf ( Code.Type ( resourceSetType ) ) );

            return resourceManager;
        }

        private static CodeExpression ConstructLocalizer ( CodeExpression resourceNamingStrategy )
        {
            var resourceManager = Code.Static ( ).Property ( ResourceManagerPropertyName );
            var initializer     = Code.Type < ResourceManagerLocalizer > ( )
                                      .Construct ( resourceManager );

            if ( resourceNamingStrategy != null )
                initializer.Parameters.Add ( Code.Type < ResourceManagerPluralizer > ( )
                                                 .Construct ( resourceManager, resourceNamingStrategy ) );

            return initializer;
        }

        public ResourceTypeSettings Validate ( CodeDomProvider codeDomProvider )
        {
            var settings = Clone ( );

            settings.ClassName       = codeDomProvider.ValidateIdentifier      ( settings.ClassName );
            settings.Namespace       = codeDomProvider.ValidateIdentifier      ( settings.Namespace, true );
            settings.AccessModifiers = codeDomProvider.ValidateAccessModifiers ( settings.AccessModifiers );

            return settings;
        }

        public ResourceTypeSettings Clone ( )
        {
            return new ResourceTypeSettings ( )
            {
                ClassName       = ClassName,
                Namespace       = Namespace,
                AccessModifiers = AccessModifiers,
                CustomToolType  = CustomToolType,

                GenerateWPFSupport          = GenerateWPFSupport,
                GenerateXamarinFormsSupport = GenerateXamarinFormsSupport,

                ResourceNamingStrategy            = ResourceNamingStrategy,
                ResourceNamingStrategyInitializer = ResourceNamingStrategyInitializer,
                ResourceManagerType               = ResourceManagerType,
                ResourceManagerInitializer        = ResourceManagerInitializer,
                LocalizerType                     = LocalizerType,
                LocalizerInitializer              = LocalizerInitializer
            };
        }
    }
}