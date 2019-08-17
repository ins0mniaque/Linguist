using System;
using System.CodeDom;
using System.Resources;

using Linguist.CodeDom.Fluent;
using Linguist.Resources;
using Linguist.Resources.Naming;
using Linguist.Resources.Binary;

namespace Linguist.Generator
{
    using static String;
    using static MemberNames;

    public static class ResourceTypeConfigurator
    {
        public static void ConfigureWithoutLocalizer ( this ResourceTypeSettings settings, string manifestResourceBaseName, string resourceSetType = null )
        {
            settings.ResourceManagerType        = Code.Type < ResourceManager > ( );
            settings.ResourceManagerInitializer = ConstructResourceManager ( settings.ClassName, manifestResourceBaseName, resourceSetType );
        }

        public static void ConfigureResourceManager ( this ResourceTypeSettings settings, string manifestResourceBaseName, string resourceSetType = null )
        {
            resourceSetType = resourceSetType ?? typeof ( BinaryResourceSet ).FullName;

            settings.ResourceNamingStrategy = ResourceNamingStrategy.Default;

            settings.ResourceManagerType        = Code.Type < ResourceManager > ( );
            settings.ResourceManagerInitializer = ConstructResourceManager ( settings.ClassName, manifestResourceBaseName, resourceSetType );

            settings.LocalizerType        = Code.Type < ResourceManagerLocalizer > ( );
            settings.LocalizerInitializer = ConstructLocalizer ( settings.ResourceNamingStrategyInitializer );
        }

        public static void ConfigureFileBasedResourceManager ( this ResourceTypeSettings settings, string baseName, string path, string resourceSetType = null )
        {
            resourceSetType = resourceSetType ?? AutoDetect.ResourceSetType ( path ).FullName;

            settings.ResourceNamingStrategy = ResourceNamingStrategy.Default;

            var pathFormat      = AutoDetect.PathFormat ( path, out var neutralCultureName );
            var resourceManager = Code.Type < FileBasedResourceManager > ( );

            settings.ResourceManagerType        = resourceManager;
            settings.ResourceManagerInitializer = resourceManager.Construct ( Code.Constant ( baseName ),
                                                                              Code.Constant ( pathFormat ),
                                                                              Code.Constant ( neutralCultureName ),
                                                                              Code.TypeOf   ( Code.Type ( resourceSetType ) ) );

            settings.LocalizerType        = Code.Type < ResourceManagerLocalizer > ( );
            settings.LocalizerInitializer = ConstructLocalizer ( settings.ResourceNamingStrategyInitializer );
        }

        private static CodeObjectCreateExpression ConstructResourceManager ( string className, string manifestResourceBaseName, string resourceSetType = null )
        {
            var resourceManager = Code.Type < ResourceManager > ( )
                                      .Construct ( Code.Constant ( manifestResourceBaseName ),
                                                   Code.Type     ( className ).Local ( )
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
    }
}