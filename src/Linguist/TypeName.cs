using System;

namespace Linguist
{
    [ Serializable ]
    public sealed class TypeName
    {
        public TypeName ( Type type )
        {
            if ( type == null )
                throw new ArgumentNullException ( nameof ( type ) );

            Type     = type.FullName;
            Assembly = ValidateAssemblyName ( type.Assembly.GetName ( ).Name );
        }

        public static implicit operator TypeName ( Type type ) => new TypeName ( type );

        public TypeName ( string assemblyQualifiedName )
        {
            Parse ( assemblyQualifiedName ?? throw new ArgumentNullException ( nameof ( assemblyQualifiedName ) ),
                    out var type,
                    out var assembly );

            Type     = type ?? throw new ArgumentException ( $"'{ assemblyQualifiedName }' is not a valid type name.", nameof ( assemblyQualifiedName ) );
            Assembly = assembly;
        }

        public static bool TryParse ( string assemblyQualifiedName, out TypeName typeName )
        {
            var parsed = Parse ( assemblyQualifiedName ?? throw new ArgumentNullException ( nameof ( assemblyQualifiedName ) ),
                                 out var type,
                                 out var assembly );

            typeName = parsed ? new TypeName ( type, assembly ) : null;

            return parsed;
        }

        [ NonSerialized ] private string assemblyQualifiedName;

        private TypeName ( string type, string assembly )
        {
            Type     = type;
            Assembly = assembly;
        }

        public string Type     { get; }
        public string Assembly { get; }

        public TypeName WithAssembly ( string assembly )
        {
            return new TypeName ( Type, ValidateAssemblyName ( assembly ) );
        }

        public override bool Equals ( object value )
        {
            var other = value as TypeName;
            if ( object.ReferenceEquals ( other, null ) ) return false;
            if ( object.ReferenceEquals ( this, other ) ) return true;

            return Assembly == other.Assembly &&
                   Type     == other.Type;
        }

        public static bool operator == ( TypeName left, TypeName right )
        {
            if ( object.ReferenceEquals ( left,  right ) ) return true;
            if ( object.ReferenceEquals ( left,  null  ) ) return false;
            if ( object.ReferenceEquals ( right, null  ) ) return false;

            return left.Assembly == right.Assembly &&
                   left.Type     == right.Type;
        }

        public static bool operator != ( TypeName left, TypeName right )
        {
            return ! ( left == right );
        }

        public override int GetHashCode ( ) => ToString ( ).GetHashCode ( );

        public override string ToString ( )
        {
            if ( assemblyQualifiedName == null )
            {
                assemblyQualifiedName = Type;
                if ( Assembly != null )
                    assemblyQualifiedName += ", " + Assembly;
            }

            return assemblyQualifiedName;
        }

        private static string ValidateAssemblyName ( string assembly )
        {
            assembly = assembly?.Split ( ',' ) [ 0 ]
                                .Trim  ( );

            switch ( assembly )
            {
                case ""                       :
                case "mscorlib"               :
                case "netstandard"            :
                case "System.Private.CoreLib" :
                case "System.Runtime"         : return null;
                default                       : return assembly;
            }
        }

        private static string ValidateTypeName ( string type )
        {
            type = type?.Trim ( );
            if ( string.IsNullOrEmpty ( type ) )
                return null;

            return type;
        }

        private static bool Parse ( string assemblyQualifiedName, out string type, out string assembly )
        {
            var comma    = -1;
            var brackets =  0;

            for ( var index = 0; index < assemblyQualifiedName.Length; index++ )
            {
                var character = assemblyQualifiedName [ index ];

                if      ( character == '[' ) brackets++;
                else if ( character == ']' ) brackets--;
                else if ( brackets == 0 && character == ',' )
                {
                    comma = index;
                    break;
                }
            }

            if ( comma >= 0 )
            {
                type     = assemblyQualifiedName.Substring ( 0, comma );
                assembly = assemblyQualifiedName.Substring ( comma + 1 );
            }
            else
            {
                type     = assemblyQualifiedName;
                assembly = null;
            }

            type     = ValidateTypeName     ( type     );
            assembly = ValidateAssemblyName ( assembly );

            return type != null;
        }
    }
}