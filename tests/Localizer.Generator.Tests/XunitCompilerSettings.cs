#if NET452

using System;
using System.IO;
using System.Reflection;

using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;

using Localizer.CodeDom;

namespace Localizer.Generator.Tests
{
    public sealed class XunitCompilerSettings : ICompilerSettings
    {
        public XunitCompilerSettings ( Language language )
        {
            var localPath = new Uri ( Assembly.GetCallingAssembly ( ).CodeBase ).LocalPath;
            var compiler  = language == Language.CSharp      ? @"..\roslyn\csc.exe" :
                            language == Language.VisualBasic ? @"..\roslyn\vbc.exe" :
                            throw new ArgumentException ( "Unsupported compiler language", nameof ( language ) );

            CompilerFullPath         = Path.Combine ( localPath, compiler );
            CompilerServerTimeToLive = 60 * 15;
        }
        
        public string CompilerFullPath         { get; }
        public int    CompilerServerTimeToLive { get; }
    }
}

#endif