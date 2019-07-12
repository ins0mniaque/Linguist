﻿using System.CodeDom.Compiler;

namespace Localizer.CodeDom
{
    public enum Language { CSharp, Unknown }

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
                default:
                    return Language.Unknown;
            }
        }
    }
}