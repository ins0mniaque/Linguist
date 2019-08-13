using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Linguist.Demo
{
    public static class Program
    {
        public static void Main ( string [ ] args )
        {
            Console.WriteLine ( Data.Resources.Welcome );
            Console.WriteLine ( );
            Console.WriteLine ( Data.Resources.Introduction );
            Console.WriteLine ( );

            Demo ( Data.Resources.PluralizationDemo,
                   ( ) => Data.Resources.FilesFoundFormat ( 0 ),
                   ( ) => Data.Resources.FilesFoundFormat ( 1 ),
                   ( ) => Data.Resources.FilesFoundFormat ( 42 ),
                   ( ) => Data.Resources.MetersFormat     ( 0.0 ),
                   ( ) => Data.Resources.MetersFormat     ( 0.5 ),
                   ( ) => Data.Resources.MetersFormat     ( 1.0 ),
                   ( ) => Data.Resources.MetersFormat     ( 1.5 ),
                   ( ) => Data.Resources.MetersFormat     ( 42.0 ) );

            Demo ( Data.Resources.PluralRangeDemo,
                   ( ) => Data.Resources.MetersRangeFormat ( 0, 1 ),
                   ( ) => Data.Resources.MetersRangeFormat ( 1, 2.5 ),
                   ( ) => Data.Resources.MetersRangeFormat ( 0, 5 ),
                   ( ) => Data.Resources.MetersRangeFormat ( 1, 5 ),
                   ( ) => Data.Resources.MetersRangeFormat ( 5, 20 ) );

            Demo ( Data.Resources.AnyInputDemo,
                   ( ) => Data.Resources.FilesFoundFormat ( "0" ),
                   ( ) => Data.Resources.FilesFoundFormat ( "1" ),
                   ( ) => Data.Resources.FilesFoundFormat ( "42" ),
                   ( ) => Data.Resources.MetersFormat     ( new System.Xml.Linq.XText ( "0" ) ),
                   ( ) => Data.Resources.MetersFormat     ( new System.Xml.Linq.XText ( "1" ) ),
                   ( ) => Data.Resources.MetersFormat     ( new System.Xml.Linq.XText ( "0.5" ) ),
                   ( ) => Data.Resources.MetersFormat     ( new System.Xml.Linq.XText ( "1.0" ) ),
                   ( ) => Data.Resources.MetersFormat     ( new System.Xml.Linq.XText ( "1.5" ) ) );

            Demo ( Data.Resources.NonNumberDemo,
                   ( ) => Data.Resources.FilesFoundFormat ( "Magic" ),
                   ( ) => Data.Resources.FilesFoundFormat ( typeof ( Program ) ) );

            Demo ( Data.Resources.MultiplePluralsDemo,
                   ( ) => Data.Resources.SearchResultFormat ( "5", 1D  ),
                   ( ) => Data.Resources.SearchResultFormat ( "5", 0E1 ),
                   ( ) => Data.Resources.SearchResultFormat ( "0", 5M  ),
                   ( ) => Data.Resources.SearchResultFormat ( "1",  0  ),
                   ( ) => Data.Resources.SearchResultFormat ( "5", 1E1 ),
                   ( ) => Data.Resources.SearchResultFormat ( "0", 0D  ) );

            Demo ( Data.Resources.BinaryDataDemo,
                   ( ) => System.Text.Encoding.UTF8.GetString ( Data.Resources.File ) );

            Demo ( Data.Resources.BinaryImageDemo,
                   ( ) => Data.Resources.Image.Size,
                   ( ) => Data.Resources.Icon.Size );

            Console.WriteLine ( );
            Console.WriteLine ( Data.Resources.Conclusion );
            Console.WriteLine ( );
        }

        private static void Demo ( string description, params Expression < Func < object > > [ ] demos )
        {
            Console.WriteLine ( description );
            Console.WriteLine ( );

            var alignment = demos.Select ( Print ).Max ( print => print.Length );
            foreach ( var demo in demos )
                Console.WriteLine ( string.Format ( "    {0,-" + alignment + "} => {1}", demo.Print ( ), demo.Compile ( ) ( ) ) );

            Console.WriteLine ( );
        }

        private static Regex RemoveCasts = new Regex ( @"Convert\((.*?),.*?\)", RegexOptions.Compiled );

        private static string Print ( this Expression < Func < object > > demo )
        {
            return RemoveCasts.Replace ( demo.Body.ToString ( ), "$1" );
        }
    }
}