using System.CodeDom.Compiler;

namespace Linguist.CodeDom
{
    public enum Language { CSharp, VisualBasic, Unknown }

    public static class LanguageExtensions
    {
        public static Language GetLanguage ( this CodeDomProvider codeDomProvider )
        {
            switch ( CodeDomProvider.GetLanguageFromExtension ( codeDomProvider.FileExtension ).ToLowerInvariant ( ) )
            {
                case "c#":
                case "cs":
                case "csharp":
                    return Language.CSharp;
                case "vb":
                case "visualbasic":
                    return Language.VisualBasic;
                default:
                    return Language.Unknown;
            }
        }
    }
}