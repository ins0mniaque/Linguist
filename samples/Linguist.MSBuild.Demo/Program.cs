using System;

namespace Linguist.MSBuild.Demo
{
    class Program
    {
        static void Main ( string [ ] args )
        {
            Console.WriteLine ( Resources.HelloWorld );

            Console.WriteLine ( $"Image size: { Resources.Image.Size }" );
        }
    }
}