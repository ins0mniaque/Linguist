using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;

using Localizer.CodeDom;
using Localizer.Resources;

namespace Localizer.Generator
{
    using static String;
    using static Comments;
    using static ErrorMessages;
    using static MemberNames;

    public class LocalizerSupportBuilder
    {
        public static LocalizerSupportBuilder GenerateBuilder ( CodeDomProvider codeDomProvider, string inputFileName, string inputFileContent, string fileNamespace, string resourcesNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            var baseName  = Path.GetFileNameWithoutExtension ( inputFileName );
            var extension = Path.GetExtension     ( inputFileName )
                                .ToLowerInvariant ( );

            var resourceSet = (ResourceSet) null;

            switch ( extension )
            {
                case ".resx" :
                case ".resw" :
                    resourceSet = ResXParser.ExtractResourceSet ( inputFileName, inputFileContent );
                    break;
                default :
                    throw new ArgumentException ( Format ( UnknownResourceFileFormat, Path.GetFileName ( inputFileName ) ), nameof ( inputFileName ) );
            }

            return new LocalizerSupportBuilder ( codeDomProvider, baseName, resourceSet, fileNamespace, resourcesNamespace, accessModifiers, customToolType );
        }

        public static CodeCompileUnit GenerateCode ( CodeDomProvider codeDomProvider, string inputFileName, string inputFileContent, string fileNamespace, string resourcesNamespace, MemberAttributes accessModifiers, Type customToolType, out CompilerError [ ] errors )
        {
            var builder = GenerateBuilder ( codeDomProvider, inputFileName, inputFileContent, fileNamespace, resourcesNamespace, accessModifiers, customToolType );
            var code    = builder.Build ( );

            errors = builder.GetErrors ( );

            return code;
        }

        public static string GenerateSource ( CodeDomProvider codeDomProvider, string inputFileName, string inputFileContent, string fileNamespace, string resourcesNamespace, MemberAttributes accessModifiers, Type customToolType, out CompilerError [ ] errors )
        {
            var code      = GenerateCode ( codeDomProvider, inputFileName, inputFileContent, fileNamespace, resourcesNamespace, accessModifiers, customToolType, out errors );
            var source    = new StringBuilder ( );
            var generator = new ExtendedCodeGenerator ( codeDomProvider );
            using ( var writer = new StringWriter ( source ) )
                generator.GenerateCodeFromCompileUnit ( code, writer, null );

            return source.ToString ( );
        }

        protected LocalizerSupportBuilder ( CodeDomProvider codeDomProvider, string baseName, ResourceSet resourceSet, string @namespace, string resourcesNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            CodeDomProvider    = codeDomProvider ?? throw new ArgumentNullException ( nameof ( codeDomProvider ) );
            BaseName           = baseName        ?? throw new ArgumentNullException ( nameof ( baseName        ) );
            ResourceSet        = resourceSet     ?? throw new ArgumentNullException ( nameof ( resourceSet     ) );
            Namespace          = IsNullOrEmpty ( @namespace         ) ? null : codeDomProvider.ValidateIdentifier ( @namespace, true );
            ResourcesNamespace = IsNullOrEmpty ( resourcesNamespace ) ? null : resourcesNamespace;
            ResourcesBaseName  = ResourcesNamespace != null ? ResourcesNamespace + '.' + baseName :
                                 Namespace          != null ? Namespace          + '.' + baseName :
                                                              baseName;
            ClassName          = codeDomProvider.ValidateBaseName        ( baseName );
            AccessModifiers    = codeDomProvider.ValidateAccessModifiers ( accessModifiers );
            TypeAttributes     = AccessModifiers.HasFlag ( MemberAttributes.Public ) ? TypeAttributes.Public :
                                                                                       TypeAttributes.AutoLayout;
            CustomToolType     = customToolType;
        }

        protected CodeDomProvider  CodeDomProvider    { get; }
        protected string           BaseName           { get; }
        protected ResourceSet      ResourceSet        { get; }
        protected string           Namespace          { get; }
        protected string           ResourcesNamespace { get; }
        protected string           ResourcesBaseName  { get; }
        protected string           ClassName          { get; }
        protected string           ClassFullName      { get; }
        protected MemberAttributes AccessModifiers    { get; }
        protected TypeAttributes   TypeAttributes     { get; }
        protected Type             CustomToolType     { get; }

        public IResourceNamingStrategy ResourceNamingStrategy            { get; } = Resources.ResourceNamingStrategy.Default;
        public CodeExpression          ResourceNamingStrategyInitializer { get; }

        public bool GenerateWPFSupport { get; set; }

        public CompilerError [ ] GetErrors ( )
        {
            var errors = ResourceSet.Resources.Values.Where ( resource => ! IsNullOrEmpty ( resource.ErrorText ) ).ToArray ( );
            if ( errors.Length == 0 )
                errors = null;

            return errors;
        }

        public virtual CodeCompileUnit Build ( )
        {
            var validResources  = ValidateResourceNames    ( );
            var codeCompileUnit = ConfigureCodeCompileUnit ( new CodeCompileUnit ( ) );
            var codeNamespace   = Code.CreateNamespace ( Namespace ?? ResourcesNamespace, "System" )
                                      .AddTo ( codeCompileUnit );

            var support = GenerateClass ( ).AddTo ( codeNamespace );

            GenerateClassMembers ( out var localizationProvider ).AddRangeTo ( support );

            foreach ( var entry in validResources )
                GenerateProperty ( entry.Key, entry.Name, entry.Resource ).AddTo ( support );

            foreach ( var entry in validResources )
            {
                if ( entry.NumberOfArguments <= 0 )
                    continue;

                var resource   = entry.Resource as StringResource;
                var methodName = entry.Key + FormatMethodSuffix;
                var isUnique   = ! support.Members.Contains ( methodName );
                if ( isUnique && CodeDomProvider.IsValidIdentifier ( methodName ) )
                    GenerateFormatMethod ( localizationProvider, methodName, entry.Key, entry.Name, resource, entry.NumberOfArguments ).AddTo ( support );
                else
                    resource.ErrorText = Format ( CannotCreateFormatMethod, methodName, entry.Name );
            }

            if ( GenerateWPFSupport )
                GenerateWPFTypedLocalizeExtension ( validResources )?.AddTo ( codeNamespace );

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
            yield return ResourceManagerFieldName;
            yield return LocalizationProviderPropertyName;
            yield return LocalizationProviderFieldName;
            yield return CultureInfoPropertyName;
            yield return CultureInfoFieldName;
            yield return NotifyCultureChangedMethodName;

            if ( AccessModifiers.HasFlag ( MemberAttributes.Static ) )
                yield return "Static" + nameof ( INotifyPropertyChanged.PropertyChanged );
            else
                yield return nameof ( INotifyPropertyChanged.PropertyChanged );
        }

        protected virtual CodeTypeDeclaration GenerateClass ( )
        {
            var generator = typeof ( LocalizerSupportBuilder );
            var version   = generator.Assembly.GetName ( ).Version;
            var type      = Code.CreateClass ( ClassName,
                                               AccessModifiers,
                                               CodeDomProvider.Supports ( GeneratorSupport.PartialTypes ) )
                                .AddSummary ( ClassSummary );

            if ( ! AccessModifiers.HasFlag ( MemberAttributes.Static ) )
                type.BaseTypes.Add ( Code.TypeRef < INotifyPropertyChanged > ( ) );

            if ( CustomToolType != null ) type.AddRemarks ( ClassRemarksFormat,         generator.Name, CustomToolType.Name );
            else                          type.AddRemarks ( ClassRemarksToollessFormat, generator.Name );

            return type.Attributed ( Code.Attribute      < GeneratedCodeAttribute       > ( generator.FullName, version.ToString ( ) ),
                                     Code.Attribute      < DebuggerNonUserCodeAttribute > ( ),
                                     Code.NamedAttribute < ObfuscationAttribute         > ( nameof ( ObfuscationAttribute.Exclude        ), true,
                                                                                            nameof ( ObfuscationAttribute.ApplyToMembers ), true ),
                                     Code.Attribute      < SuppressMessageAttribute     > ( "Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces" ) );
        }

        protected virtual IEnumerable < CodeTypeMember > GenerateClassMembers ( out CodeExpression localizationProvider )
        {
            var editorBrowsable = Code.Attribute < EditorBrowsableAttribute > ( Code.Type < EditorBrowsableState > ( )
                                                                                    .Field ( nameof ( EditorBrowsableState.Advanced ) ) );

            var members =
            Enumerable.Empty < CodeTypeMember > ( )
                      .Concat ( new [ ] { GenerateConstructor ( ) } )
                      .Concat ( GenerateCultureChangedEvent ( out var notifyCultureChanged ) )
                      .Concat ( GenerateResourceManagerSingleton ( out var resourceManager ) )
                      .Concat ( new [ ] { GenerateResourceManagerProperty ( resourceManager ).Attributed ( editorBrowsable ) } )
                      .Concat ( new [ ] { GenerateCultureProperty ( notifyCultureChanged, out var cultureField ).Attributed ( editorBrowsable ) } )
                      .Concat ( new [ ] { cultureField } );

            if ( ResourceNamingStrategy != null )
                members = members.Concat ( GenerateLocalizationProviderSingleton ( resourceManager, out localizationProvider ) )
                                 .Concat ( new [ ] { GenerateLocalizationProviderProperty  ( localizationProvider ) } );
            else
                localizationProvider = null;

            return members;
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

        protected virtual IEnumerable < CodeTypeMember > GenerateCultureChangedEvent ( out CodeExpression notifyCultureChanged )
        {
            var propertyChangedEvent = new CodeMemberEvent ( )
            {
                Name       = nameof ( INotifyPropertyChanged.PropertyChanged ),
                Type       = Code.TypeRef < PropertyChangedEventHandler > ( ),
                Attributes = MemberAttributes.Public | AccessModifiers & MemberAttributes.Static
            };

            if ( AccessModifiers.HasFlag ( MemberAttributes.Static ) )
                propertyChangedEvent.Name = "Static" + propertyChangedEvent.Name;
            else
                propertyChangedEvent.ImplementationTypes.Add ( Code.TypeRef < INotifyPropertyChanged > ( ) );

            var propertyChanged = new CodeEventReferenceExpression ( Code.Access ( AccessModifiers ), propertyChangedEvent.Name );
            var notify          = Code.CreateMethod ( typeof ( void ), NotifyCultureChangedMethodName, AccessModifiers );

            notify.Statements.Add ( Code.Declare < PropertyChangedEventHandler > ( NotifyCultureChangedVariableName )
                                        .Initialize ( propertyChanged ) );
            notify.Statements.Add ( Code.If   ( Code.Variable ( NotifyCultureChangedVariableName ).ValueEquals ( Code.Null ) )
                                        .Then ( Code.Return   ( ) ) );
            notify.Statements.Add ( Code.Variable ( NotifyCultureChangedVariableName )
                                        .InvokeDelegate ( Code.Access ( AccessModifiers ) ?? Code.Null,
                                                          Code.TypeRef < PropertyChangedEventArgs > ( )
                                                              .Construct ( Code.Null ) ) );

            notifyCultureChanged = Code.Access ( AccessModifiers )
                                       .Method ( NotifyCultureChangedMethodName )
                                       .Invoke ( );

            return new CodeTypeMember [ ] { propertyChangedEvent, notify };
        }

        protected virtual CodeMemberProperty GenerateCultureProperty ( CodeExpression notifyCultureChanged, out CodeMemberField cultureField )
        {
            cultureField = Code.CreateField < CultureInfo > ( CultureInfoFieldName, MemberAttributes.Private | AccessModifiers & MemberAttributes.Static );

            var field = Code.Access ( AccessModifiers ).Field ( CultureInfoFieldName );

            return Code.CreateProperty < CultureInfo > ( CultureInfoPropertyName, AccessModifiers )
                       .Get ( get          => get.Return ( field ) )
                       .Set ( (set, value) =>
                              {
                                  set.Add ( Code.If   ( field.ObjectEquals ( value ) )
                                                .Then ( Code.Return ( ) ) );
                                  set.Add ( field.Assign ( value ) );
                                  set.Add ( notifyCultureChanged );
                              } )
                       .AddSummary ( CultureInfoPropertySummary );
        }

        protected virtual IEnumerable < CodeTypeMember > GenerateResourceManagerSingleton ( out CodeExpression singleton )
        {
            return GenerateSingleton ( ResourceSet.ResourceManagerType,
                                       ResourceManagerFieldName,
                                       ResourceSet.ResourceManagerInitializer ( ResourcesBaseName, ClassName ),
                                       true,
                                       out singleton );
        }

        protected virtual CodeMemberProperty GenerateResourceManagerProperty ( CodeExpression resourceManager )
        {
            return Code.CreateProperty ( ResourceSet.ResourceManagerType, ResourceManagerPropertyName, AccessModifiers | MemberAttributes.Static, false )
                       .Get            ( get => get.Return ( resourceManager ) )
                       .AddSummary     ( ResourceManagerPropertySummary );
        }

        protected virtual IEnumerable < CodeTypeMember > GenerateLocalizationProviderSingleton ( CodeExpression resourceManager, out CodeExpression localizationProvider )
        {
            var type        = ResourceSet.LocalizationProviderType;
            var initializer = ResourceSet.LocalizationProviderInitializer ( resourceManager, ResourceNamingStrategyInitializer );

            return GenerateSingleton ( type,
                                       LocalizationProviderFieldName,
                                       initializer,
                                       false,
                                       out localizationProvider );
        }

        protected virtual CodeMemberProperty GenerateLocalizationProviderProperty ( CodeExpression localizationProvider )
        {
            return Code.CreateProperty ( ResourceSet.LocalizationProviderType, LocalizationProviderPropertyName, AccessModifiers | MemberAttributes.Static, false )
                       .Get            ( get => get.Return ( localizationProvider ) )
                       .AddSummary     ( LocalizationProviderPropertySummary );
        }

        protected virtual IEnumerable < CodeTypeMember > GenerateSingleton ( CodeTypeReference type, string fieldName, CodeExpression initializer, bool isFirst, out CodeExpression singleton )
        {
            var cctor = (CodeTypeMember) new CodeTypeConstructor ( ).AddComment ( SingletonBeforeFieldInitComment );

            if ( CodeDomProvider.Supports ( GeneratorSupport.NestedTypes ) )
            {
                var lazyType = Code.CreateNestedClass ( fieldName, MemberAttributes.Private | MemberAttributes.Static );

                cctor.AddTo ( lazyType );

                Code.CreateField ( type,
                                   SingletonFieldName,
                                   MemberAttributes.Assembly | MemberAttributes.Static )
                    .Initialize  ( initializer )
                    .AddTo       ( lazyType );

                singleton = Code.Type  ( fieldName, default )
                                .Field ( SingletonFieldName );

                return new [ ] { lazyType };
            }

            singleton = Code.Static ( ).Field ( fieldName );

            var field = Code.CreateField ( type,
                                           fieldName,
                                           MemberAttributes.Private | MemberAttributes.Static )
                            .Initialize  ( initializer );

            if ( isFirst )
                return new [ ] { cctor, field };

            return new [ ] { field };
        }

        protected virtual CodeMemberProperty GenerateProperty ( string propertyName, string resourceName, Resource resource )
        {
            var resourceType    = resource.Type ?? Code.TypeRef < object > ( );
            var resourceManager = Code.Static ( ).Property ( ResourceManagerPropertyName );
            var culture         = Code.Access ( AccessModifiers ).Field ( CultureInfoFieldName );
            var summary         = resource is StringResource stringResource ?
                                  Format ( StringPropertySummary,    GeneratePreview ( stringResource.Value ) ) :
                                  Format ( NonStringPropertySummary, resourceName );

            return Code.CreateProperty ( resourceType, propertyName, AccessModifiers, false )
                       .Get            ( get => get.Return ( resource.Getter ( resourceManager, resourceName, culture ) ) )
                       .AddSummary     ( summary + FormatResourceComment ( resource.Comment ) );
        }

        protected virtual CodeMemberMethod GenerateFormatMethod ( CodeExpression localizationProvider, string methodName, string propertyName, string resourceName, StringResource resource, int numberOfArguments )
        {
            if ( resource == null )
                throw new ArgumentNullException ( nameof ( resource ) );

            if ( numberOfArguments <= 0 )
                throw new ArgumentOutOfRangeException ( nameof ( numberOfArguments ), numberOfArguments, "Number of argument must be greater than zero" );

            var objectType   = Code.TypeRef < object > ( );
            var summary      = Format ( FormatMethodSummary, GeneratePreview ( resource.Value ) );
            var formatMethod = Code.CreateMethod ( typeof ( string ), methodName, AccessModifiers )
                                   .AddSummary   ( summary + FormatResourceComment ( resource.Comment ) );

            var format           = Code.Type < string > ( ).Method ( nameof ( string.Format ) );
            var formatExpression = (CodeExpression) Code.Access ( AccessModifiers ).Property ( propertyName );

            if ( localizationProvider != null )
            {
                format           = localizationProvider.Method ( nameof ( ILocalizationProvider.Format ) );
                formatExpression = Code.Constant ( resourceName );
            }

            if ( numberOfArguments > 3 )
                formatMethod.Attributed ( Code.Attribute < SuppressMessageAttribute > ( "Microsoft.Design", "CA1025:ReplaceRepetitiveArgumentsWithParamsArray" ) );

            var initialArguments = localizationProvider != null ? 3 : 2;

            var parameters = new CodeExpression [ initialArguments + numberOfArguments ];

            parameters [ 0 ] = Code.Access ( AccessModifiers ).Field ( CultureInfoFieldName );

            if ( localizationProvider != null )
            {
                parameters [ 1 ] = Code.Access ( AccessModifiers ).Field ( CultureInfoFieldName );
                parameters [ 2 ] = formatExpression;
            }
            else
                parameters [ 1 ] = formatExpression;

            for ( var index = 0; index < numberOfArguments; index++ )
            {
                var parameterName = Format ( CultureInfo.InvariantCulture, FormatMethodParameterName, index );

                formatMethod.Parameters.Add ( objectType.Parameter ( parameterName ) );

                parameters [ initialArguments + index ] = Code.Variable ( parameterName );

                if ( numberOfArguments > 1 )
                    formatMethod.AddParameterComment ( parameterName, FormatMultiParameterComment, Ordinals [ Math.Min ( index, Ordinals.Length - 1 ) ] );
                else
                    formatMethod.AddParameterComment ( parameterName, FormatParameterComment, index );
            }

            formatMethod.AddReturnComment  ( FormatReturnComment )
                        .Statements.Return ( format.Invoke ( parameters ) );

            return formatMethod;
        }

        protected string GeneratePreview ( string resourceValue )
        {
            if ( resourceValue.Length > ResourcePreviewMaximumLength )
                resourceValue = Format ( TruncatedResourcePreview, resourceValue.Substring ( 0, ResourcePreviewMaximumLength ) );

            return SecurityElement.Escape ( resourceValue );
        }

        protected string FormatResourceComment ( string comment )
        {
            if ( ! IsNullOrWhiteSpace ( comment ) )
                return Format ( ResourceCommentFormat, comment.Trim ( ) );

            return null;
        }

        protected virtual CodeTypeDeclaration GenerateWPFTypedLocalizeExtension ( IList < Entry > entries )
        {
            if ( ! CodeDomProvider.Supports ( GeneratorSupport.NestedTypes ) )
                return null;

            var type           = Code.CreateClass      ( ClassName + "Extension", MemberAttributes.Public );
            var keyEnum        = Code.CreateNestedEnum ( ResourceKeyEnumName, MemberAttributes.Public )
                                     .AddSummary       ( ResourceKeyEnumNameSummary )
                                     .AddTo            ( type );
            var keyEnumTypeRef = Code.TypeRef ( ResourceKeyEnumName, default );

            type.BaseTypes.Add ( Code.TypeRef ( "Localizer.WPF.TypedLocalizeExtension",
                                                CodeTypeReferenceOptions.GlobalReference,
                                                Code.TypeRef ( type.Name + "+" + ResourceKeyEnumName, default ) ) );

            var objectType = Code.TypeRef < object > ( );
            var distinctNumberOfArguments = entries.Select ( entry => entry.NumberOfArguments )
                                                   .DefaultIfEmpty ( 0 )
                                                   .Distinct ( )
                                                   .OrderBy  ( numberOfArguments => numberOfArguments );

            foreach ( var numberOfArguments in distinctNumberOfArguments )
            {
                var ctor = new CodeConstructor ( ) { Attributes = MemberAttributes.Public }.AddTo ( type );

                for ( var argument = 0; argument < numberOfArguments; argument++ )
                {
                    var parameterName = Format ( CultureInfo.InvariantCulture, FormatMethodParameterName, argument );

                    ctor.Parameters         .Add ( objectType.Parameter ( parameterName ) );
                    ctor.BaseConstructorArgs.Add ( Code.Variable        ( parameterName ) );
                }
            }

            Code.CreateField    ( keyEnumTypeRef, "_key", MemberAttributes.Private ).AddTo ( type );
            Code.CreateProperty ( keyEnumTypeRef, "Key", MemberAttributes.Public | MemberAttributes.Override )
                .Get   ( get          => get.Return ( Code.This ( ).Field ( "_key" ) ) )
                .Set   ( (set, value) => set.Add    ( Code.Assign ( Code.This ( ).Field ( "_key" ), value ) ) )
                .AddTo ( type );

            Code.CreateField    ( Code.TypeRef ( "System.Windows.Data.BindingBase" ), "_keyPath", MemberAttributes.Private ).AddTo ( type );
            Code.CreateProperty ( Code.TypeRef ( "System.Windows.Data.BindingBase" ), "KeyPath", MemberAttributes.Public | MemberAttributes.Override )
                .Get        ( get          => get.Return ( Code.This ( ).Field ( "_keyPath" ) ) )
                .Set        ( (set, value) => set.Add    ( Code.Assign ( Code.This ( ).Field ( "_keyPath" ), value ) ) )
                .Attributed ( Code.Attribute < TypeConverterAttribute > ( Code.TypeOf ( Code.TypeRef ( "Localizer.WPF.BindingBaseTypeConverter" ) ) ) )
                .AddTo      ( type );

            Code.CreateField    < Type > ( "_type", MemberAttributes.Private ).AddTo ( type );
            Code.CreateProperty < Type > ( "Type", MemberAttributes.Public | MemberAttributes.Override )
                .Get   ( get          => get.Return ( Code.This ( ).Field ( "_type" ) ) )
                .Set   ( (set, value) => set.Add    ( Code.Assign ( Code.This ( ).Field ( "_type" ), value ) ) )
                .AddTo ( type );

            Code.CreateProperty ( Code.TypeRef < ILocalizationProvider > ( ), "Provider", MemberAttributes.Family | MemberAttributes.Override, false )
                .Get   ( get => get.Return ( Code.Type ( ClassName, default ).Property ( LocalizationProviderPropertyName ) ) )
                .AddTo ( type );

            var translator  = Code.CreateField < string [ ] > ( ResourceKeyTranslatorFieldName, MemberAttributes.Private | MemberAttributes.Static )
                                  .AddTo ( type );
            var translation = new CodeArrayCreateExpression ( Code.TypeRef < string > ( ) );

            translator.InitExpression = translation;

            var index = 0;

            foreach ( var entry in entries )
            {
                Code.CreateField ( keyEnumTypeRef, entry.Key, MemberAttributes.Const | AccessModifiers & ~MemberAttributes.Static )
                    .Initialize  ( Code.Constant ( index++ ) )
                    .AddSummary  ( ResourceKeyFieldSummaryFormat, entry.Name )
                    .AddTo       ( keyEnum );

                translation.Initializers.Add ( Code.Constant ( entry.Name ) );
            }

            var translate = Code.CreateMethod ( typeof ( string ),
                                                "KeyToName",
                                                MemberAttributes.Family | MemberAttributes.Override )
                                .AddTo ( type );

            var key   = Code.Variable ( ResourceKeyParameterName );
            var first = keyEnumTypeRef.ToType ( ).Field ( entries [ 0 ].Key );
            var last  = keyEnumTypeRef.ToType ( ).Field ( entries [ index - 1 ].Key );

            translate.Parameters.Add    ( keyEnumTypeRef.Parameter ( ResourceKeyParameterName ) );
            translate.Statements.Add    ( Code.If   ( key.IsLessThan    ( first ).Or (
                                                      key.IsGreaterThan ( last  ) ) )
                                              .Then ( Code.Throw < ArgumentOutOfRangeException > ( Code.Constant ( ResourceKeyParameterName ) ) ) );
            translate.Statements.Return ( Code.Static  ( )
                                              .Field   ( ResourceKeyTranslatorFieldName )
                                              .Indexer ( Code.Variable ( ResourceKeyParameterName ).Cast < int > ( ) ) );

            return type;
        }

        protected virtual IList < Entry > ValidateResourceNames ( )
        {
            var classProperties = new HashSet < string > ( GetClassMemberNames ( ) );
            var entries         = new SortedList < string, Entry > ( ResourceSet.Resources.Count, StringComparer.InvariantCultureIgnoreCase );

            foreach ( var entry in ResourceSet.Resources )
            {
                var name     = entry.Key;
                var key      = name;
                var resource = entry.Value;

                if ( classProperties.Contains ( name ) )
                {
                    resource.ErrorText = Format ( PropertyAlreadyExists, name );
                    continue;
                }

                if ( typeof ( void ).FullName == resource.Type?.BaseType )
                {
                    resource.ErrorText = Format ( InvalidPropertyType, resource.Type, name );
                    continue;
                }

                var isWinFormsLocalizableResource = name.Length > 0 && name [ 0 ] == '$' ||
                                                    name.Length > 1 && name [ 0 ] == '>' &&
                                                                       name [ 1 ] == '>';
                if ( isWinFormsLocalizableResource )
                {
                    resource.IsWarning = true;
                    resource.ErrorText = Format ( SkippingWinFormsResource, name );
                    continue;
                }

                var isBaseName = ResourceNamingStrategy == null ||
                                 ResourceNamingStrategy.ParseArguments ( PluralRules.Invariant, name, out var _ ) == name;
                if ( ! isBaseName )
                    continue;

                if ( ! CodeDomProvider.IsValidIdentifier ( key ) )
                {
                    key = CodeDomProvider.ValidateIdentifier ( key );

                    if ( key == null )
                    {
                        resource.ErrorText = Format ( CannotCreateResourceProperty, name );
                        continue;
                    }
                }

                if ( entries.TryGetValue ( key, out var duplicate ) )
                {
                    if ( IsNullOrEmpty ( duplicate.Resource.ErrorText ) )
                        duplicate.Resource.ErrorText = Format ( CannotCreateResourceProperty, duplicate.Name );

                    entries.Remove ( key );

                    resource.ErrorText = Format ( CannotCreateResourceProperty, name );

                    continue;
                }

                entries.Add ( key, new Entry ( name, key, resource ) );
            }

            return entries.Values.ToList ( );
        }

        protected class Entry
        {
            public Entry ( string name, string key, Resource resource )
            {
                Name              = name;
                Key               = key;
                Resource          = resource;
                NumberOfArguments = GetNumberOfArguments ( name, resource );
            }

            public string   Name              { get; }
            public string   Key               { get; }
            public Resource Resource          { get; }
            public int      NumberOfArguments { get; }

            private static int GetNumberOfArguments ( string resourceName, Resource resource )
            {
                if ( ! ( resource is StringResource stringResource ) )
                    return 0;

                try
                {
                    var formatString      = FormatString.Parse ( stringResource.Value );
                    var numberOfArguments = formatString.ArgumentHoles
                                                        .Select ( argumentHole => argumentHole.Index )
                                                        .DefaultIfEmpty ( -1 )
                                                        .Max ( ) + 1;
                    return numberOfArguments;
                }
                catch ( FormatException exception )
                {
                    resource.ErrorText = Format ( ErrorInStringResourceFormat, exception.Message, resourceName );
                }

                return 0;
            }
        }
    }
}