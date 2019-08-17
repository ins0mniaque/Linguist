using System;
using System.CodeDom;
using System.CodeDom.Compiler;

using Linguist.CodeDom;
using Linguist.Resources.Naming;

namespace Linguist.Generator
{
    public class ResourceTypeSettings
    {
        public string                ClassName       { get; set; }
        public string                Namespace       { get; set; }
        public MemberAttributes      AccessModifiers { get; set; } = MemberAttributes.Assembly | MemberAttributes.Static;
        public ResourceTypeOptions   Options         { get; set; } = ResourceTypeOptions.CultureChangedEvent;
        public ResourceTypeExtension Extension       { get; set; }
        public Type                  CustomToolType  { get; set; }

        public IResourceNamingStrategy ResourceNamingStrategy            { get; set; }
        public CodeExpression          ResourceNamingStrategyInitializer { get; set; }
        public CodeTypeReference       ResourceManagerType               { get; set; }
        public CodeExpression          ResourceManagerInitializer        { get; set; }
        public CodeTypeReference       LocalizerType                     { get; set; }
        public CodeExpression          LocalizerInitializer              { get; set; }

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
                Options         = Options,
                Extension       = Extension,
                CustomToolType  = CustomToolType,

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