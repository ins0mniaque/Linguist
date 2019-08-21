using System;

using Xunit;

namespace Linguist.Tests
{
    public class TypeNameTests
    {
        [ Fact ]
        public static void ThrowsOnNull ( )
        {
            Assert.Throws < ArgumentNullException > ( ( ) => new TypeName ( (Type)   null ) );
            Assert.Throws < ArgumentNullException > ( ( ) => new TypeName ( (string) null ) );
        }

        [ Theory ]
        [ InlineData ( ""    ) ]
        [ InlineData ( " "   ) ]
        [ InlineData ( "   " ) ]
        [ InlineData ( ","   ) ]
        [ InlineData ( ", "  ) ]
        [ InlineData ( " ,"  ) ]
        [ InlineData ( " , " ) ]
        public static void ThrowsOnInvalidTypeName ( string typeName )
        {
            Assert.Throws < ArgumentException > ( ( ) => new TypeName ( typeName ) );
        }

        [ Fact ]
        public static void CanBeImplicitlyCastFromType ( )
        {
            TypeName type = typeof ( string );

            Assert.Equal ( "System.String", type.ToString ( ) );
        }

        [ Fact ]
        public static void CanBeCreatedFromType ( )
        {
            var type = new TypeName ( typeof ( string ) );

            Assert.Equal ( "System.String", type.ToString ( ) );
        }

        [ Theory ]
        [ InlineData ( "System.String", null,                                  "System.String" ) ]
        [ InlineData ( "System.String", "mscorlib",                            "System.String" ) ]
        [ InlineData ( "System.String", "netstandard",                         "System.String" ) ]
        [ InlineData ( "System.String", "System.Runtime",                      "System.String" ) ]
        [ InlineData ( "System.Drawing.Bitmap", "System.Drawing",              "System.Drawing.Bitmap, System.Drawing" ) ]
        [ InlineData ( "System.Drawing.Icon", "System.Drawing",                "System.Drawing.Icon, System.Drawing" ) ]
        [ InlineData ( "System.Resources.ResXFileRef", "System.Windows.Forms", "System.Resources.ResXFileRef, System.Windows.Forms" ) ]
        [ InlineData ( "System.Byte[]", "",                                    "System.Byte[]" ) ]
        [ InlineData ( "System.Byte[]", "mscorlib",                            "System.Byte[]" ) ]
        [ InlineData ( "System.Byte[,]", "netstandard",                        "System.Byte[,]" ) ]
        [ InlineData ( "System.Byte[,,]", "System.Runtime",                    "System.Byte[,,]" ) ]
        [ InlineData ( "System.Drawing.Bitmap[,]", "System.Drawing",           "System.Drawing.Bitmap[,], System.Drawing" ) ]
        public static void HasCorrectStringRepresentation ( string type, string assembly, string expectedTypeName )
        {
            Assert.Equal ( expectedTypeName, new TypeName ( type ).WithAssembly ( assembly ).ToString ( ) );
        }

        [ Theory ]
        [ InlineData ( "System.String",                                                                                "System.String" ) ]
        [ InlineData ( "System.String, mscorlib",                                                                      "System.String" ) ]
        [ InlineData ( "System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",   "System.String" ) ]
        [ InlineData ( "System.Byte[], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",   "System.Byte[]" ) ]
        [ InlineData ( "System.Byte[,], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",  "System.Byte[,]" ) ]
        [ InlineData ( "System.Byte[,,], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Byte[,,]" ) ]
        public static void ConstructsFromTypesCorrectly ( string typeName, string expectedTypeName )
        {
            var type = Type.GetType ( typeName );

            Assert.NotNull ( type );
            Assert.Equal   ( expectedTypeName, new TypeName ( type ).ToString ( ) );
        }

        [ Theory ]
        [ InlineData ( "System.String",                                                                                               "System.String" ) ]
        [ InlineData ( "System.String, mscorlib",                                                                                     "System.String" ) ]
        [ InlineData ( "System.String, netstandard",                                                                                  "System.String" ) ]
        [ InlineData ( "System.String, System.Runtime",                                                                               "System.String" ) ]
        [ InlineData ( "System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",                  "System.String" ) ]
        [ InlineData ( "System.String, netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51",               "System.String" ) ]
        [ InlineData ( "System.String, System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",            "System.String" ) ]
        [ InlineData ( "System.Drawing.Bitmap, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",    "System.Drawing.Bitmap, System.Drawing" ) ]
        [ InlineData ( "System.Drawing.Icon, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",      "System.Drawing.Icon, System.Drawing" ) ]
        [ InlineData ( "System.Resources.ResXFileRef, System.Windows.Forms",                                                          "System.Resources.ResXFileRef, System.Windows.Forms" ) ]
        [ InlineData ( "System.Byte[], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",                  "System.Byte[]" ) ]
        [ InlineData ( "System.Byte[,], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",                 "System.Byte[,]" ) ]
        [ InlineData ( "System.Byte[,,], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",                "System.Byte[,,]" ) ]
        [ InlineData ( "System.Drawing.Bitmap[,], System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Bitmap[,], System.Drawing" ) ]
        public static void ConstructsFromTypeNamesCorrectly ( string typeName, string expectedTypeName )
        {
            Assert.Equal ( expectedTypeName, new TypeName ( typeName ).ToString ( ) );
        }

        [ Theory ]
        [ InlineData ( "System.String",                                                                                               "System.String" ) ]
        [ InlineData ( "System.String, mscorlib",                                                                                     "System.String" ) ]
        [ InlineData ( "System.String, netstandard",                                                                                  "System.String" ) ]
        [ InlineData ( "System.String, System.Runtime",                                                                               "System.String" ) ]
        [ InlineData ( "System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",                  "System.String" ) ]
        [ InlineData ( "System.String, netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51",               "System.String" ) ]
        [ InlineData ( "System.String, System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",            "System.String" ) ]
        [ InlineData ( "System.Drawing.Bitmap, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",    "System.Drawing.Bitmap, System.Drawing" ) ]
        [ InlineData ( "System.Drawing.Icon, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",      "System.Drawing.Icon, System.Drawing" ) ]
        [ InlineData ( "System.Resources.ResXFileRef, System.Windows.Forms",                                                          "System.Resources.ResXFileRef, System.Windows.Forms" ) ]
        [ InlineData ( "System.Byte[], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",                  "System.Byte[]" ) ]
        [ InlineData ( "System.Byte[,], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",                 "System.Byte[,]" ) ]
        [ InlineData ( "System.Byte[,,], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",                "System.Byte[,,]" ) ]
        [ InlineData ( "System.Drawing.Bitmap[,], System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Bitmap[,], System.Drawing" ) ]
        public static void ParsesTypeNamesCorrectly ( string typeName, string expectedTypeName )
        {
            Assert.True  ( TypeName.TryParse ( typeName, out var type ) );
            Assert.Equal ( expectedTypeName, type.ToString ( ) );
        }

        [ Theory ]
        [ InlineData ( "System.String",                 "System.String" ) ]
        [ InlineData ( "System.String, mscorlib",       "System.String" ) ]
        [ InlineData ( "System.String, netstandard",    "System.String" ) ]
        [ InlineData ( "System.String, System.Runtime", "System.String" ) ]
        [ InlineData ( "System.Byte[]",                 "System.Byte[]" ) ]
        [ InlineData ( "System.Byte[,], netstandard",   "System.Byte[,]" ) ]
        [ InlineData ( "System.Byte[,,], mscorlib",     "System.Byte[,,]" ) ]
        public static void ImplementsEqualityCorrectly ( string leftTypeName, string rightTypeName )
        {
            var left  = new TypeName ( leftTypeName  );
            var right = new TypeName ( rightTypeName );

            Assert.Equal ( left, right );
            Assert.True  ( left.Equals ( right ) );
            Assert.True  ( left == right );
            Assert.False ( left != right );
        }

        [ Theory ]
        [ InlineData ( "System.String",                 "System.String, OtherAssembly" ) ]
        [ InlineData ( "System.String, mscorlib",       "System.String, OtherAssembly" ) ]
        [ InlineData ( "System.String, netstandard",    "System.String, OtherAssembly" ) ]
        [ InlineData ( "System.String, System.Runtime", "System.String, OtherAssembly" ) ]
        [ InlineData ( "System.Byte[]",                 "System.Byte[], OtherAssembly" ) ]
        [ InlineData ( "System.Byte[,], netstandard",   "System.Byte[,], OtherAssembly" ) ]
        [ InlineData ( "System.Byte[,,], mscorlib",     "System.Byte[,,], OtherAssembly" ) ]
        public static void ImplementsInequalityCorrectly ( string leftTypeName, string rightTypeName )
        {
            var left  = new TypeName ( leftTypeName  );
            var right = new TypeName ( rightTypeName );

            Assert.NotEqual ( left, right );
            Assert.False    ( left.Equals ( right ) );
            Assert.False    ( left == right );
            Assert.True     ( left != right );
        }
    }
}