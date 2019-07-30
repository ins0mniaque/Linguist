using System.Collections.Generic;
using System.IO;

namespace Linguist.CodeDom.Extensions
{
    public abstract class TextRewriterProxy : TextWriterProxy
    {
        public const string Wildcard = "🃏";

        private readonly List < string > buffer = new List < string > ( );
        private readonly string [ ] [ ]  paths;
        private readonly int    [ ]      indices;

        public TextRewriterProxy ( string [ ] [ ] searchPaths, TextWriter textWriter ) : base ( textWriter )
        {
            paths   = searchPaths;
            indices = new int [ paths.Length ];

            WriteBufferAndReset ( );
        }

        protected abstract void OnPathFound ( string [ ] path, IList < string > buffer );

        private void WriteBufferAndReset ( )
        {
            foreach ( var entry in buffer )
                writer.Write ( entry );

            buffer.Clear ( );

            for ( var index = 0; index < paths.Length; index++ )
                if ( indices [ index ] != int.MaxValue )
                    indices [ index ] = -1;
        }

        protected override void BeforeWrite ( )
        {
            WriteBufferAndReset ( );
        }

        public override void Write ( string value )
        {
            var buffered = false;

            for ( var definition = 0; definition < paths.Length; definition++ )
            {
                var path  = paths   [ definition ];
                var index = indices [ definition ];

                if ( index == int.MaxValue )
                    continue;

                var next = path [ index + 1 ];

                if ( next == Wildcard || next == value )
                {
                    indices [ definition ] = ++index;

                    if ( index < path.Length - 1 )
                    {
                        buffered = true;
                        continue;
                    }

                    indices [ definition ] = int.MaxValue;

                    OnPathFound ( path, buffer );

                    buffered = false;
                    break;
                }
            }

            if ( buffered )
            {
                buffer.Add ( value );
                return;
            }

            WriteBufferAndReset ( );

            writer.Write ( value );
        }
    }
}