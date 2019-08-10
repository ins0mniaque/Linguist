using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Linguist.Resources
{
    /// <summary>Represents a file based resource manager that provides convenient access to culture-specific resources at run time.</summary>
    public class FileBasedResourceManager < TResourceSet > : FileBasedResourceManager
    {
        /// <summary>
        /// Creates a new instance of <see cref="T:Linguist.Resources.FileBasedResourceManager" />.
        /// </summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="path">The root directory of the resource files.</param>
        /// <param name="pathFormat">The format of the resource files path using the {resource}, {culture} and {locale} placeholders.</param>
        /// <param name="neutralCultureName">The name of the neutral culture, or null to default to a file without a culture identifier.</param>
        /// <param name="usingResourceSet">The type of the resource set object that the resource manager uses to construct a <see cref="T:System.Resources.ResourceSet" /> object.</param>
        public FileBasedResourceManager ( string baseName, string path, string pathFormat, string neutralCultureName )
            : base ( baseName, path, pathFormat, neutralCultureName, typeof ( TResourceSet ) ) { }

        /// <summary>
        /// Creates a new instance of <see cref="T:Linguist.Resources.FileBasedResourceManager" />.
        /// </summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="pathFormat">The format of the resource files path using the {resource}, {culture} and {locale} placeholders.</param>
        /// <param name="neutralCultureName">The name of the neutral culture, or null to default to a file without a culture identifier.</param>
        /// <param name="usingResourceSet">The type of the resource set object that the resource manager uses to construct a <see cref="T:System.Resources.ResourceSet" /> object.</param>
        public FileBasedResourceManager ( string baseName, string pathFormat, string neutralCultureName )
            : base ( baseName, pathFormat, neutralCultureName, typeof ( TResourceSet ) ) { }

        /// <summary>
        /// Creates a new instance of <see cref="T:Linguist.Resources.FileBasedResourceManager" />.
        /// </summary>
        /// <param name="pathFormat">The format of the resource files path using the {culture} and {locale} placeholders.</param>
        /// <param name="neutralCultureName">The name of the neutral culture, or null to default to a file without a culture identifier.</param>
        /// <param name="usingResourceSet">The type of the resource set object that the resource manager uses to construct a <see cref="T:System.Resources.ResourceSet" /> object.</param>
        public FileBasedResourceManager ( string pathFormat, string neutralCultureName )
            : base ( pathFormat, neutralCultureName, typeof ( TResourceSet ) ) { }
    }

    /// <summary>Represents a file based resource manager that provides convenient access to culture-specific resources at run time.</summary>
    public class FileBasedResourceManager : ResourceManager
    {
        protected readonly Cache < CultureInfo, ResourceSet > cache = new Cache < CultureInfo, ResourceSet > ( );

        protected TemplateEngine PathTemplate { get; }

        #if NETSTANDARD2_0
        /// <summary>
        /// Specifies the root name of the resource files that the <see cref="T:Linguist.Resources.FileBasedResourceManager" /> searches for resources.
        /// </summary>
        public override string BaseName => BaseNameField;

        /// <summary>
        /// Specifies the root name of the resource files that the <see cref="T:Linguist.Resources.FileBasedResourceManager" /> searches for resources.
        /// </summary>
        protected string BaseNameField;
        #endif

        /// <summary>
        /// Specifies the format of the resource files using the {resource}, {culture} and {locale} placeholders.
        /// </summary>
        public string PathFormat => PathTemplate.Template;

        /// <summary>
        /// Specifies the name of the neutral culture.
        /// </summary>
        public string NeutralCultureName { get; }

        /// <summary>Gets the type of the resource set object that the resource manager uses to construct a <see cref="T:System.Resources.ResourceSet" /> object.</summary>
        /// <returns>The type of the resource set object that the resource manager uses to construct a <see cref="T:System.Resources.ResourceSet" /> object.</returns>
        public override Type ResourceSetType { get; }

        /// <summary>
        /// Creates a new instance of <see cref="T:Linguist.Resources.FileBasedResourceManager" />.
        /// </summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="path">The root directory of the resource files.</param>
        /// <param name="pathFormat">The format of the resource files path using the {resource}, {culture} and {locale} placeholders.</param>
        /// <param name="neutralCultureName">The name of the neutral culture, or null to default to a file without a culture identifier.</param>
        /// <param name="usingResourceSet">The type of the resource set object that the resource manager uses to construct a <see cref="T:System.Resources.ResourceSet" /> object.</param>
        public FileBasedResourceManager ( string baseName, string path, string pathFormat, string neutralCultureName, Type usingResourceSet = null )
            : this ( baseName, Path.Combine ( path, pathFormat ), neutralCultureName, usingResourceSet ) { }

        /// <summary>
        /// Creates a new instance of <see cref="T:Linguist.Resources.FileBasedResourceManager" />.
        /// </summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="pathFormat">The format of the resource files path using the {resource}, {culture} and {locale} placeholders.</param>
        /// <param name="neutralCultureName">The name of the neutral culture, or null to default to a file without a culture identifier.</param>
        /// <param name="usingResourceSet">The type of the resource set object that the resource manager uses to construct a <see cref="T:System.Resources.ResourceSet" /> object.</param>
        public FileBasedResourceManager ( string baseName, string pathFormat, string neutralCultureName, Type usingResourceSet = null )
            : this ( pathFormat, neutralCultureName, usingResourceSet )
        {
            PathTemplate [ "resource" ] = BaseNameField = baseName;
        }

        /// <summary>
        /// Creates a new instance of <see cref="T:Linguist.Resources.FileBasedResourceManager" />.
        /// </summary>
        /// <param name="pathFormat">The format of the resource files path using the {culture} and {locale} placeholders.</param>
        /// <param name="neutralCultureName">The name of the neutral culture, or null to default to a file without a culture identifier.</param>
        /// <param name="usingResourceSet">The type of the resource set object that the resource manager uses to construct a <see cref="T:System.Resources.ResourceSet" /> object.</param>
        public FileBasedResourceManager ( string pathFormat, string neutralCultureName, Type usingResourceSet = null )
        {
            PathTemplate       = new TemplateEngine ( pathFormat );
            NeutralCultureName = ValidateNeutralCultureName ( neutralCultureName );
            ResourceSetType    = usingResourceSet;
        }

        protected override string GetResourceFileName ( CultureInfo culture )
        {
            if ( culture?.Name == CultureInfo.InvariantCulture.Name )
                culture = null;

            var cultureName = culture?.Name ?? NeutralCultureName ?? string.Empty;

            PathTemplate [ "culture" ] = cultureName;
            PathTemplate [ "locale"  ] = cultureName.Replace ( '-', '_' );

            var path = PathTemplate.ToString ( );

            if ( string.IsNullOrEmpty ( cultureName ) )
                return RemoveNeutralCultureSeparator ( path );

            return path;
        }

        protected override ResourceSet InternalGetResourceSet ( CultureInfo culture, bool createIfNotExists, bool tryParents )
        {
            if ( culture == null )
                throw new ArgumentNullException ( nameof ( culture ) );

            if ( cache.TryGet ( culture, out var resourceSet ) )
                return resourceSet;

            if ( ! createIfNotExists )
                return null;

            var fallback = culture;
            var fileName = FindResourceFile ( fallback );

            if ( fileName == null )
            {
                if ( ! tryParents )
                    return null;

                fallback = culture.Parent;
                if ( fallback != culture )
                    fileName = FindResourceFile ( fallback );
            }

            if ( fileName == null )
            {
                fallback = CultureInfo.InvariantCulture;
                if ( fallback != culture )
                    fileName = FindResourceFile ( fallback );
            }

            if ( fileName == null )
                throw new MissingManifestResourceException ( "Could not find resource file " + GetResourceFileName ( culture ) );

            resourceSet = LoadResourceSet ( fileName );

            cache.Add ( culture, resourceSet );
            if ( fallback != culture )
                cache.Add ( fallback, resourceSet );

            return resourceSet;
        }

        protected virtual ResourceSet LoadResourceSet ( string resourceFileName )
        {
            return (ResourceSet) Activator.CreateInstance ( ResourceSetType,
                                                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance,
                                                            null,
                                                            new object [ ] { resourceFileName },
                                                            null );
        }

        private string rootPath;

        private string FindResourceFile ( CultureInfo culture )
        {
            var resourceFileName = GetResourceFileName ( culture );

            if ( rootPath != null )
            {
                var path = Path.Combine ( rootPath, resourceFileName );
                if ( File.Exists ( path ) )
                    return path;
            }

            if ( File.Exists ( resourceFileName ) )
                return resourceFileName;

            if ( Path.IsPathRooted ( resourceFileName ) )
                return null;

            var entryPath = GetAssemblyPath ( Assembly.GetEntryAssembly ( ), resourceFileName );
            if ( entryPath != null && File.Exists ( entryPath ) )
            {
                rootPath = Path.GetDirectoryName ( Assembly.GetEntryAssembly ( ).Location );

                return entryPath;
            }

            var executingPath = GetAssemblyPath ( Assembly.GetExecutingAssembly ( ), resourceFileName );
            if ( executingPath != null && executingPath != entryPath && File.Exists ( executingPath ) )
            {
                rootPath = Path.GetDirectoryName ( Assembly.GetExecutingAssembly ( ).Location );

                return executingPath;
            }

            return null;
        }

        private static string GetAssemblyPath ( Assembly assembly, string path )
        {
            if ( ! string.IsNullOrEmpty ( assembly?.Location ) )
                return Path.Combine ( Path.GetDirectoryName ( assembly.Location ), path );

            return null;
        }

        private static string RemoveNeutralCultureSeparator ( string path )
        {
            path = path.Replace ( "//",   "/"  )
                       .Replace ( "\\\\", "\\" );

            var fileName = Path.GetFileNameWithoutExtension ( path );
            var trimmed  = fileName.Trim ( '-', '.', '_' );

            if ( fileName != trimmed )
            {
                var fileNameIndex = path.LastIndexOf ( fileName );

                path = path.Remove ( fileNameIndex, fileName.Length )
                           .Insert ( fileNameIndex, trimmed );
            }

            return path;
        }

        private static readonly char [ ] PathFormatTemplateCharacters        = new [ ] { '{', '}' };
        private const           string   PathFormatInNeutralCultureNameError = "Path format passed as neutral culture name argument. Make sure to specify the neutralCultureName, or null to default to a file without a culture identifier.";

        private static string ValidateNeutralCultureName ( string neutralCultureName )
        {
            if ( neutralCultureName?.IndexOfAny ( PathFormatTemplateCharacters ) >= 0 )
                throw new ArgumentException ( PathFormatInNeutralCultureNameError, nameof ( neutralCultureName ) );

            return neutralCultureName;
        }
    }
}