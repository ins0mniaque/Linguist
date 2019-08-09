using System;
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
                case "System.Drawing" : return Assembly.Load    ( "System.Drawing.Common" )
                                                      ?.GetType ( name, false, ignore );
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

        public class Binder : SerializationBinder
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
    }
}