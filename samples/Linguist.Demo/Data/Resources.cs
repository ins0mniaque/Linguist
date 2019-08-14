﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Linguist.Demo.Data {
    using System;
    
    
    /// <summary>
    /// A strongly-typed resource class, for looking up and formatting localized strings, etc.
    /// </summary>
    /// <remarks>
    /// This class was auto-generated by the LinguistSupportBuilder class via the
    /// InternalLinguistCodeGenerator custom tool.
    /// To add or remove a member, edit your .ResX file then rerun the InternalLinguistCodeGenerator custom
    /// tool or rebuild your VS project.
    /// </remarks>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Linguist.Generator.LinguistSupportBuilder", "0.9.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Reflection.ObfuscationAttribute(Exclude=true, ApplyToMembers=true)]
    [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    internal static partial class Resources {
        
        private static global::System.Globalization.CultureInfo _culture;
        
        /// <summary>
        /// Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                return _resourceManager.Value;
            }
        }
        
        /// <summary>
        /// Overrides the current thread's CurrentUICulture property for all resource lookups using this
        /// strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return _culture;
            }
            set {
                if (object.Equals(_culture, value)) {
                    return;
                }
                _culture = value;
                NotifyCultureChanged();
            }
        }
        
        /// <summary>
        /// Returns the cached Localizer instance used by this class.
        /// </summary>
        internal static global::Linguist.Resources.ResourceManagerLocalizer Localizer {
            get {
                return _localizer.Value;
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Can use any formattable object as input for number :'.
        /// </summary>
        internal static string AnyInputDemo {
            get {
                return ResourceManager.GetString("AnyInputDemo", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Can read binary data on .NET Core 2.0 (Using
        /// BinaryResourceSet) :'.
        /// </summary>
        internal static string BinaryDataDemo {
            get {
                return ResourceManager.GetString("BinaryDataDemo", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Can read System.Drawing image objects as System.Drawing.
        /// Common objects on .NET Core 2.0 (Using BinaryResourceSet) :'.
        /// </summary>
        internal static string BinaryImageDemo {
            get {
                return ResourceManager.GetString("BinaryImageDemo", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'There is a lot more to Linguist!
        /// 
        /// See https://github.com/ins0mniaque/Linguist for information on NuGet packages and updates,
        /// and check out the Linguist extension on the Marketplace!
        /// 
        /// https://marketplace.visualstudio.com/items?itemName=ins0mniaque.linguist'.
        /// </summary>
        internal static string Conclusion {
            get {
                return ResourceManager.GetString("Conclusion", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a resource 'File'.
        /// </summary>
        internal static byte[] File {
            get {
                return ((byte[])(ResourceManager.GetObject("File", _culture)));
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to '{0} files found'.
        /// </summary>
        internal static string FilesFound {
            get {
                return ResourceManager.GetString("FilesFound", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to '{0} files not found'.
        /// </summary>
        internal static string FilesNotFound {
            get {
                return ResourceManager.GetString("FilesNotFound", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a resource 'Icon'.
        /// </summary>
        internal static global::System.Drawing.Icon Icon {
            get {
                return ((global::System.Drawing.Icon)(ResourceManager.GetObject("Icon", _culture)));
            }
        }
        
        /// <summary>
        /// Looks up a resource 'Image'.
        /// </summary>
        internal static global::System.Drawing.Bitmap Image {
            get {
                return ((global::System.Drawing.Bitmap)(ResourceManager.GetObject("Image", _culture)));
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Linguist adds support for pluralization rules defined by the
        /// Unicode CLDR Project (http://cldr.unicode.org) by adding additional plural forms entries to resource
        /// files.
        /// 
        /// The default naming strategy reads a .PluralForm suffix at the end of resources, where PluralForm can
        /// be one of these :
        /// 
        /// - Zero
        /// - One
        /// - Two
        /// - Few
        /// - Many
        /// - Other
        /// - 0 (For explicit zero)
        /// - 1 (For explicit one)
        /// - Range (Special form to specify the start of a range)
        /// 
        /// See https://unicode.org/reports/tr35/tr35-n [rest of string was truncated]&quot;;'.
        /// </summary>
        internal static string Introduction {
            get {
                return ResourceManager.GetString("Introduction", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to '{0} meters'.
        /// </summary>
        internal static string Meters {
            get {
                return ResourceManager.GetString("Meters", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to '{0}–{1} meters'.
        /// </summary>
        internal static string MetersRange {
            get {
                return ResourceManager.GetString("MetersRange", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Support for multiple plurals :
        /// 
        /// NOTE: Not recommended with current naming strategy; translators needs to add (Number of
        /// arguments)^(Number of plurals - 1) entries, and some locales have up to 6 number of plurals (see
        /// Resources.resx).'.
        /// </summary>
        internal static string MultiplePluralsDemo {
            get {
                return ResourceManager.GetString("MultiplePluralsDemo", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Formats using the default plural rule with non-numbers input
        /// :'.
        /// </summary>
        internal static string NonNumberDemo {
            get {
                return ResourceManager.GetString("NonNumberDemo", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Automatic pluralization with support for integer and decimal
        /// rules :'.
        /// </summary>
        internal static string PluralizationDemo {
            get {
                return ResourceManager.GetString("PluralizationDemo", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Supports plural range rules :'.
        /// </summary>
        internal static string PluralRangeDemo {
            get {
                return ResourceManager.GetString("PluralRangeDemo", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to '{0} files found; {1} files not found'.
        /// </summary>
        internal static string SearchResult {
            get {
                return ResourceManager.GetString("SearchResult", _culture);
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Welcome to Linguist!'.
        /// </summary>
        internal static string Welcome {
            get {
                return ResourceManager.GetString("Welcome", _culture);
            }
        }
        
        public static event global::System.ComponentModel.PropertyChangedEventHandler StaticPropertyChanged;
        
        internal static void NotifyCultureChanged() {
            global::System.ComponentModel.PropertyChangedEventHandler notify = StaticPropertyChanged;
            if ((notify == null)) {
                return;
            }
            notify(null, new global::System.ComponentModel.PropertyChangedEventArgs(null));
        }
        
        /// <summary>
        /// Formats a localized string similar to '{0} files found'.
        /// </summary>
        /// <param name="arg0">The object to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string representation of the
        /// corresponding object arguments.
        /// </returns>
        internal static string FilesFoundFormat(object arg0) {
            return _localizer.Value.Format(_culture, _culture, "FilesFound", arg0);
        }
        
        /// <summary>
        /// Formats a localized string similar to '{0} files not found'.
        /// </summary>
        /// <param name="arg0">The object to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string representation of the
        /// corresponding object arguments.
        /// </returns>
        internal static string FilesNotFoundFormat(object arg0) {
            return _localizer.Value.Format(_culture, _culture, "FilesNotFound", arg0);
        }
        
        /// <summary>
        /// Formats a localized string similar to '{0} meters'.
        /// </summary>
        /// <param name="arg0">The object to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string representation of the
        /// corresponding object arguments.
        /// </returns>
        internal static string MetersFormat(object arg0) {
            return _localizer.Value.Format(_culture, _culture, "Meters", arg0);
        }
        
        /// <summary>
        /// Formats a localized string similar to '{0}–{1} meters'.
        /// </summary>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string representation of the
        /// corresponding object arguments.
        /// </returns>
        internal static string MetersRangeFormat(object arg0, object arg1) {
            return _localizer.Value.Format(_culture, _culture, "MetersRange", arg0, arg1);
        }
        
        /// <summary>
        /// Formats a localized string similar to '{0} files found; {1} files not found'.
        /// </summary>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string representation of the
        /// corresponding object arguments.
        /// </returns>
        internal static string SearchResultFormat(object arg0, object arg1) {
            return _localizer.Value.Format(_culture, _culture, "SearchResult", arg0, arg1);
        }
        
        private static class _resourceManager {
            
            internal static global::System.Resources.ResourceManager Value = new global::System.Resources.ResourceManager("Linguist.Demo.Data.Resources", typeof(Resources).Assembly, typeof(global::Linguist.Resources.Binary.BinaryResourceSet));
            
            // Explicit static constructor to tell compiler not to mark type as beforefieldinit
            static _resourceManager() {
            }
        }
        
        private static class _localizer {
            
            internal static global::Linguist.Resources.ResourceManagerLocalizer Value = new global::Linguist.Resources.ResourceManagerLocalizer(_resourceManager.Value);
            
            // Explicit static constructor to tell compiler not to mark type as beforefieldinit
            static _localizer() {
            }
        }
    }
}