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

using Linguist.CodeDom;
using Linguist.CodeDom.Fluent;
using Linguist.Resources;

namespace Linguist.Generator
{
    using static String;
    using static Comments;
    using static MemberNames;

    public class ResourceTypeBuilder
    {
        public ResourceTypeBuilder ( CodeDomProvider codeDomProvider, IList < IResource > resourceSet, ResourceTypeSettings settings )
        {
            CodeDomProvider = codeDomProvider ?? throw new ArgumentNullException ( nameof ( codeDomProvider ) );
            ResourceSet     = resourceSet     ?? throw new ArgumentNullException ( nameof ( resourceSet     ) );
            Settings        = settings        ?? throw new ArgumentNullException ( nameof ( settings        ) );
        }

        public CodeDomProvider      CodeDomProvider { get; }
        public IList < IResource >  ResourceSet     { get; }
        public ResourceTypeSettings Settings        { get; }

        public virtual CompilerError [ ] GetErrors ( ) => mapper?.GetErrors ( );

        protected ResourceTypeSettings settings;
        protected ResourceMapper       mapper;

        public virtual CodeCompileUnit Build ( )
        {
            settings = Settings.Validate ( CodeDomProvider );

            var code       = GenerateCodeCompileUnit ( );
            var @namespace = code.Namespaces.Add ( settings.Namespace, "System" );
            var @class     = GenerateClass ( @namespace );

            GenerateClassMembers ( @class );

            mapper = new ResourceMapper ( CodeDomProvider, settings.ResourceNamingStrategy );

            var map = mapper.Map ( @class, ResourceSet );

            foreach ( var mapping in map )
                GenerateProperty ( mapping ).AddTo ( @class );

            foreach ( var mapping in map )
                if ( ! IsNullOrEmpty ( mapping.FormatMethod ) )
                    GenerateFormatMethod ( mapping ).AddTo ( @class );

            var extension = TypedLocalizeExtensionBuilder.Build ( settings.Extension,
                                                                  settings.ClassName,
                                                                  settings.AccessModifiers & ~MemberAttributes.Static,
                                                                  map );
            if ( extension != null )
                extension.AddTo ( @class );

            CodeGenerator.ValidateIdentifiers ( code );

            return code;
        }

        protected virtual CodeCompileUnit GenerateCodeCompileUnit ( )
        {
            var codeCompileUnit = new CodeCompileUnit ( );

            codeCompileUnit.ReferencedAssemblies.Add ( "System.dll" );

            codeCompileUnit.UserData.Add ( "AllowLateBound",             false );
            codeCompileUnit.UserData.Add ( "RequireVariableDeclaration", true  );

            return codeCompileUnit;
        }

        protected virtual CodeTypeDeclaration GenerateClass ( CodeNamespace @namespace )
        {
            var generator = typeof ( ResourceTypeBuilder );
            var version   = generator.Assembly.GetName ( ).Version;
            var type      = Declare.Class      ( settings.ClassName )
                                   .Modifiers  ( settings.AccessModifiers )
                                   .IsPartial  ( CodeDomProvider.Supports ( GeneratorSupport.PartialTypes ) )
                                   .AddSummary ( ClassSummary )
                                   .AddTo      ( @namespace );

            if ( settings.CustomToolType != null ) type.AddRemarks ( ClassRemarksFormat,         generator.FullName, settings.CustomToolType.Name );
            else                                   type.AddRemarks ( ClassRemarksToollessFormat, generator.FullName );

            if ( string.Equals ( @namespace.Name.Split ( '.' ).Last ( ), settings.ClassName, StringComparison.OrdinalIgnoreCase ) )
                type.Attributed ( Declare.Attribute < SuppressMessageAttribute > ( "Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces" ) );

            return type.Attributed ( Declare.Attribute < GeneratedCodeAttribute       > ( generator.FullName, version.ToString ( ) ),
                                     Declare.Attribute < DebuggerNonUserCodeAttribute > ( ),
                                     Declare.Attribute < ObfuscationAttribute         > ( )
                                            .WithArgument ( nameof ( ObfuscationAttribute.Exclude        ), true )
                                            .WithArgument ( nameof ( ObfuscationAttribute.ApplyToMembers ), true ) );
        }

        protected virtual void GenerateClassMembers ( CodeTypeDeclaration @class )
        {
            var editorBrowsable      = Declare.Attribute < EditorBrowsableAttribute > ( Code.Constant ( EditorBrowsableState.Advanced ) );
            var notifyCultureChanged = (CodeExpression) null;

            var ctor = Declare.Constructor ( )
                              .AddSummary  ( ConstructorSummaryFormat, settings.ClassName )
                              .Attributed  ( Declare.Attribute < SuppressMessageAttribute > ( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ) )
                              .AddTo       ( @class );

            if ( settings.AccessModifiers.HasBitMask ( MemberAttributes.Static ) )
                ctor.Private ( );
            else
                ctor.Modifiers ( settings.AccessModifiers );

            if ( ( settings.Options & ResourceTypeOptions.CultureChangedEvent ) == ResourceTypeOptions.CultureChangedEvent )
                GenerateCultureChangedEvent ( @class, out notifyCultureChanged );

            var resourceManager = GenerateSingleton ( @class,
                                                      settings.ResourceManagerType,
                                                      ResourceManagerFieldName,
                                                      settings.ResourceManagerInitializer );

            if ( settings.LocalizerType != null )
            {
                var localizer = GenerateSingleton ( @class,
                                                    settings.LocalizerType,
                                                    LocalizerFieldName,
                                                    settings.LocalizerInitializer );

                Declare.Property   ( settings.LocalizerType, LocalizerPropertyName ).Static ( )
                       .Modifiers  ( settings.AccessModifiers )
                       .Get        ( get => get.Return ( localizer ) )
                       .AddSummary ( LocalizerPropertySummary )
                       .AddTo      ( @class );
            }

            Declare.Property   ( settings.ResourceManagerType, ResourceManagerPropertyName ).Static ( )
                   .Modifiers  ( settings.AccessModifiers )
                   .Get        ( get => get.Return ( resourceManager ) )
                   .AddSummary ( ResourceManagerPropertySummary )
                   .Attributed ( editorBrowsable )
                   .AddTo      ( @class );

            var cultureField = Declare.Field < CultureInfo > ( CultureInfoFieldName )
                                      .Modifiers ( settings.AccessModifiers & MemberAttributes.Static )
                                      .AddTo ( @class );

            var field = @class.Instance ( ).Field ( CultureInfoFieldName );

            Declare.Property < CultureInfo > ( CultureInfoPropertyName )
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
                   .Attributed ( editorBrowsable )
                   .AddTo      ( @class );
        }

        protected virtual CodeMemberProperty GenerateProperty ( ResourceMapping mapping )
        {
            var resource     = mapping.Resource;
            var resourceType = resource.Type != null ? Code.Type ( resource.Type ).Local ( ) : Code.Type < object > ( );
            var summary      = resource.Type == typeof ( string ).FullName ?
                               Format ( StringPropertySummary,    GeneratePreview ( (string) resource.Value ) ) :
                               Format ( NonStringPropertySummary, resource.Name );

            return Declare.Property   ( resourceType, mapping.Property )
                          .Modifiers  ( settings.AccessModifiers )
                          .Get        ( get => get.Return ( GenerateResourceGetter ( resource ) ) )
                          .AddSummary ( summary + FormatResourceComment ( resource.Comment ) );
        }

        protected virtual CodeMemberMethod GenerateFormatMethod ( ResourceMapping mapping )
        {
            var resource          = mapping.Resource;
            var numberOfArguments = mapping.NumberOfArguments;
            if ( numberOfArguments <= 0 )
                throw new ArgumentOutOfRangeException ( nameof ( numberOfArguments ), numberOfArguments, "Number of argument must be greater than zero" );

            var localizer = (CodeExpression) null;
            if ( settings.LocalizerType != null )
                localizer = Code.Static ( ).Property ( LocalizerPropertyName );

            var format           = Code.Type < string > ( ).Static ( ).Method ( nameof ( string.Format ) );
            var formatExpression = (CodeExpression) Code.Instance ( settings.AccessModifiers ).Property ( mapping.Property );

            if ( localizer != null )
            {
                format           = localizer.Method ( nameof ( ILocalizer.Format ) );
                formatExpression = Code.Constant ( resource.Name );
            }

            var summary      = Format ( FormatMethodSummary, GeneratePreview ( (string) resource.Value ) );
            var formatMethod = Declare.Method < string > ( mapping.FormatMethod, settings.AccessModifiers )
                                      .AddSummary        ( summary + FormatResourceComment ( resource.Comment ) );

            var objectType = Code.Type < object > ( );
            var start      = localizer != null ? 3 : 2;
            var parameters = new CodeExpression [ start + numberOfArguments ];

            parameters [ 0 ] = Code.Instance ( settings.AccessModifiers ).Field ( CultureInfoFieldName );
            parameters [ 1 ] = formatExpression;
            if ( localizer != null )
            {
                parameters [ 2 ] = parameters [ 1 ];
                parameters [ 1 ] = parameters [ 0 ];
            }

            for ( var index = 0; index < numberOfArguments; index++ )
            {
                var parameterName = Format ( CultureInfo.InvariantCulture, FormatMethodParameterName, index );

                formatMethod.Parameters.Add ( objectType.Parameter ( parameterName ) );

                parameters [ start + index ] = Code.Variable ( parameterName );

                if ( numberOfArguments > 1 )
                    formatMethod.AddParameterComment ( parameterName, FormatMultiParameterComment, Ordinals [ Math.Min ( index, Ordinals.Length - 1 ) ] );
                else
                    formatMethod.AddParameterComment ( parameterName, FormatParameterComment, index );
            }

            if ( numberOfArguments > 3 )
                formatMethod.Attributed ( Declare.Attribute < SuppressMessageAttribute > ( "Microsoft.Design", "CA1025:ReplaceRepetitiveArgumentsWithParamsArray" ) );

            return formatMethod.Define ( method => method.Return ( format.Invoke ( parameters ) ) )
                               .AddReturnComment ( FormatReturnComment );
        }

        protected void GenerateCultureChangedEvent ( CodeTypeDeclaration @class, out CodeExpression notifyCultureChanged )
        {
            var propertyChangedEvent = Declare.Event < PropertyChangedEventHandler > ( nameof ( INotifyPropertyChanged.PropertyChanged ) )
                                              .AddTo ( @class );

            if ( ! settings.AccessModifiers.HasBitMask ( MemberAttributes.Static ) )
            {
                @class              .BaseTypes          .Add ( Code.Type < INotifyPropertyChanged > ( ) );
                propertyChangedEvent.ImplementationTypes.Add ( Code.Type < INotifyPropertyChanged > ( ) );
            }
            else
                propertyChangedEvent.Static ( ).Name = "Static" + propertyChangedEvent.Name;

            var propertyChanged = Code.Event ( @class.Instance ( ), propertyChangedEvent.Name );
            var notify          = Declare.Method ( NotifyCultureChangedMethodName, settings.AccessModifiers )
                                         .Define ( method =>
                                                   {
                                                       method.Add ( Declare.Variable < PropertyChangedEventHandler > ( NotifyCultureChangedVariableName )
                                                                           .Initialize ( propertyChanged ) );
                                                       method.Add ( Code.If   ( Code.Variable ( NotifyCultureChangedVariableName ).ValueEquals ( Code.Null ) )
                                                                        .Then ( Code.Return   ( ) ) );
                                                       method.Add ( Code.Variable ( NotifyCultureChangedVariableName )
                                                                        .InvokeDelegate ( @class.Instance ( ) ?? Code.Null,
                                                                                          Code.Type < PropertyChangedEventArgs > ( )
                                                                                              .Construct ( Code.Null ) ) );
                                                   } )
                                         .AddTo  ( @class );

            notifyCultureChanged = @class.Instance ( )
                                         .Method ( NotifyCultureChangedMethodName )
                                         .Invoke ( );
        }

        protected CodeExpression GenerateSingleton ( CodeTypeDeclaration @class, CodeTypeReference type, string fieldName, CodeExpression initializer )
        {
            var cctor = Declare.Constructor ( ).Static ( )
                               .AddComment  ( SingletonBeforeFieldInitComment );

            if ( CodeDomProvider.Supports ( GeneratorSupport.NestedTypes ) )
            {
                var lazyType = Declare.NestedClass ( fieldName ).Private ( ).Static ( )
                                      .AddTo       ( @class );

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
                       .AddTo      ( @class );

                if ( ! @class.Members.OfType < CodeTypeConstructor > ( ).Any ( ) )
                    @class.Members.Add ( cctor );

                return Code.Static ( ).Field ( fieldName );
            }
        }

        protected CodeExpression GenerateResourceGetter ( IResource resource )
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
    }
}