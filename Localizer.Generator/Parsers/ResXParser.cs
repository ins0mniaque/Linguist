using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Localizer.Generator
{
    public static class ResXParser
    {
        public static readonly Type ResourceManagerType = typeof ( ResourceManager );

        public static IDictionary < string, Resource > ExtractResources ( string inputFileName, string inputFileContent )
        {
            var resources = new Dictionary < string, Resource > ( StringComparer.InvariantCultureIgnoreCase );

            using ( var reader = ResXResourceReader.FromFileContents ( inputFileContent ) )
            {
                if ( reader is ResXResourceReader resXReader )
                {
                    resXReader.UseResXDataNodes = true;
                    resXReader.BasePath = Path.GetFullPath ( Path.GetDirectoryName ( inputFileName ) );
                }

                foreach ( DictionaryEntry entry in reader )
                {
                    var node          = (ResXDataNode) entry.Value;
                    var name          = (string)       entry.Key;
                    var valueTypeName = node.GetValueTypeName ( (AssemblyName [ ]) null );
                    var valueType     = Type.GetType ( valueTypeName );
                    var position      = node.GetNodePosition  ( );
                    var comment       = node.Comment;

                    while ( ! valueType.IsPublic )
                        valueType = valueType.BaseType;

                    valueTypeName = valueType.FullName;

                    var resource = (Resource) null;

                    if ( valueType == typeof ( string ) )
                    {
                        var value = (string) node.GetValue ( (AssemblyName [ ]) null );

                        resource = new StringResource ( valueTypeName, value, nameof ( ResourceManager.GetString ), false, comment );
                    }
                    else if ( valueType == typeof ( MemoryStream ) || valueType == typeof ( UnmanagedMemoryStream ) )
                        resource = new Resource ( valueTypeName, nameof ( ResourceManager.GetStream ), false, comment );
                    else
                        resource = new Resource ( valueTypeName, nameof ( ResourceManager.GetObject ), true, comment );

                    resource.FileName = inputFileName;
                    resource.Line     = position.Y;
                    resource.Column   = position.X;

                    resources.Add ( name, resource );
                }
            }

            return resources;
        }
    }
}