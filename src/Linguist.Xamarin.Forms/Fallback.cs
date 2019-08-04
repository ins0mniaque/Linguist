using System;

using Xamarin.Forms;

namespace Linguist.Xamarin.Forms
{
    internal static class Fallback
    {
        private static ImageSource image;

        public static Color       Color { get; } =  CreateFallbackColor ( );
        public static ImageSource Image          => image ?? ( image = CreateFallbackImage ( ) );

        public static string String ( string name )
        {
            return $"[{ name }]";
        }

        public static object Object ( string name, Type targetType )
        {
            if ( typeof ( Color       ).IsAssignableFrom ( targetType ) ) return Color;
            if ( typeof ( ImageSource ).IsAssignableFrom ( targetType ) ) return Image;

            return targetType.IsValueType ? Activator.CreateInstance ( targetType ) : null;
        }

        private static Color CreateFallbackColor ( )
        {
            return Color.FromRgba ( Color.Crimson.R, Color.Crimson.G, Color.Crimson.B, 127 );
        }

        private static ImageSource CreateFallbackImage ( )
        {
            return ImageSource.FromResource ( "Linguist.Xamarin.Forms.Resources.fallback.png", typeof ( Fallback ).Assembly );
        }
    }
}