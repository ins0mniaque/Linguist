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

using Linguist.CodeDom;
using Linguist.Resources;

namespace Linguist.Generator
{
    using static String;
    using static Comments;
    using static ErrorMessages;
    using static MemberNames;

    public class LinguistSupportBuilder
    {
        public static LinguistSupportBuilder GenerateBuilder ( CodeDomProvider codeDomProvider, string inputFileName, string inputFileContent, string fileNamespace, string resourcesNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            var extractorType = AutoDetect.ResourceExtractorType ( inputFileName );
            if ( extractorType == null )
                throw new ArgumentException ( Format ( UnknownResourceFileFormat, Path.GetFileName ( inputFileName ) ), nameof ( inputFileName ) );

            var extractor   = (IResourceExtractor) Activator.CreateInstance ( extractorType, new MemoryStream ( Encoding.UTF8.GetBytes ( inputFileContent ) ) );
            var resourceSet = extractor.Extract ( ).ToList ( );
            var settings    = new LinguistSupportBuilderSettings ( );
            var baseName    = Path.GetFileNameWithoutExtension ( inputFileName );

            settings.BaseName           = baseName ?? throw new ArgumentNullException ( nameof ( baseName ) );
            settings.Namespace          = IsNullOrEmpty ( fileNamespace      ) ? null : codeDomProvider.ValidateIdentifier ( fileNamespace, true );
            settings.ResourcesNamespace = IsNullOrEmpty ( resourcesNamespace ) ? null : resourcesNamespace;
            settings.ResourceSetType    = AutoDetect.ResourceSetType ( inputFileName ).FullName;
            settings.AccessModifiers    = codeDomProvider.ValidateAccessModifiers ( accessModifiers );
            settings.CustomToolType     = customToolType;

            return new LinguistSupportBuilder ( codeDomProvider, resourceSet, settings );
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

        protected LinguistSupportBuilder ( CodeDomProvider codeDomProvider, IList < IResource > resourceSet, LinguistSupportBuilderSettings settings )
        {
            CodeDomProvider = codeDomProvider ?? throw new ArgumentNullException ( nameof ( codeDomProvider ) );
            ResourceSet     = resourceSet     ?? throw new ArgumentNullException ( nameof ( resourceSet     ) );
            Settings        = settings        ?? throw new ArgumentNullException ( nameof ( settings        ) );
        }

        public CodeDomProvider                CodeDomProvider { get; }
        public IList < IResource >            ResourceSet     { get; }
        public LinguistSupportBuilderSettings Settings        { get; }

        protected Dictionary < IResource, CompilerError > Errors { get; } = new Dictionary < IResource, CompilerError > ( );

        public CompilerError [ ] GetErrors ( )
        {
            foreach ( var entry in Errors )
            {
                entry.Value.FileName = entry.Key.Source;
                entry.Value.Line     = entry.Key.Line   ?? 0;
                entry.Value.Column   = entry.Key.Column ?? 0;
            }

            return Errors.Values.ToArray ( );
        }

        protected void AddError   ( IResource resource, string error   ) => AddError ( resource, null, error,   false );
        protected void AddWarning ( IResource resource, string warning ) => AddError ( resource, null, warning, true  );

        private void AddError ( IResource resource, string number, string message, bool isWarning )
        {
            if ( Errors.TryGetValue ( resource, out var error ) )
            {
                error.ErrorNumber = error.ErrorNumber ?? number;

                if ( ! error.ErrorText.Contains ( message ) )
                    error.ErrorText += "\n" + message;

                if ( ! isWarning )
                    error.IsWarning = false;
            }
            else
                Errors.Add ( resource, new CompilerError ( ) { ErrorNumber = number,
                                                               ErrorText   = message,
                                                               IsWarning   = isWarning } );
        }

        protected LinguistSupportBuilderSettings settings;

        public virtual CodeCompileUnit Build ( )
        {
            settings = Settings.Setup ( CodeDomProvider );

            var validResources  = ValidateResourceNames    ( );
            var codeCompileUnit = ConfigureCodeCompileUnit ( new CodeCompileUnit ( ) );
            var codeNamespace   = Code.CreateNamespace ( settings.Namespace ?? settings.ResourcesNamespace, "System" )
                                      .AddTo ( codeCompileUnit );

            var support = GenerateClass ( ).AddTo ( codeNamespace );

            GenerateClassMembers ( ).AddRangeTo ( support );

            foreach ( var entry in validResources )
                GenerateProperty ( entry.Key, entry.Resource ).AddTo ( support );

            foreach ( var entry in validResources )
            {
                if ( entry.NumberOfArguments <= 0 )
                    continue;

                var methodName = entry.Key + FormatMethodSuffix;
                var isUnique   = ! support.Members.Contains ( methodName );
                if ( isUnique && CodeDomProvider.IsValidIdentifier ( methodName ) )
                    GenerateFormatMethod ( methodName, entry.Key, entry.Resource, entry.NumberOfArguments ).AddTo ( support );
                else
                    AddError ( entry.Resource, Format ( CannotCreateFormatMethod, methodName, entry.Resource.Name ) );
            }

            if ( settings.GenerateWPFSupport )
                GenerateWPFTypedLocalizeExtension ( validResources )?.AddTo ( codeNamespace );
            else if ( settings.GenerateXamarinFormsSupport )
                GenerateXamarinFormsTypedLocalizeExtension ( validResources )?.AddTo ( codeNamespace );

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
            yield return LocalizerPropertyName;
            yield return LocalizerFieldName;
            yield return CultureInfoPropertyName;
            yield return CultureInfoFieldName;
            yield return NotifyCultureChangedMethodName;

            if ( settings.AccessModifiers.HasBitMask ( MemberAttributes.Static ) )
                yield return "Static" + nameof ( INotifyPropertyChanged.PropertyChanged );
            else
                yield return nameof ( INotifyPropertyChanged.PropertyChanged );
        }

        protected virtual CodeTypeDeclaration GenerateClass ( )
        {
            var generator = typeof ( LinguistSupportBuilder );
            var version   = generator.Assembly.GetName ( ).Version;
            var type      = Code.CreateClass ( settings.ClassName,
                                               settings.AccessModifiers,
                                               CodeDomProvider.Supports ( GeneratorSupport.PartialTypes ) )
                                .AddSummary ( ClassSummary );

            if ( ! settings.AccessModifiers.HasBitMask ( MemberAttributes.Static ) )
                type.BaseTypes.Add ( Code.TypeRef < INotifyPropertyChanged > ( ) );

            if ( settings.CustomToolType != null ) type.AddRemarks ( ClassRemarksFormat,         generator.Name, settings.CustomToolType.Name );
            else                                   type.AddRemarks ( ClassRemarksToollessFormat, generator.Name );

            return type.Attributed ( Code.Attribute      < GeneratedCodeAttribute       > ( generator.FullName, version.ToString ( ) ),
                                     Code.Attribute      < DebuggerNonUserCodeAttribute > ( ),
                                     Code.NamedAttribute < ObfuscationAttribute         > ( nameof ( ObfuscationAttribute.Exclude        ), true,
                                                                                            nameof ( ObfuscationAttribute.ApplyToMembers ), true ),
                                     Code.Attribute      < SuppressMessageAttribute     > ( "Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces" ) );
        }

        protected virtual IEnumerable < CodeTypeMember > GenerateClassMembers ( )
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

            if ( settings.LocalizerType != null )
                members = members.Concat ( GenerateLocalizerSingleton ( resourceManager, out var localizer ) )
                                 .Concat ( new [ ] { GenerateLocalizerProperty ( localizer ) } );

            return members;
        }

        protected virtual CodeConstructor GenerateConstructor ( )
        {
            var ctor = new CodeConstructor ( )
            {
                Attributes = settings.AccessModifiers.HasBitMask ( MemberAttributes.Static ) ? MemberAttributes.Private :
                                                                                               settings.AccessModifiers & ~MemberAttributes.Static
            };

            return ctor.AddSummary ( ConstructorSummaryFormat, settings.ClassName )
                       .Attributed ( Code.Attribute < SuppressMessageAttribute > ( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ) );
        }

        protected virtual IEnumerable < CodeTypeMember > GenerateCultureChangedEvent ( out CodeExpression notifyCultureChanged )
        {
            var propertyChangedEvent = new CodeMemberEvent ( )
            {
                Name       = nameof ( INotifyPropertyChanged.PropertyChanged ),
                Type       = Code.TypeRef < PropertyChangedEventHandler > ( ),
                Attributes = MemberAttributes.Public | settings.AccessModifiers & MemberAttributes.Static
            };

            if ( settings.AccessModifiers.HasBitMask ( MemberAttributes.Static ) )
                propertyChangedEvent.Name = "Static" + propertyChangedEvent.Name;
            else
                propertyChangedEvent.ImplementationTypes.Add ( Code.TypeRef < INotifyPropertyChanged > ( ) );

            var propertyChanged = new CodeEventReferenceExpression ( Code.Access ( settings.AccessModifiers ), propertyChangedEvent.Name );
            var notify          = Code.CreateMethod ( typeof ( void ), NotifyCultureChangedMethodName, settings.AccessModifiers );

            notify.Statements.Add ( Code.Declare < PropertyChangedEventHandler > ( NotifyCultureChangedVariableName )
                                        .Initialize ( propertyChanged ) );
            notify.Statements.Add ( Code.If   ( Code.Variable ( NotifyCultureChangedVariableName ).ValueEquals ( Code.Null ) )
                                        .Then ( Code.Return   ( ) ) );
            notify.Statements.Add ( Code.Variable ( NotifyCultureChangedVariableName )
                                        .InvokeDelegate ( Code.Access ( settings.AccessModifiers ) ?? Code.Null,
                                                          Code.TypeRef < PropertyChangedEventArgs > ( )
                                                              .Construct ( Code.Null ) ) );

            notifyCultureChanged = Code.Access ( settings.AccessModifiers )
                                       .Method ( NotifyCultureChangedMethodName )
                                       .Invoke ( );

            return new CodeTypeMember [ ] { propertyChangedEvent, notify };
        }

        protected virtual CodeMemberProperty GenerateCultureProperty ( CodeExpression notifyCultureChanged, out CodeMemberField cultureField )
        {
            cultureField = Code.CreateField < CultureInfo > ( CultureInfoFieldName, MemberAttributes.Private | settings.AccessModifiers & MemberAttributes.Static );

            var field = Code.Access ( settings.AccessModifiers ).Field ( CultureInfoFieldName );

            return Code.CreateProperty < CultureInfo > ( CultureInfoPropertyName, settings.AccessModifiers )
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
            return GenerateSingleton ( settings.ResourceManagerType,
                                       ResourceManagerFieldName,
                                       settings.ResourceManagerInitializer,
                                       true,
                                       out singleton );
        }

        protected virtual CodeMemberProperty GenerateResourceManagerProperty ( CodeExpression resourceManager )
        {
            return Code.CreateProperty ( settings.ResourceManagerType, ResourceManagerPropertyName, settings.AccessModifiers | MemberAttributes.Static, false )
                       .Get            ( get => get.Return ( resourceManager ) )
                       .AddSummary     ( ResourceManagerPropertySummary );
        }

        protected virtual IEnumerable < CodeTypeMember > GenerateLocalizerSingleton ( CodeExpression resourceManager, out CodeExpression localizer )
        {
            var type        = settings.LocalizerType;
            var initializer = settings.LocalizerInitializer;

            return GenerateSingleton ( type,
                                       LocalizerFieldName,
                                       initializer,
                                       false,
                                       out localizer );
        }

        protected virtual CodeMemberProperty GenerateLocalizerProperty ( CodeExpression localizer )
        {
            return Code.CreateProperty ( settings.LocalizerType, LocalizerPropertyName, settings.AccessModifiers | MemberAttributes.Static, false )
                       .Get            ( get => get.Return ( localizer ) )
                       .AddSummary     ( LocalizerPropertySummary );
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

        protected virtual CodeMemberProperty GenerateProperty ( string propertyName, IResource resource )
        {
            var resourceType = resource.Type != null ? Code.TypeRef ( resource.Type, default ) : Code.TypeRef < object > ( );
            var summary      = resource.Type == typeof ( string ).FullName ?
                               Format ( StringPropertySummary,    GeneratePreview ( (string) resource.Value ) ) :
                               Format ( NonStringPropertySummary, resource.Name );

            return Code.CreateProperty ( resourceType, propertyName, settings.AccessModifiers, false )
                       .Get            ( get => get.Return ( GenerateResourceGetter ( resource ) ) )
                       .AddSummary     ( summary + FormatResourceComment ( resource.Comment ) );
        }

        protected virtual CodeExpression GenerateResourceGetter ( IResource resource )
        {
            var culture = Code.Access ( settings.AccessModifiers ).Field ( CultureInfoFieldName );

            if ( settings.LocalizerType != null )
            {
                var localizer = Code.Static ( ).Property ( LocalizerPropertyName );

                if ( resource.Type == typeof ( string ).FullName )
                    return localizer.Method ( "GetString" )
                                    .Invoke ( culture, Code.Constant ( resource.Name ) );

                return localizer.Method ( "GetObject" )
                                .Invoke ( culture, Code.Constant ( resource.Name ) )
                                .Cast   ( Code.TypeRef ( resource.Type, default ) );
            }
            else
            {
                var resourceManager = Code.Static ( ).Property ( ResourceManagerPropertyName );

                if ( resource.Type == typeof ( string ).FullName )
                    return resourceManager.Method ( "GetString" )
                                          .Invoke ( Code.Constant ( resource.Name ), culture );

                if ( resource.Type == typeof ( Stream                ).FullName ||
                     resource.Type == typeof ( MemoryStream          ).FullName ||
                     resource.Type == typeof ( UnmanagedMemoryStream ).FullName )
                    return resourceManager.Method ( "GetStream" )
                                          .Invoke ( Code.Constant ( resource.Name ), culture );

                return resourceManager.Method ( "GetObject" )
                                      .Invoke ( Code.Constant ( resource.Name ), culture )
                                      .Cast   ( Code.TypeRef ( resource.Type, default ) );
            }
        }

        protected virtual CodeMemberMethod GenerateFormatMethod ( string methodName, string propertyName, IResource resource, int numberOfArguments )
        {
            if ( resource == null )
                throw new ArgumentNullException ( nameof ( resource ) );

            if ( numberOfArguments <= 0 )
                throw new ArgumentOutOfRangeException ( nameof ( numberOfArguments ), numberOfArguments, "Number of argument must be greater than zero" );

            var localizer = (CodeExpression) null;
            if ( settings.LocalizerType != null )
                localizer = Code.Static ( ).Property ( LocalizerPropertyName );

            var objectType   = Code.TypeRef < object > ( );
            var summary      = Format ( FormatMethodSummary, GeneratePreview ( (string) resource.Value ) );
            var formatMethod = Code.CreateMethod ( typeof ( string ), methodName, settings.AccessModifiers )
                                   .AddSummary   ( summary + FormatResourceComment ( resource.Comment ) );

            var format           = Code.Type < string > ( ).Method ( nameof ( string.Format ) );
            var formatExpression = (CodeExpression) Code.Access ( settings.AccessModifiers ).Property ( propertyName );

            if ( localizer != null )
            {
                format           = localizer.Method ( nameof ( ILocalizer.Format ) );
                formatExpression = Code.Constant ( resource.Name );
            }

            if ( numberOfArguments > 3 )
                formatMethod.Attributed ( Code.Attribute < SuppressMessageAttribute > ( "Microsoft.Design", "CA1025:ReplaceRepetitiveArgumentsWithParamsArray" ) );

            var initialArguments = localizer != null ? 3 : 2;

            var parameters = new CodeExpression [ initialArguments + numberOfArguments ];

            parameters [ 0 ] = Code.Access ( settings.AccessModifiers ).Field ( CultureInfoFieldName );

            if ( localizer != null )
            {
                parameters [ 1 ] = Code.Access ( settings.AccessModifiers ).Field ( CultureInfoFieldName );
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
            comment = comment?.Trim ( );
            if ( ! IsNullOrEmpty ( comment ) )
                return Format ( ResourceCommentFormat, comment );

            return null;
        }

        protected virtual CodeTypeDeclaration GenerateWPFTypedLocalizeExtension ( IList < Entry > entries )
        {
            return GenerateTypedLocalizeExtension ( entries,
                                                    "Linguist.WPF.TypedLocalizeExtension",
                                                    true,
                                                    "System.Windows.Data.BindingBase",
                                                    Code.Attribute < TypeConverterAttribute > ( Code.TypeOf ( Code.TypeRef ( "Linguist.WPF.BindingSyntax+TypeConverter" ) ) ) );
        }

        protected virtual CodeTypeDeclaration GenerateXamarinFormsTypedLocalizeExtension ( IList < Entry > entries )
        {
            return GenerateTypedLocalizeExtension ( entries,
                                                    "Linguist.Xamarin.Forms.TypedLocalizeExtension",
                                                    false,
                                                    "Xamarin.Forms.BindingBase",
                                                    Code.Attribute ( Code.TypeRef ( "Xamarin.Forms.TypeConverterAttribute" ),
                                                                     Code.TypeOf ( Code.TypeRef ( "Linguist.Xamarin.Forms.BindingSyntax+TypeConverter" ) ) ) );
        }

        protected virtual CodeTypeDeclaration GenerateTypedLocalizeExtension ( IList < Entry > entries, string extensionType, bool generateConstructors, string bindingType, CodeAttributeDeclaration bindingTypeConverter )
        {
            if ( ! CodeDomProvider.Supports ( GeneratorSupport.NestedTypes ) )
                return null;

            var type           = Code.CreateClass      ( settings.ClassName + "Extension", MemberAttributes.Public );
            var keyEnum        = Code.CreateNestedEnum ( ResourceKeyEnumName, MemberAttributes.Public )
                                     .AddSummary       ( ResourceKeyEnumNameSummary )
                                     .AddTo            ( type );
            var keyEnumTypeRef = Code.TypeRef ( ResourceKeyEnumName, default );

            type.BaseTypes.Add ( Code.TypeRef ( extensionType,
                                                CodeTypeReferenceOptions.GlobalReference,
                                                Code.TypeRef ( type.Name + "+" + ResourceKeyEnumName, default ) ) );

            var objectType = Code.TypeRef < object > ( );
            var distinctNumberOfArguments = entries.Select ( entry => entry.NumberOfArguments )
                                                   .DefaultIfEmpty ( 0 )
                                                   .Distinct ( )
                                                   .OrderBy  ( numberOfArguments => numberOfArguments );

            if ( generateConstructors )
            {
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
            }

            Code.CreateField    ( keyEnumTypeRef, "_key", MemberAttributes.Private ).AddTo ( type );
            Code.CreateProperty ( keyEnumTypeRef, "Key", MemberAttributes.Public | MemberAttributes.Override )
                .Get   ( get          => get.Return ( Code.This ( ).Field ( "_key" ) ) )
                .Set   ( (set, value) => set.Add    ( Code.Assign ( Code.This ( ).Field ( "_key" ), value ) ) )
                .AddTo ( type );

            var bindingTypeRef = Code.TypeRef ( bindingType );

            Code.CreateField    ( bindingTypeRef, "_keyPath", MemberAttributes.Private ).AddTo ( type );
            Code.CreateProperty ( bindingTypeRef, "KeyPath", MemberAttributes.Public | MemberAttributes.Override )
                .Get        ( get          => get.Return ( Code.This ( ).Field ( "_keyPath" ) ) )
                .Set        ( (set, value) => set.Add    ( Code.Assign ( Code.This ( ).Field ( "_keyPath" ), value ) ) )
                .Attributed ( bindingTypeConverter )
                .AddTo      ( type );

            Code.CreateField    < Type > ( "_type", MemberAttributes.Private ).AddTo ( type );
            Code.CreateProperty < Type > ( "Type", MemberAttributes.Public | MemberAttributes.Override )
                .Get   ( get          => get.Return ( Code.This ( ).Field ( "_type" ) ) )
                .Set   ( (set, value) => set.Add    ( Code.Assign ( Code.This ( ).Field ( "_type" ), value ) ) )
                .AddTo ( type );

            Code.CreateProperty ( Code.TypeRef < ILocalizer > ( ), "Localizer", MemberAttributes.Family | MemberAttributes.Override, false )
                .Get   ( get => get.Return ( Code.Type ( settings.ClassName, default ).Property ( LocalizerPropertyName ) ) )
                .AddTo ( type );

            var translator  = Code.CreateField < string [ ] > ( ResourceKeyTranslatorFieldName, MemberAttributes.Private | MemberAttributes.Static )
                                  .AddTo ( type );
            var translation = new CodeArrayCreateExpression ( Code.TypeRef < string > ( ) );

            translator.InitExpression = translation;

            var index = 0;

            foreach ( var entry in entries )
            {
                Code.CreateField ( keyEnumTypeRef, entry.Key, MemberAttributes.Const | settings.AccessModifiers & ~MemberAttributes.Static )
                    .Initialize  ( Code.Constant ( index++ ) )
                    .AddSummary  ( ResourceKeyFieldSummaryFormat, entry.Resource.Name )
                    .AddTo       ( keyEnum );

                translation.Initializers.Add ( Code.Constant ( entry.Resource.Name ) );
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
            var classProperties = new HashSet    < string > ( GetClassMemberNames ( ) );
            var entries         = new SortedList < string, Entry > ( ResourceSet.Count, StringComparer.InvariantCultureIgnoreCase );

            foreach ( var resource in ResourceSet )
            {
                var name = resource.Name;
                var key  = name;

                if ( classProperties.Contains ( name ) )
                {
                    AddError ( resource, Format ( PropertyAlreadyExists, name ) );
                    continue;
                }

                if ( typeof ( void ).FullName == resource.Type )
                {
                    AddError ( resource, Format ( InvalidPropertyType, resource.Type, name ) );
                    continue;
                }

                var isWinFormsLocalizableResource = name.Length > 0 && name [ 0 ] == '$' ||
                                                    name.Length > 1 && name [ 0 ] == '>' &&
                                                                       name [ 1 ] == '>';
                if ( isWinFormsLocalizableResource )
                {
                    AddWarning ( resource, Format ( SkippingWinFormsResource, name ) );
                    continue;
                }

                var isBaseName = settings.ResourceNamingStrategy == null ||
                                 settings.ResourceNamingStrategy.ParseArguments ( PluralRules.Invariant, name, out _ ) == name;
                if ( ! isBaseName )
                    continue;

                if ( ! CodeDomProvider.IsValidIdentifier ( key ) )
                {
                    key = CodeDomProvider.ValidateIdentifier ( key );

                    if ( key == null )
                    {
                        AddError ( resource, Format ( CannotCreateResourceProperty, name ) );
                        continue;
                    }
                }

                if ( entries.TryGetValue ( key, out var duplicate ) )
                {
                    AddError ( duplicate.Resource, Format ( CannotCreateResourceProperty, duplicate.Resource.Name ) );

                    entries.Remove ( key );

                    AddError ( resource, Format ( CannotCreateResourceProperty, name ) );

                    continue;
                }

                entries.Add ( key, new Entry ( key, resource, GetNumberOfArguments ( resource ) ) );
            }

            return entries.Values.ToList ( );
        }

        private int GetNumberOfArguments ( IResource resource )
        {
            if ( resource.Type != typeof ( string ).FullName )
                return 0;

            try
            {
                var formatString      = FormatString.Parse ( (string) resource.Value );
                var numberOfArguments = formatString.ArgumentHoles
                                                    .Select ( argumentHole => argumentHole.Index )
                                                    .DefaultIfEmpty ( -1 )
                                                    .Max ( ) + 1;
                return numberOfArguments;
            }
            catch ( FormatException exception )
            {
                AddError ( resource, Format ( ErrorInStringResourceFormat, exception.Message, resource.Name ) );
            }

            return 0;
        }

        protected class Entry
        {
            public Entry ( string key, IResource resource, int numberOfArguments )
            {
                Key               = key;
                Resource          = resource;
                NumberOfArguments = numberOfArguments;
            }

            public string    Key               { get; }
            public IResource Resource          { get; }
            public int       NumberOfArguments { get; }
        }
    }
}