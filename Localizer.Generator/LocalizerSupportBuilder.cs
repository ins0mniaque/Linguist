using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;

using Localizer.CodeDom;

namespace Localizer.Generator
{
    using static String;
    using static Comments;
    using static ErrorMessages;
    using static MemberNames;

    public class LocalizerSupportBuilder
    {
        public static CodeCompileUnit GenerateCode ( CodeDomProvider codeDomProvider, string inputFileName, string inputFileContent, string fileNamespace, string resourcesNamespace, MemberAttributes accessModifiers, Type customToolType, out CompilerError [ ] errors )
        {
            var baseName  = Path.GetFileNameWithoutExtension ( inputFileName );
            var extension = Path.GetExtension     ( inputFileName )
                                .ToLowerInvariant ( );

            var resources           = (IDictionary < string, Resource >) null;
            var resourceManagerType = (Type) null;

            switch ( extension )
            {
                case ".resx" :
                case ".resw" :
                    resourceManagerType = ResXParser.ResourceManagerType;
                    resources           = ResXParser.ExtractResources ( inputFileName, inputFileContent );
                    break;
                default :
                    throw new ArgumentException ( Format ( UnknownResourceFileFormat, Path.GetFileName ( inputFileName ) ), nameof ( inputFileName ) );
            }

            var builder = new LocalizerSupportBuilder ( codeDomProvider, baseName, resourceManagerType, resources, fileNamespace, resourcesNamespace, accessModifiers, customToolType );
            var code    = builder.Build ( );

            errors = resources.Values.Where ( resource => ! IsNullOrEmpty ( resource.ErrorText ) ).ToArray ( );
            if ( errors.Length == 0 )
                errors = null;

            return code;
        }

        protected LocalizerSupportBuilder ( CodeDomProvider codeDomProvider, string baseName, Type resourceManagerType, IDictionary < string, Resource > resources, string @namespace, string resourcesNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            CodeDomProvider     = codeDomProvider     ?? throw new ArgumentNullException ( nameof ( codeDomProvider     ) );
            BaseName            = baseName            ?? throw new ArgumentNullException ( nameof ( baseName            ) );
            ResourceManagerType = resourceManagerType ?? throw new ArgumentNullException ( nameof ( resourceManagerType ) );
            Namespace           = IsNullOrEmpty ( @namespace         ) ? null : codeDomProvider.ValidateIdentifier ( @namespace, true );
            ResourcesNamespace  = IsNullOrEmpty ( resourcesNamespace ) ? null : resourcesNamespace;
            ResourcesBaseName   = ResourcesNamespace != null ? ResourcesNamespace + '.' + baseName :
                                  Namespace          != null ? Namespace          + '.' + baseName :
                                                               baseName;
            ClassName           = codeDomProvider.ValidateBaseName        ( baseName );
            AccessModifiers     = codeDomProvider.ValidateAccessModifiers ( accessModifiers );
            TypeAttributes      = AccessModifiers.HasFlag ( MemberAttributes.Public ) ? TypeAttributes.Public :
                                                                                        TypeAttributes.AutoLayout;
            CustomToolType      = customToolType;

            Resources = new ReadOnlyDictionary < string, Resource > ( resources ?? throw new ArgumentNullException ( nameof ( resources ) ) );
        }

        protected CodeDomProvider   CodeDomProvider     { get; }
        protected string            BaseName            { get; }
        protected Type              ResourceManagerType { get; }
        protected string            Namespace           { get; }
        protected string            ResourcesNamespace  { get; }
        protected string            ResourcesBaseName   { get; }
        protected string            ClassName           { get; }
        protected string            ClassFullName       { get; }
        protected MemberAttributes  AccessModifiers     { get; }
        protected TypeAttributes    TypeAttributes      { get; }
        protected Type              CustomToolType      { get; }

        protected IReadOnlyDictionary < string, Resource > Resources { get; }

        public virtual CodeCompileUnit Build ( )
        {
            var validResources  = ValidateResourceNames    ( out var propertyNames );
            var codeCompileUnit = ConfigureCodeCompileUnit ( new CodeCompileUnit ( ) );
            var codeNamespace   = Code.CreateNamespace ( Namespace ?? ResourcesNamespace, "System" )
                                      .AddTo ( codeCompileUnit.Namespaces );

            var support = GenerateClass ( ).AddTo ( codeNamespace.Types );
            foreach ( var member in GenerateClassMembers ( ) )
                support.Members.Add ( member );

            var stringResources = new SortedList < string, StringResource > ( validResources.Count, StringComparer.InvariantCultureIgnoreCase );
            var resourceNames   = GenerateResourceNames ( out var getResourceName )?.AddTo ( support.Members );

            foreach ( var entry in validResources )
            {
                var propertyName     = entry.Key;
                var resource         = entry.Value;
                var resourceName     = GetResourceName  ( propertyNames, propertyName );
                var resourceProperty = GenerateProperty ( propertyName, resourceName, resource, getResourceName );

                if ( resourceProperty?.AddTo ( support.Members ) != null )
                {
                    if ( resourceNames != null )
                        GenerateResourceNameField ( resourceNames, propertyName, resourceName );

                    if ( resource is StringResource stringResource )
                        stringResources.Add ( propertyName, stringResource );
                }
                else
                    resource.ErrorText = Format ( CannotCreateResourceProperty, resourceName );
            }

            foreach ( var entry in stringResources )
            {
                var propertyName = entry.Key;
                var resource     = entry.Value;

                try
                {
                    var numberOfArguments = FormatParser.Parse ( resource.Value );
                    if ( numberOfArguments <= 0 )
                        continue;

                    var methodName       = propertyName + FormatMethodSuffix;
                    var uniqueMethodName = ! support.Members.Contains ( methodName );
                    if ( uniqueMethodName && CodeDomProvider.IsValidIdentifier ( methodName ) )
                    {
                        support.Members.Add ( GenerateFormatMethod ( methodName, propertyName, resource, numberOfArguments ) );
                        continue;
                    }

                    resource.ErrorText = Format ( CannotCreateFormatMethod, methodName, GetResourceName ( propertyNames, propertyName ) );
                }
                catch ( FormatException exception )
                {
                    resource.ErrorText = Format ( ErrorInStringResourceFormat, exception.Message, GetResourceName ( propertyNames, propertyName ) );
                }
            }

            CodeGenerator.ValidateIdentifiers ( codeCompileUnit );

            return codeCompileUnit;
        }

        protected virtual CodeCompileUnit ConfigureCodeCompileUnit ( CodeCompileUnit codeCompileUnit )
        {
            codeCompileUnit.ReferencedAssemblies.Add ( "System.dll" );
            codeCompileUnit.UserData.Add ( "AllowLateBound", false );
            codeCompileUnit.UserData.Add ( "RequireVariableDeclaration", true );

            return codeCompileUnit;
        }

        protected virtual IEnumerable < string > GetClassMemberNames ( )
        {
            yield return ResourceManagerPropertyName;
            yield return CultureInfoFieldName;
            yield return CultureInfoPropertyName;

            if ( CodeDomProvider.Supports ( GeneratorSupport.NestedTypes ) )
            {
                yield return ResourceNamesClassName;
                yield return ResourceManagerFieldName;
            }
        }

        protected virtual CodeTypeDeclaration GenerateClass ( )
        {
            var generator = typeof ( LocalizerSupportBuilder );
            var version   = generator.Assembly.GetName ( ).Version;
            var type      = new CodeTypeDeclaration ( ClassName )
            {
                TypeAttributes = TypeAttributes,
                Attributes     = AccessModifiers,
                IsPartial      = CodeDomProvider.Supports ( GeneratorSupport.PartialTypes )
            };

            type.AddSummary ( ClassSummary );

            if ( CustomToolType != null ) type.AddRemarks ( ClassRemarksFormat,         generator.Name, CustomToolType.Name );
            else                          type.AddRemarks ( ClassRemarksToollessFormat, generator.Name );

            return type.Attributed ( Code.Attribute < GeneratedCodeAttribute       > ( generator.FullName, version.ToString ( ) ),
                                     Code.Attribute < DebuggerNonUserCodeAttribute > ( ),
                                     Code.Attribute < ObfuscationAttribute         > ( ( "Exclude", true ), ( "ApplyToMembers", true ) ),
                                     Code.Attribute < SuppressMessageAttribute     > ( "Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces" ) );
        }

        protected virtual IEnumerable < CodeTypeMember > GenerateClassMembers ( )
        {
            var editorBrowsable = Code.Attribute < EditorBrowsableAttribute > ( Code.Type < EditorBrowsableState > ( )
                                                                                    .Field ( nameof ( EditorBrowsableState.Advanced ) ) );

            yield return GenerateConstructor ( );

            var lazy = GenerateLazyResourceManager ( out var lazyValue );
            foreach ( var member in lazy )
                yield return member;

            yield return GenerateResourceManagerProperty ( lazyValue ).Attributed ( editorBrowsable );

            yield return GenerateCultureProperty ( out var cultureField ).Attributed ( editorBrowsable );
            if ( cultureField != null )
                yield return cultureField;
        }

        protected virtual CodeConstructor GenerateConstructor ( )
        {
            var ctor = new CodeConstructor ( )
            {
                Attributes = AccessModifiers.HasFlag ( MemberAttributes.Static ) ? MemberAttributes.Private :
                                                                                   AccessModifiers & ~MemberAttributes.Static
            };

            return ctor.AddSummary ( ConstructorSummaryFormat, ClassName )
                       .Attributed ( Code.Attribute < SuppressMessageAttribute > ( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ) );
        }

        protected virtual CodeMemberProperty GenerateCultureProperty ( out CodeMemberField cultureField )
        {
            cultureField = Code.CreateField < CultureInfo > ( CultureInfoFieldName, MemberAttributes.Private | AccessModifiers & MemberAttributes.Static );

            var field = Code.Access ( AccessModifiers ).Field ( CultureInfoFieldName );

            return Code.CreateProperty < CultureInfo > ( CultureInfoPropertyName, AccessModifiers )
                       .Get ( get          => get.Return ( field ) )
                       .Set ( (set, value) => set.Add    ( field.Assign ( value ) ) )
                       .AddSummary ( CultureInfoPropertySummary );
        }

        protected virtual CodeTypeMember [ ] GenerateLazyResourceManager ( out CodeExpression lazyValue )
        {
            var cctor = (CodeTypeMember) new CodeTypeConstructor ( ).AddComment ( SingletonBeforeFieldInitComment );
            var init  = Code.TypeRef   ( ResourceManagerType )
                            .Construct ( Code.Constant ( ResourcesBaseName ),
                                         Code.TypeRef  ( ClassName, default )
                                             .TypeOf   ( )
                                             .Property ( nameof ( Type.Assembly ) ) );

            if ( CodeDomProvider.Supports ( GeneratorSupport.NestedTypes ) )
            {
                var lazyResourceManager = new CodeTypeDeclaration ( ResourceManagerFieldName ) { TypeAttributes = TypeAttributes.NestedPrivate };

                cctor.AddTo ( lazyResourceManager.Members );

                Code.CreateField ( ResourceManagerType, LazyResourceManagerFieldName, MemberAttributes.Assembly | MemberAttributes.Static )
                    .Initialize  ( init )
                    .AddTo       ( lazyResourceManager.Members );

                lazyValue = Code.Type  ( ResourceManagerFieldName, default )
                                .Field ( LazyResourceManagerFieldName );

                return new [ ] { lazyResourceManager };
            }

            lazyValue = Code.Static ( ).Field ( ResourceManagerFieldName );

            return new CodeTypeMember [ ] { cctor,
                                            Code.CreateField ( ResourceManagerType, ResourceManagerFieldName, MemberAttributes.Private | MemberAttributes.Static )
                                                .Initialize  ( init ) };
        }

        protected virtual CodeMemberProperty GenerateResourceManagerProperty ( CodeExpression lazyValue )
        {
            return Code.CreateProperty ( ResourceManagerType, ResourceManagerPropertyName, AccessModifiers, false )
                       .Get            ( get => get.Return ( lazyValue ) )
                       .AddSummary     ( ResourceManagerPropertySummary );
        }

        protected delegate CodeExpression GenerateResourceNameExpression ( string resourceName, string propertyName );

        protected virtual CodeTypeDeclaration GenerateResourceNames ( out GenerateResourceNameExpression getResourceName )
        {
            if ( ! CodeDomProvider.Supports ( GeneratorSupport.NestedTypes ) )
            {
                getResourceName = (resourceName, _) => new CodePrimitiveExpression ( resourceName );
                return null;
            }

            var resourceNames     = new CodeTypeDeclaration ( ResourceNamesClassName ) { TypeAttributes = TypeAttributes };
            var resourceNamesType = Code.Type ( ResourceNamesClassName, default );

            getResourceName = (_, propertyName) => resourceNamesType.Field ( propertyName );

            return resourceNames.AddSummary ( ResourceNamesClassSummary );
        }

        protected virtual void GenerateResourceNameField ( CodeTypeDeclaration resourceNames, string propertyName, string resourceName )
        {
            Code.CreateField < string > ( propertyName, MemberAttributes.Const | AccessModifiers & ~MemberAttributes.Static )
                .Initialize ( Code.Constant ( resourceName ) )
                .AddSummary ( ResourceNamesFieldSummaryFormat, resourceName )
                .AddTo      ( resourceNames.Members );
        }

        protected virtual CodeMemberProperty GenerateProperty ( string propertyName, string resourceName, Resource resource, GenerateResourceNameExpression getResourceName )
        {
            if ( IsNullOrEmpty ( resource.Type ) )
                return null;

            var resourceType    = Code.TypeRef ( resource.Type, default );
            var resourceManager = Code.Static ( ).Property ( ResourceManagerPropertyName );
            var culture         = Code.Access ( AccessModifiers ).Field ( CultureInfoFieldName );
            var getResource     = (CodeExpression) resourceManager.Method ( resource.Method )
                                                                  .Invoke ( getResourceName ( resourceName, propertyName ),
                                                                            culture );

            if ( resource.CastToType )
                getResource = getResource.Cast ( resourceType );

            var summary = resource is StringResource stringResource ?
                          Format ( StringPropertySummary,    GeneratePreview ( stringResource.Value ) ) :
                          Format ( NonStringPropertySummary, resourceName );

            return Code.CreateProperty ( Code.TypeRef ( resource.Type, default ), propertyName, AccessModifiers, false )
                       .Get            ( get => get.Return ( getResource ) )
                       .AddSummary     ( summary + FormatResourceComment ( resource.Comment ) );
        }

        protected virtual CodeMemberMethod GenerateFormatMethod ( string methodName, string propertyName, StringResource resource, int numberOfArguments )
        {
            if ( numberOfArguments <= 0 )
                throw new ArgumentOutOfRangeException ( nameof ( numberOfArguments ), numberOfArguments, "Number of argument must be greater than zero" );

            var objectType   = Code.TypeRef < object > ( );
            var summary      = Format ( FormatMethodSummary, GeneratePreview ( resource.Value ) );
            var formatMethod = Code.CreateMethod ( typeof ( string ), methodName, AccessModifiers )
                                   .AddSummary   ( summary + FormatResourceComment ( resource.Comment ) );

            if ( numberOfArguments > 3 )
                formatMethod.Attributed ( Code.Attribute < SuppressMessageAttribute > ( "Microsoft.Design", "CA1025:ReplaceRepetitiveArgumentsWithParamsArray" ) );

            var parameters = new CodeExpression [ 2 + numberOfArguments ];

            parameters [ 0 ] = Code.Access ( AccessModifiers ).Field    ( CultureInfoFieldName );
            parameters [ 1 ] = Code.Access ( AccessModifiers ).Property ( propertyName );

            for ( var index = 0; index < numberOfArguments; index++ )
            {
                var parameterName = "arg" + index.ToString ( CultureInfo.InvariantCulture );

                objectType.Parameter ( parameterName )
                          .AddTo     ( formatMethod.Parameters );

                parameters [ 2 + index ] = Code.Variable ( parameterName );

                if ( numberOfArguments > 1 )
                    formatMethod.AddParameterComment ( parameterName, FormatMultiParameterComment, Ordinals [ Math.Min ( index, Ordinals.Length - 1 ) ] );
                else
                    formatMethod.AddParameterComment ( parameterName, FormatParameterComment, index );
            }

            formatMethod.AddReturnComment  ( FormatReturnComment )
                        .Statements.Return ( Code.Type < string > ( )
                                                 .Method ( nameof ( string.Format ) )
                                                 .Invoke ( parameters ) );

            return formatMethod;
        }

        protected string GeneratePreview ( string resourceValue )
        {
            if ( resourceValue.Length > PreviewMaximumLength )
                resourceValue = Format ( TruncatedPreview, resourceValue.Substring ( 0, PreviewMaximumLength ) );

            return SecurityElement.Escape ( resourceValue );
        }

        protected string FormatResourceComment ( string comment )
        {
            if ( ! IsNullOrWhiteSpace ( comment ) )
                return "\n\n" + comment.Trim ( );

            return null;
        }

        private static string GetResourceName ( IDictionary < string, string > propertyNames, string propertyName )
        {
            if ( ! propertyNames.TryGetValue ( propertyName, out var resourceName ) )
                resourceName = propertyName;

            return resourceName;
        }

        protected virtual SortedList < string, Resource > ValidateResourceNames ( out Dictionary < string, string > propertyNames )
        {
            var classProperties = new HashSet < string > ( GetClassMemberNames ( ) );
            var validResources  = new SortedList < string, Resource > ( Resources.Count, StringComparer.InvariantCultureIgnoreCase );
                propertyNames   = new Dictionary < string, string >   ( 0,               StringComparer.InvariantCultureIgnoreCase );

            foreach ( var entry in Resources )
            {
                var resourceName = entry.Key;
                var resource     = entry.Value;

                if ( classProperties.Contains ( resourceName ) )
                {
                    resource.ErrorText = Format ( PropertyAlreadyExists, resourceName );
                    continue;
                }

                if ( typeof ( void ).FullName == resource.Type )
                {
                    resource.ErrorText = Format ( InvalidPropertyType, resource.Type, resourceName );
                    continue;
                }

                var isWinFormsLocalizableResource = resourceName.Length > 0 && resourceName [ 0 ] == '$' ||
                                                    resourceName.Length > 1 && resourceName [ 0 ] == '>' &&
                                                                               resourceName [ 1 ] == '>';
                if ( isWinFormsLocalizableResource )
                {
                    resource.IsWarning = true;
                    resource.ErrorText = Format ( SkippingWinFormsResource, resourceName );
                    continue;
                }

                if ( ! CodeDomProvider.IsValidIdentifier ( resourceName ) )
                {
                    var propertyName = CodeDomProvider.ValidateIdentifier ( resourceName );
                    if ( propertyName == null )
                    {
                        resource.ErrorText = Format ( CannotCreateResourceProperty, resourceName );
                        continue;
                    }

                    if ( propertyNames.TryGetValue ( propertyName, out var duplicateResourceName ) )
                    {
                        var duplicateResource = Resources [ duplicateResourceName ];
                        if ( IsNullOrEmpty ( duplicateResource.ErrorText ) )
                            duplicateResource.ErrorText = Format ( CannotCreateResourceProperty, duplicateResourceName );

                        if ( validResources.ContainsKey ( propertyName ) )
                            validResources.Remove ( propertyName );

                        resource.ErrorText = Format ( CannotCreateResourceProperty, resourceName );

                        continue;
                    }

                    propertyNames [ propertyName ] = resourceName;
                    resourceName = propertyName;
                }

                if ( validResources.ContainsKey ( resourceName ) )
                {
                    if ( propertyNames.TryGetValue ( resourceName, out var duplicateResourceName ) )
                    {
                        var duplicateResource = Resources [ duplicateResourceName ];
                        if ( IsNullOrEmpty ( duplicateResource.ErrorText ) )
                            duplicateResource.ErrorText = Format ( CannotCreateResourceProperty, duplicateResourceName );

                        propertyNames.Remove ( resourceName );
                    }

                    resource.ErrorText = Format ( CannotCreateResourceProperty, entry.Key );

                    validResources.Remove ( resourceName );

                    continue;
                }

                validResources.Add ( resourceName, resource );
            }

            return validResources;
        }
    }
}