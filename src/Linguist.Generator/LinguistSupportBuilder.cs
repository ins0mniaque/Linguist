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
using Linguist.CodeDom.Fluent;
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

            var codeCompileUnit = ConfigureCodeCompileUnit ( new CodeCompileUnit ( ) );
            var codeNamespace   = codeCompileUnit.Namespaces.Add ( settings.Namespace ?? settings.ResourcesNamespace, "System" );

            var support = GenerateClass ( codeNamespace );

            GenerateClassMembers ( support );

            var validResources  = ValidateResourceNames ( support );

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
                codeNamespace.Types.Add ( TypedLocalizeExtensionBuilder.WPF ( settings.ClassName,
                                                                              settings.AccessModifiers & ~MemberAttributes.Static,
                                                                              validResources ) );
            else if ( settings.GenerateXamarinFormsSupport )
                codeNamespace.Types.Add ( TypedLocalizeExtensionBuilder.XamarinForms ( settings.ClassName,
                                                                                       settings.AccessModifiers & ~MemberAttributes.Static,
                                                                                       validResources ) );

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

        protected virtual CodeTypeDeclaration GenerateClass ( CodeNamespace @namespace )
        {
            var generator = typeof ( LinguistSupportBuilder );
            var version   = generator.Assembly.GetName ( ).Version;
            var type      = Declare.Class      ( settings.ClassName )
                                   .Modifiers  ( settings.AccessModifiers )
                                   .IsPartial  ( CodeDomProvider.Supports ( GeneratorSupport.PartialTypes ) )
                                   .AddSummary ( ClassSummary )
                                   .AddTo      ( @namespace );

            if ( settings.CustomToolType != null ) type.AddRemarks ( ClassRemarksFormat,         generator.Name, settings.CustomToolType.Name );
            else                                   type.AddRemarks ( ClassRemarksToollessFormat, generator.Name );

            return type.Attributed ( Declare.Attribute < GeneratedCodeAttribute       > ( generator.FullName, version.ToString ( ) ),
                                     Declare.Attribute < DebuggerNonUserCodeAttribute > ( ),
                                     Declare.Attribute < ObfuscationAttribute         > ( )
                                            .WithArgument ( nameof ( ObfuscationAttribute.Exclude        ), true )
                                            .WithArgument ( nameof ( ObfuscationAttribute.ApplyToMembers ), true ),
                                     Declare.Attribute < SuppressMessageAttribute     > ( "Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces" ) );
        }

        protected virtual void GenerateClassMembers ( CodeTypeDeclaration support )
        {
            var editorBrowsable      = Declare.Attribute < EditorBrowsableAttribute > ( Code.Constant ( EditorBrowsableState.Advanced ) );
            var notifyCultureChanged = (CodeExpression) null;

            GenerateConstructor ( support );

            GenerateCultureChangedEvent ( support, out notifyCultureChanged );

            var resourceManager = GenerateResourceManagerSingleton ( support );

            if ( settings.LocalizerType != null )
            {
                var localizer = GenerateLocalizerSingleton ( support, resourceManager );

                GenerateLocalizerProperty ( support, localizer );
            }

            GenerateResourceManagerProperty ( support, resourceManager ).Attributed ( editorBrowsable );

            GenerateCultureProperty ( support, notifyCultureChanged ).Attributed ( editorBrowsable );
        }

        protected virtual CodeConstructor GenerateConstructor ( CodeTypeDeclaration support )
        {
            var ctor = new CodeConstructor ( )
            {
                Attributes = settings.AccessModifiers.HasBitMask ( MemberAttributes.Static ) ? MemberAttributes.Private :
                                                                                               settings.AccessModifiers & ~MemberAttributes.Static
            };

            return ctor.AddSummary ( ConstructorSummaryFormat, settings.ClassName )
                       .Attributed ( Declare.Attribute < SuppressMessageAttribute > ( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ) );
        }

        protected virtual CodeMemberEvent GenerateCultureChangedEvent ( CodeTypeDeclaration support, out CodeExpression notifyCultureChanged )
        {
            var propertyChangedEvent = Declare.Event < PropertyChangedEventHandler > ( nameof ( INotifyPropertyChanged.PropertyChanged ) )
                                              .AddTo ( support );

            if ( ! settings.AccessModifiers.HasBitMask ( MemberAttributes.Static ) )
            {
                support             .BaseTypes          .Add ( Code.Type < INotifyPropertyChanged > ( ) );
                propertyChangedEvent.ImplementationTypes.Add ( Code.Type < INotifyPropertyChanged > ( ) );
            }
            else
                propertyChangedEvent.Static ( ).Name = "Static" + propertyChangedEvent.Name;

            var propertyChanged = Code.Event ( support.Instance ( ), propertyChangedEvent.Name );
            var notify          = Declare.Method ( NotifyCultureChangedMethodName, settings.AccessModifiers )
                                         .Define ( method =>
                                                   {
                                                       method.Add ( Declare.Variable < PropertyChangedEventHandler > ( NotifyCultureChangedVariableName )
                                                                           .Initialize ( propertyChanged ) );
                                                       method.Add ( Code.If   ( Code.Variable ( NotifyCultureChangedVariableName ).ValueEquals ( Code.Null ) )
                                                                        .Then ( Code.Return   ( ) ) );
                                                       method.Add ( Code.Variable ( NotifyCultureChangedVariableName )
                                                                        .InvokeDelegate ( support.Instance ( ) ?? Code.Null,
                                                                                          Code.Type < PropertyChangedEventArgs > ( )
                                                                                              .Construct ( Code.Null ) ) );
                                                   } )
                                         .AddTo  ( support );

            notifyCultureChanged = support.Instance ( )
                                          .Method ( NotifyCultureChangedMethodName )
                                          .Invoke ( );

            return propertyChangedEvent;
        }

        protected virtual CodeMemberProperty GenerateCultureProperty ( CodeTypeDeclaration support, CodeExpression notifyCultureChanged )
        {
            var cultureField = Declare.Field < CultureInfo > ( CultureInfoFieldName )
                                      .Modifiers ( settings.AccessModifiers & MemberAttributes.Static )
                                      .AddTo ( support );

            var field = support.Instance ( ).Field ( CultureInfoFieldName );

            return Declare.Property < CultureInfo > ( CultureInfoPropertyName )
                          .Modifiers ( settings.AccessModifiers )
                          .Get ( get          => get.Return ( field ) )
                          .Set ( (set, value) =>
                                 {
                                     set.Add ( Code.If   ( field.ObjectEquals ( value ) )
                                                   .Then ( Code.Return ( ) ) );
                                     set.Add ( field.Assign ( value ) );
                                     if ( notifyCultureChanged != null )
                                         set.Add ( notifyCultureChanged );
                                 } )
                          .AddSummary ( CultureInfoPropertySummary )
                          .AddTo ( support );
        }

        protected virtual CodeExpression GenerateResourceManagerSingleton ( CodeTypeDeclaration support )
        {
            return GenerateSingleton ( support,
                                       settings.ResourceManagerType,
                                       ResourceManagerFieldName,
                                       settings.ResourceManagerInitializer );
        }

        protected virtual CodeMemberProperty GenerateResourceManagerProperty ( CodeTypeDeclaration support, CodeExpression resourceManager )
        {
            return Declare.Property   ( settings.ResourceManagerType, ResourceManagerPropertyName ).Static ( )
                          .Modifiers  ( settings.AccessModifiers )
                          .Get        ( get => get.Return ( resourceManager ) )
                          .AddSummary ( ResourceManagerPropertySummary )
                          .AddTo      ( support );
        }

        protected virtual CodeExpression GenerateLocalizerSingleton ( CodeTypeDeclaration support, CodeExpression resourceManager )
        {
            return GenerateSingleton ( support,
                                       settings.LocalizerType,
                                       LocalizerFieldName,
                                       settings.LocalizerInitializer );
        }

        protected virtual CodeMemberProperty GenerateLocalizerProperty ( CodeTypeDeclaration support, CodeExpression localizer )
        {
            return Declare.Property   ( settings.LocalizerType, LocalizerPropertyName ).Static ( )
                          .Modifiers  ( settings.AccessModifiers )
                          .Get        ( get => get.Return ( localizer ) )
                          .AddSummary ( LocalizerPropertySummary )
                          .AddTo      ( support );
        }

        protected virtual CodeExpression GenerateSingleton ( CodeTypeDeclaration support, CodeTypeReference type, string fieldName, CodeExpression initializer )
        {
            var cctor = (CodeTypeMember) new CodeTypeConstructor ( ).AddComment ( SingletonBeforeFieldInitComment );

            if ( CodeDomProvider.Supports ( GeneratorSupport.NestedTypes ) )
            {
                var lazyType = Declare.NestedClass ( fieldName ).Private ( ).Static ( )
                                      .AddTo       ( support );

                cctor.AddTo ( lazyType );

                Declare.Field      ( type, SingletonFieldName ).Internal ( ).Static ( )
                       .Initialize ( initializer )
                       .AddTo      ( lazyType );

                return Code.Type   ( fieldName ).Local ( )
                           .Static ( )
                           .Field  ( SingletonFieldName );
            }
            else
            {
                Declare.Field      ( type, fieldName ).Private ( ).Static ( )
                       .Initialize ( initializer )
                       .AddTo      ( support );

                if ( ! support.Members.OfType < CodeTypeConstructor > ( ).Any ( ) )
                    support.Members.Add ( cctor );

                return Code.Static ( ).Field ( fieldName );
            }
        }

        protected virtual CodeMemberProperty GenerateProperty ( string propertyName, IResource resource )
        {
            var resourceType = resource.Type != null ? Code.Type ( resource.Type ).Local ( ) : Code.Type < object > ( );
            var summary      = resource.Type == typeof ( string ).FullName ?
                               Format ( StringPropertySummary,    GeneratePreview ( (string) resource.Value ) ) :
                               Format ( NonStringPropertySummary, resource.Name );

            return Declare.Property   ( resourceType, propertyName )
                          .Modifiers  ( settings.AccessModifiers )
                          .Get        ( get => get.Return ( GenerateResourceGetter ( resource ) ) )
                          .AddSummary ( summary + FormatResourceComment ( resource.Comment ) );
        }

        protected virtual CodeExpression GenerateResourceGetter ( IResource resource )
        {
            var culture = Code.Instance ( settings.AccessModifiers ).Field ( CultureInfoFieldName );

            if ( settings.LocalizerType != null )
            {
                var localizer = Code.Static ( ).Property ( LocalizerPropertyName );

                if ( resource.Type == typeof ( string ).FullName )
                    return localizer.Method ( "GetString" )
                                    .Invoke ( culture, Code.Constant ( resource.Name ) );

                return localizer.Method ( "GetObject" )
                                .Invoke ( culture, Code.Constant ( resource.Name ) )
                                .Cast   ( Code.Type ( resource.Type ).Local ( ) );
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
                                      .Cast   ( Code.Type ( resource.Type ).Local ( ) );
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

            var objectType   = Code.Type < object > ( );
            var summary      = Format ( FormatMethodSummary, GeneratePreview ( (string) resource.Value ) );
            var formatMethod = Declare.Method < string > ( methodName, settings.AccessModifiers )
                                      .AddSummary        ( summary + FormatResourceComment ( resource.Comment ) )
                                      .AddReturnComment  ( FormatReturnComment );

            var format           = Code.Type < string > ( ).Static ( ).Method ( nameof ( string.Format ) );
            var formatExpression = (CodeExpression) Code.Instance ( settings.AccessModifiers ).Property ( propertyName );

            if ( localizer != null )
            {
                format           = localizer.Method ( nameof ( ILocalizer.Format ) );
                formatExpression = Code.Constant ( resource.Name );
            }

            if ( numberOfArguments > 3 )
                formatMethod.Attributed ( Declare.Attribute < SuppressMessageAttribute > ( "Microsoft.Design", "CA1025:ReplaceRepetitiveArgumentsWithParamsArray" ) );

            var initialArguments = localizer != null ? 3 : 2;

            var parameters = new CodeExpression [ initialArguments + numberOfArguments ];

            parameters [ 0 ] = Code.Instance ( settings.AccessModifiers ).Field ( CultureInfoFieldName );

            if ( localizer != null )
            {
                parameters [ 1 ] = parameters [ 0 ];
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

            return formatMethod.Define ( method => method.Return ( format.Invoke ( parameters ) ) );
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

        protected virtual IList < Entry > ValidateResourceNames ( CodeTypeDeclaration support )
        {
            var classProperties = new HashSet < string > ( );
            foreach ( CodeTypeMember member in support.Members )
                classProperties.Add ( member.Name );

            var entries = new SortedList < string, Entry > ( ResourceSet.Count, StringComparer.InvariantCultureIgnoreCase );

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
    }

    public class Entry
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