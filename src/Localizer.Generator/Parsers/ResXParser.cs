using System;
using System.Collections;
using System.Collections.Generic;
using System.CodeDom;
using System.IO;
using System.Reflection;
using System.Resources;

using Localizer.CodeDom;

namespace Localizer.Generator
{
    public static class ResXParser
    {
        public static ResourceSet ExtractResourceSet ( string inputFileName, string inputFileContent )
        {
            const AssemblyName [ ] DefaultAssemblies = null;

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
                    var node     = (ResXDataNode) entry.Value;
                    var name     = (string)       entry.Key;
                    var typeName = node.GetValueTypeName ( DefaultAssemblies );
                    var type     = Type.GetType ( typeName );
                    var position = node.GetNodePosition  ( );
                    var comment  = node.Comment;

                    while ( ! type.IsPublic )
                        type = type.BaseType;

                    typeName = type.FullName;

                    var typeRef  = Code.TypeRef ( type );
                    var resource = (Resource) null;

                    if ( type == typeof ( string ) )
                        resource = new StringResource ( typeRef, (string) node.GetValue ( DefaultAssemblies ), GetString, comment );
                    else if ( type == typeof ( MemoryStream ) || type == typeof ( UnmanagedMemoryStream ) )
                        resource = new Resource ( typeRef, GetStream, comment );
                    else
                        resource = new Resource ( typeRef, GetObject ( typeRef ), comment );

                    resource.FileName = inputFileName;
                    resource.Line     = position.Y;
                    resource.Column   = position.X;

                    resources.Add ( name, resource );
                }
            }

            return new ResourceSet ( Code.TypeRef < ResourceManager > ( ), Initializer, ResourceSetGetter, resources );
        }

        private static CodeExpression Initializer ( string resourcesBaseName, string className )
        {
            return Code.TypeRef < ResourceManager > ( )
                       .Construct ( Code.Constant ( resourcesBaseName ),
                                    Code.TypeRef  ( className, default )
                                        .TypeOf   ( )
                                        .Property ( nameof ( Type.Assembly ) ) );
        }

        private static CodeExpression ResourceSetGetter ( CodeExpression resourceManager, CodeExpression culture )
        {
            return resourceManager.Method ( nameof ( ResourceManager.GetResourceSet ) )
                                  .Invoke ( culture, Code.Constant ( true ), Code.Constant ( true ) );
        }

        private static CodeExpression GetString ( CodeExpression resourceManager, string name, CodeExpression culture )
        {
            return resourceManager.Method ( nameof ( ResourceManager.GetString ) )
                                  .Invoke ( Code.Constant ( name ), culture );
        }

        private static ResourceGetter GetObject ( CodeTypeReference type )
        {
            return (resourceManager, name, culture) =>
                   resourceManager.Method ( nameof ( ResourceManager.GetObject ) )
                                  .Invoke ( Code.Constant ( name ), culture )
                                  .Cast   ( type );
        }

        private static CodeExpression GetStream ( CodeExpression resourceManager, string name, CodeExpression culture )
        {
            return resourceManager.Method ( nameof ( ResourceManager.GetStream ) )
                                  .Invoke ( Code.Constant ( name ), culture );
        }
    }
}