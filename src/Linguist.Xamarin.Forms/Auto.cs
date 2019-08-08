using System;
using System.Collections.Generic;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Linguist.Xamarin.Forms
{
    internal static class Auto
    {
        public static string GenerateKey ( IServiceProvider serviceProvider, out ILocalizer localizer )
        {
            var pvt = (IProvideValueTarget) serviceProvider.GetService ( typeof ( IProvideValueTarget ) );

            var target   = pvt.TargetObject   as BindableObject;
            var property = pvt.TargetProperty as BindableProperty;
            var ancestry = GetParentObjects ( pvt );

            localizer = GetLocalizer ( ancestry );

            var component = GetComponent ( ancestry, out var componentElement );

            if ( pvt.TargetObject == componentElement )
                return GenerateKey ( component, property.PropertyName );

            var name = GetName ( pvt ) ?? target.GetType ( ).Name;

            return GenerateKey ( component, name, property.PropertyName );
        }

        private static string GenerateKey ( params string [ ] parts )
        {
            return string.Join ( ".", Array.FindAll ( parts, part => ! string.IsNullOrEmpty ( part ) ) );
        }

        public static ILocalizer GetLocalizer ( IServiceProvider serviceProvider )
        {
            var pvt = (IProvideValueTarget) serviceProvider.GetService ( typeof ( IProvideValueTarget ) );

            return GetLocalizer ( GetParentObjects ( pvt ) );
        }

        private static ILocalizer GetLocalizer ( IEnumerable<object> ancestry )
        {
            foreach ( var ancestor in ancestry )
            {
                if ( ! ( ancestor is BindableObject element ) )
                    continue;

                var localizer = Localize.GetLocalizer ( element );
                if ( localizer != null )
                    return localizer;
            }

            return null;
        }

        private static string GetComponent ( IEnumerable < object > ancestry, out BindableObject element )
        {
            foreach ( var ancestor in ancestry )
            {
                element = ancestor as BindableObject;
                if ( element == null )
                    continue;

                var component = Localize.GetComponent ( element );
                if ( component != null )
                    return component;

                var isComponent = GetFilePathForObject ( element ) != null;
                if ( isComponent )
                {
                    Localize.SetComponent ( element, component = element.GetType ( ).Name );
                    return component;
                }
            }

            element = null;
            return null;
        }

        private static string GetName ( IProvideValueTarget pvt )
        {
            if ( pvt.TargetObject is BindableObject element )
            {
                var name = Localize.GetName ( element );
                if ( name != null )
                    return name;
            }

            return GetNameScope ( pvt )?.GetName ( pvt.TargetObject );
        }

        private static PropertyInfo ParentObjectsProperty;

        private static IEnumerable < object > GetParentObjects ( IProvideValueTarget pvt )
        {
            if ( ParentObjectsProperty == null )
                ParentObjectsProperty = pvt.GetType ( )
                                           .GetInterface ( "Xamarin.Forms.Xaml.IProvideParentValues" )
                                           .GetProperty  ( "ParentObjects" ) ??
                                        throw new ArgumentException ( nameof ( pvt ), "Could not get parent objects property" );

            return (IEnumerable < object >) ParentObjectsProperty.GetValue ( pvt, null );
        }

        private static FieldInfo NameScopeField;

        private static NameScope GetNameScope ( IProvideValueTarget pvt )
        {
            if ( NameScopeField == null )
                NameScopeField = pvt.GetType ( ).GetField ( "scope", BindingFlags.NonPublic | BindingFlags.Instance ) ??
                                 throw new ArgumentException ( nameof ( pvt ), "Could not get namescope" );

            return NameScopeField.GetValue ( pvt ) as NameScope;
        }

        private static FieldInfo NameScopeNamesField;

        private static string GetName ( this NameScope scope, object element )
        {
            if ( NameScopeNamesField == null )
                NameScopeNamesField = typeof ( NameScope ).GetField ( "_names", BindingFlags.NonPublic | BindingFlags.Instance ) ??
                                      throw new ArgumentException ( nameof ( scope ), "Could not get namescope dictionary" );

            var names = (IDictionary < string, object >) NameScopeNamesField.GetValue ( scope );

            foreach ( var entry in names )
                if ( entry.Value == element )
                    return entry.Key;

            return null;
        }

        private static string GetFilePathForObject ( object view )
        {
            var xaml = view.GetType ( ).GetCustomAttributes ( typeof ( XamlFilePathAttribute ), false );
            if ( xaml.Length > 0 )
                return ( (XamlFilePathAttribute) xaml [ 0 ] ).FilePath;

            return null;
        }
    }
}