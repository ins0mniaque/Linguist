using System;
using System.Windows;
using System.Windows.Media;

namespace Linguist.WPF
{
    internal static class Fallback
    {
        private static Brush       brush;
        private static ImageSource image;

        public static Color       Color { get; } =  CreateFallbackColor ( );
        public static Brush       Brush          => brush ?? ( brush = CreateFallbackBrush ( ) );
        public static ImageSource Image          => image ?? ( image = CreateFallbackImage ( ) );

        public static string String ( string name )
        {
            return $"[{ name }]";
        }

        public static object Object ( string name, Type targetType )
        {
            if ( typeof ( Color       ).IsAssignableFrom ( targetType ) ) return Color;
            if ( typeof ( Brush       ).IsAssignableFrom ( targetType ) ) return Brush;
            if ( typeof ( ImageSource ).IsAssignableFrom ( targetType ) ) return Image;

            return targetType.IsValueType ? Activator.CreateInstance ( targetType ) : null;
        }

        private static Color CreateFallbackColor ( )
        {
            return Color.FromArgb ( 127, Colors.Crimson.R, Colors.Crimson.G, Colors.Crimson.B );
        }

        private static Brush CreateFallbackBrush ( )
        {
            var line    = Geometry.Parse ( "M0,0 L16,16 M8,-8 L24,8 M-8,8 L8,24" );
            var pen     = new Pen ( Brushes.Crimson, 1 );
            var drawing = new GeometryDrawing ( null, pen, line );
            var brush   = new DrawingBrush ( drawing ) { TileMode = TileMode.Tile };

            brush.Viewport      = brush.Viewbox       = new Rect ( 0, 0, 16, 16 );
            brush.ViewportUnits = brush.ViewboxUnits  = BrushMappingMode.Absolute;

            brush.Freeze ( );

            return brush;
        }

        private static ImageSource CreateFallbackImage ( )
        {
            var cross   = Geometry.Parse ( "M0,0 L16,16 M0,16 L16,0" );
            var pen     = new Pen ( Brushes.Crimson, 2 );
            var drawing = new GeometryDrawing ( null, pen, cross );
            var image   = new DrawingImage ( drawing );

            image.Freeze ( );

            return image;
        }
    }
}