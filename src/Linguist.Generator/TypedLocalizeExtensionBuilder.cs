using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

using Linguist.CodeDom.Fluent;

namespace Linguist.Generator
{
    using static Comments;
    using static MemberNames;
    using static String;

    public static class TypedLocalizeExtensionBuilder
    {
        public static CodeTypeDeclaration WPF ( string className, MemberAttributes memberAttributes, IList < ResourceMapping > map )
        {
            return GenerateTypedLocalizeExtension ( className,
                                                    memberAttributes,
                                                    map,
                                                    "Linguist.WPF.TypedLocalizeExtension",
                                                    true,
                                                    "System.Windows.Data.BindingBase",
                                                    Declare.Attribute < TypeConverterAttribute > ( Code.TypeOf ( Code.NestedType ( "Linguist.WPF.BindingSyntax", "TypeConverter" ) ) ) );
        }

        public static CodeTypeDeclaration XamarinForms ( string className, MemberAttributes memberAttributes, IList < ResourceMapping > map )
        {
            return GenerateTypedLocalizeExtension ( className,
                                                    memberAttributes,
                                                    map,
                                                    "Linguist.Xamarin.Forms.TypedLocalizeExtension",
                                                    false,
                                                    "Xamarin.Forms.BindingBase",
                                                    Declare.Attribute ( Code.Type ( "Xamarin.Forms.TypeConverterAttribute" ),
                                                                        Code.TypeOf ( Code.NestedType ( "Linguist.Xamarin.Forms.BindingSyntax", "TypeConverter" ) ) ) );
        }

        private static CodeTypeDeclaration GenerateTypedLocalizeExtension ( string className, MemberAttributes memberAttributes, IList < ResourceMapping > map, string extensionType, bool generateConstructors, string bindingType, CodeAttributeDeclaration bindingTypeConverter )
        {
            var type        = Declare.Class      ( className + "Extension" )
                                     .Modifiers  ( memberAttributes );
            var keyEnum     = Declare.NestedEnum ( ResourceKeyEnumName )
                                     .Modifiers  ( memberAttributes )
                                     .AddSummary ( ResourceKeyEnumNameSummary )
                                     .AddTo      ( type );
            var keyEnumType = Code.Type ( ResourceKeyEnumName ).Local ( );
            var objectType  = Code.Type < object > ( );

            type.BaseTypes.Add ( Code.Type ( extensionType, Code.NestedType ( type.Name, ResourceKeyEnumName ).Local ( ) ) );

            if ( generateConstructors )
            {
                var distinctNumberOfArguments = map.Select ( mapping => mapping.NumberOfArguments )
                                                   .DefaultIfEmpty ( 0 )
                                                   .Distinct ( )
                                                   .OrderBy  ( numberOfArguments => numberOfArguments );

                foreach ( var numberOfArguments in distinctNumberOfArguments )
                {
                    var ctor = new CodeConstructor ( ) { Attributes = memberAttributes }.AddTo ( type );

                    for ( var argument = 0; argument < numberOfArguments; argument++ )
                    {
                        var parameterName = Format ( CultureInfo.InvariantCulture, FormatMethodParameterName, argument );

                        ctor.Parameters         .Add ( objectType.Parameter ( parameterName ) );
                        ctor.BaseConstructorArgs.Add ( Code.Variable        ( parameterName ) );
                    }
                }
            }

            var keyPathType = Code.Type ( bindingType );

            var _key     = Code.This ( ).Field ( "_key" );
            var _keyPath = Code.This ( ).Field ( "_keyPath" );
            var _type    = Code.This ( ).Field ( "_type" );

            Declare.Field    ( keyEnumType, _key.FieldName ).AddTo ( type );
            Declare.Property ( keyEnumType, "Key"  ).Public ( ).Override ( )
                   .Get   ( get          => get.Return ( _key ) )
                   .Set   ( (set, value) => set.Add    ( Code.Assign ( _key, value ) ) )
                   .AddTo ( type );


            Declare.Field    ( keyPathType, _keyPath.FieldName ).AddTo ( type );
            Declare.Property ( keyPathType, "KeyPath"  ).Public ( ).Override ( )
                   .Get        ( get          => get.Return ( _keyPath ) )
                   .Set        ( (set, value) => set.Add    ( Code.Assign ( _keyPath, value ) ) )
                   .Attributed ( bindingTypeConverter )
                   .AddTo      ( type );

            Declare.Field    < Type > ( _type.FieldName ).AddTo ( type );
            Declare.Property < Type > ( "Type"  ).Public ( ).Override ( )
                   .Get   ( get          => get.Return ( _type ) )
                   .Set   ( (set, value) => set.Add    ( Code.Assign ( _type, value ) ) )
                   .AddTo ( type );

            Declare.Property ( Code.Type < ILocalizer > ( ), "Localizer" ).Protected ( ).Override ( )
                   .Get      ( get => get.Return ( Code.Type ( className ).Local ( ).Static ( ).Property ( LocalizerPropertyName ) ) )
                   .AddTo    ( type );

            var translation = new CodeArrayCreateExpression ( Code.Type < string > ( ) );
            var index       = 0;

            foreach ( var mapping in map )
            {
                Declare.Field      ( keyEnumType, mapping.Property ).Const ( )
                       .Modifiers  ( memberAttributes )
                       .Initialize ( Code.Constant ( index++ ) )
                       .AddSummary ( ResourceKeyFieldSummaryFormat, mapping.Resource.Name )
                       .AddTo      ( keyEnum );

                translation.Initializers.Add ( Code.Constant ( mapping.Resource.Name ) );
            }

            var translator = Declare.Field < string [ ] > ( ResourceKeyTranslatorFieldName ).Static ( )
                                    .Initialize ( translation )
                                    .AddTo ( type );

            var translate = Declare.Method < string > ( "KeyToName",
                                                        MemberAttributes.Family | MemberAttributes.Override )
                                   .AddTo ( type );

            var key   = Code.Variable ( ResourceKeyParameterName );
            var first = keyEnumType.Static ( ).Field ( map [ 0 ].Property );
            var last  = keyEnumType.Static ( ).Field ( map [ index - 1 ].Property );

            translate.Parameters.Add    ( keyEnumType.Parameter ( ResourceKeyParameterName ) );
            translate.Statements.Add    ( Code.If   ( key.IsLessThan    ( first ).Or (
                                                      key.IsGreaterThan ( last  ) ) )
                                              .Then ( Code.Throw < ArgumentOutOfRangeException > ( Code.Constant ( ResourceKeyParameterName ) ) ) );
            translate.Statements.Return ( Code.Static  ( )
                                              .Field   ( ResourceKeyTranslatorFieldName )
                                              .Indexer ( Code.Variable ( ResourceKeyParameterName ).Cast < int > ( ) ) );

            return type;
        }
    }
}