using System;
using System.IO;
using System.Text;

#if ! NET35
using System.Threading.Tasks;
#endif

namespace Linguist.CodeDom.Extensions
{
    /// <summary>
    /// Represents a wrapped <see cref="T:System.IO.TextWriter" /> that can override any write overload.
    /// This class is abstract.
    /// </summary>
    public abstract class TextWriterProxy : TextWriter
    {
        protected readonly TextWriter writer;

        public TextWriterProxy ( TextWriter textWriter )
        {
            writer = textWriter;
        }

        protected virtual void BeforeWrite ( ) { }

        private TextWriter TriggerBeforeWriteThen { get { BeforeWrite ( ); return writer; } }

        public override IFormatProvider FormatProvider => writer.FormatProvider;
        public override Encoding        Encoding       => writer.Encoding;
        public override string          NewLine
        {
            get => writer.NewLine;
            set => writer.NewLine = value;
        }

        public override void   Close        ( )                    => writer.Close ( );
        public override object InitializeLifetimeService ( )       => writer.InitializeLifetimeService ( );

        public override void Flush ( ) => writer.Flush ( );

        public override void Write ( char    value ) => TriggerBeforeWriteThen.Write ( value );
        public override void Write ( bool    value ) => TriggerBeforeWriteThen.Write ( value );
        public override void Write ( int     value ) => TriggerBeforeWriteThen.Write ( value );
        public override void Write ( uint    value ) => TriggerBeforeWriteThen.Write ( value );
        public override void Write ( long    value ) => TriggerBeforeWriteThen.Write ( value );
        public override void Write ( ulong   value ) => TriggerBeforeWriteThen.Write ( value );
        public override void Write ( float   value ) => TriggerBeforeWriteThen.Write ( value );
        public override void Write ( double  value ) => TriggerBeforeWriteThen.Write ( value );
        public override void Write ( decimal value ) => TriggerBeforeWriteThen.Write ( value );
        public override void Write ( object  value ) => TriggerBeforeWriteThen.Write ( value );
        public override void Write ( string  value ) => TriggerBeforeWriteThen.Write ( value );

        public override void Write ( char [ ] buffer )                                      => TriggerBeforeWriteThen.Write ( buffer );
        public override void Write ( char [ ] buffer, int index, int count )                => TriggerBeforeWriteThen.Write ( buffer, index, count );
        public override void Write ( string format, object arg0 )                           => TriggerBeforeWriteThen.Write ( format, arg0 );
        public override void Write ( string format, object arg0, object arg1 )              => TriggerBeforeWriteThen.Write ( format, arg0, arg1 );
        public override void Write ( string format, object arg0, object arg1, object arg2 ) => TriggerBeforeWriteThen.Write ( format, arg0, arg1, arg2 );
        public override void Write ( string format, params object [ ] arg )                 => TriggerBeforeWriteThen.Write ( format, arg );

        public override void WriteLine ( )               => TriggerBeforeWriteThen.WriteLine ( );
        public override void WriteLine ( char    value ) => TriggerBeforeWriteThen.WriteLine ( value );
        public override void WriteLine ( bool    value ) => TriggerBeforeWriteThen.WriteLine ( value );
        public override void WriteLine ( int     value ) => TriggerBeforeWriteThen.WriteLine ( value );
        public override void WriteLine ( uint    value ) => TriggerBeforeWriteThen.WriteLine ( value );
        public override void WriteLine ( long    value ) => TriggerBeforeWriteThen.WriteLine ( value );
        public override void WriteLine ( ulong   value ) => TriggerBeforeWriteThen.WriteLine ( value );
        public override void WriteLine ( float   value ) => TriggerBeforeWriteThen.WriteLine ( value );
        public override void WriteLine ( double  value ) => TriggerBeforeWriteThen.WriteLine ( value );
        public override void WriteLine ( decimal value ) => TriggerBeforeWriteThen.WriteLine ( value );
        public override void WriteLine ( string  value ) => TriggerBeforeWriteThen.WriteLine ( value );
        public override void WriteLine ( object  value ) => TriggerBeforeWriteThen.WriteLine ( value );

        public override void WriteLine ( char [ ] buffer )                                      => TriggerBeforeWriteThen.WriteLine ( buffer );
        public override void WriteLine ( char [ ] buffer, int index, int count )                => TriggerBeforeWriteThen.WriteLine ( buffer, index, count );
        public override void WriteLine ( string format, object arg0 )                           => TriggerBeforeWriteThen.WriteLine ( format, arg0 );
        public override void WriteLine ( string format, object arg0, object arg1 )              => TriggerBeforeWriteThen.WriteLine ( format, arg0, arg1 );
        public override void WriteLine ( string format, object arg0, object arg1, object arg2 ) => TriggerBeforeWriteThen.WriteLine ( format, arg0, arg1, arg2 );
        public override void WriteLine ( string format, params object [ ] arg )                 => TriggerBeforeWriteThen.WriteLine ( format, arg );

        #if ! NET35
        public override Task FlushAsync ( ) => writer.FlushAsync ( );

        public override Task WriteAsync ( char     value )                        => TriggerBeforeWriteThen.WriteAsync ( value );
        public override Task WriteAsync ( string   value )                        => TriggerBeforeWriteThen.WriteAsync ( value );
        public override Task WriteAsync ( char [ ] buffer, int index, int count ) => TriggerBeforeWriteThen.WriteAsync ( buffer, index, count );

        public override Task WriteLineAsync ( )                                       => TriggerBeforeWriteThen.WriteLineAsync ( );
        public override Task WriteLineAsync ( char     value )                        => TriggerBeforeWriteThen.WriteLineAsync ( value );
        public override Task WriteLineAsync ( string   value )                        => TriggerBeforeWriteThen.WriteLineAsync ( value );
        public override Task WriteLineAsync ( char [ ] buffer, int index, int count ) => TriggerBeforeWriteThen.WriteLineAsync ( buffer, index, count );
        #endif

        #if ! NETSTANDARD2_0
        public override System.Runtime.Remoting.ObjRef CreateObjRef ( Type requestedType ) => writer.CreateObjRef ( requestedType );
        #endif

        public override bool   Equals      ( object other ) => writer.Equals      ( other );
        public override int    GetHashCode ( )              => writer.GetHashCode ( );
        public override string ToString    ( )              => writer.ToString    ( );

        protected override void Dispose ( bool disposing )
        {
            base.Dispose ( disposing );

            writer.Dispose ( );
        }
    }
}