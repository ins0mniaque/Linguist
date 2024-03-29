﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace Linguist
{
    public static class TypeResolver
    {
        private static readonly Cache < string, Type > types = new Cache < string, Type > ( );
        private static readonly Cache < Type, string > names = new Cache < Type, string > ( );

        public static Type ResolveType ( string typeName, bool ignoreCase = false )
        {
            #if ! NET35
            return Type.GetType ( typeName, ResolveAssembly, ResolveType, true, ignoreCase );
            #else
            return FindType ( typeName, ignoreCase ) ?? throw new TypeLoadException ( "Could not resolve type " + typeName );
            #endif
        }

        public static Type ResolveType ( TypeName typeName )
        {
            return ResolveType ( typeName.ToString ( ) );
        }

        public static string GetResolvedAssemblyQualifiedName ( Type type )
        {
            if ( names.TryGet ( type, out var name ) )
                return name;

            return null;
        }

        private static Assembly ResolveAssembly ( AssemblyName assemblyName )
        {
            return Assembly.Load ( assemblyName );
        }

        private static Type ResolveType ( Assembly assembly, string name, bool ignore )
        {
            if ( assembly == null )
                return FindType ( name, ignore );

            var type = assembly.GetType ( name, false, ignore );

            if ( type == null )
            {
                var assemblyQualifiedName = name + ", " + assembly.GetName ( ).FullName;

                if ( ! types.TryGet ( assemblyQualifiedName, out type ) )
                {
                    type = ResolveCompatibilityPackType ( assembly, name, ignore );

                    types.Add ( assemblyQualifiedName, type );
                    if ( type != null )
                        names.Add ( type, assemblyQualifiedName );
                }
            }

            return type;
        }

        private static Type ResolveCompatibilityPackType ( Assembly assembly, string name, bool ignore )
        {
            switch ( assembly.GetName ( ).Name )
            {
                case "System.Drawing" : return Type.GetType ( name + ", " + "System.Drawing.Common", false, ignore );
                default               : return null;
            }
        }

        private static Type FindType ( string name, bool ignore )
        {
            var type = Type.GetType ( name, false, ignore );
            if ( type != null )
                return type;

            foreach ( var assembly in AppDomain.CurrentDomain.GetAssemblies ( ) )
            {
                type = assembly.GetType ( name, false, ignore );
                if ( type != null )
                    return type;
            }

            return null;
        }

        private static SerializationBinder binder;
        public  static SerializationBinder Binder
        {
            get
            {
                if ( binder == null )
                    binder = new TypeBinder ( );

                return binder;
            }
        }

        private class TypeBinder : SerializationBinder
        {
            public override Type BindToType ( string assemblyName, string typeName )
            {
                return ResolveType ( typeName + ", " + assemblyName );
            }

            #if ! NET35
            public override void BindToName ( Type serializedType, out string assemblyName, out string typeName )
            {
                var assemblyQualifiedName = GetResolvedAssemblyQualifiedName ( serializedType );

                if ( ! string.IsNullOrEmpty ( assemblyQualifiedName ) )
                {
                    var comma = assemblyQualifiedName.IndexOf ( ',' );

                    if ( comma > 0 && comma < assemblyQualifiedName.Length - 1 )
                    {
                        assemblyName = assemblyQualifiedName.Substring ( comma + 1 ).TrimStart ( );
                        typeName     = assemblyQualifiedName.Substring ( 0, comma );

                        if ( typeName == serializedType.FullName )
                            typeName = null;

                        return;
                    }
                }

                base.BindToName ( serializedType, out assemblyName, out typeName );
            }
            #endif
        }

        private static bool              surrogateSelectorInitialized;
        private static SurrogateSelector surrogateSelector;
        public  static SurrogateSelector SurrogateSelector
        {
            get
            {
                if ( surrogateSelectorInitialized )
                    return surrogateSelector;

                surrogateSelectorInitialized = true;

                return surrogateSelector = GetSurrogateSelector ( );
            }
        }

        private static SurrogateSelector GetSurrogateSelector ( )
        {
            var bitmap = Type.GetType ( "System.Drawing.Bitmap, System.Drawing.Common" );
            if ( bitmap == null )
                return null;

            var icon = Type.GetType ( "System.Drawing.Icon, System.Drawing.Common" );

            var selector = new SurrogateSelector ( );
            var context  = new StreamingContext ( StreamingContextStates.All );

            selector.AddSurrogate ( bitmap, context, new BitmapSurrogate ( bitmap ) );
            selector.AddSurrogate ( icon,   context, new IconSurrogate   ( icon   ) );

            return selector;
        }

        private class BitmapSurrogate : ISerializationSurrogate
        {
            private readonly Type       type;
            private readonly MethodInfo save;

            public BitmapSurrogate ( Type bitmapType )
            {
                type = bitmapType;
                save = bitmapType.GetMethod ( "Save", new [ ] { typeof ( Stream ) } );
            }

            public void GetObjectData ( object bitmap, SerializationInfo info, StreamingContext context )
            {
                using ( var stream = new MemoryStream ( ) )
                {
                    save.Invoke ( bitmap, new object [ ] { stream } );

                    info.AddValue ( "Data", stream.ToArray ( ), typeof ( byte [ ] ) );
                }
            }

            public object SetObjectData ( object bitmap, SerializationInfo info, StreamingContext context, ISurrogateSelector selector )
            {
                var buffer = (byte [ ]) info.GetValue ( "Data", typeof ( byte [ ] ) );

                return Activator.CreateInstance ( type, new MemoryStream ( buffer ) );
            }
        }

        private class IconSurrogate : ISerializationSurrogate
        {
            private readonly Type         type;
            private readonly MethodInfo   save;
            private readonly PropertyInfo size;

            public IconSurrogate ( Type iconType )
            {
                type = iconType;
                save = iconType.GetMethod   ( "Save", new [ ] { typeof ( Stream ) } );
                size = iconType.GetProperty ( "Size" );
            }

            public void GetObjectData ( object icon, SerializationInfo info, StreamingContext context )
            {
                using ( var stream = new MemoryStream ( ) )
                {
                    save.Invoke ( icon, new object [ ] { stream } );

                    info.AddValue ( "IconData", stream.ToArray ( ), typeof ( byte [ ] ) );
                    info.AddValue ( "IconSize", size.GetValue ( icon, null ), typeof ( System.Drawing.Size ) );
                }
            }

            public object SetObjectData ( object icon, SerializationInfo info, StreamingContext context, ISurrogateSelector selector )
            {
                var buffer = (byte [ ]) info.GetValue ( "IconData", typeof ( byte [ ] ) );
                var size   =            info.GetValue ( "IconSize", typeof ( System.Drawing.Size ) );

                return Activator.CreateInstance ( type, new MemoryStream ( buffer ), size );
            }
        }
    }
}