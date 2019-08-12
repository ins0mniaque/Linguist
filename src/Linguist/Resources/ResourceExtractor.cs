using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;

namespace Linguist.Resources
{
    public abstract class ResourceExtractor : IResourceExtractor
    {
        private Stream stream;
        private string fileName;

        protected ResourceExtractor ( Stream stream )
        {
            this.stream = stream ?? throw new ArgumentNullException ( nameof ( stream ) );
        }

        protected ResourceExtractor ( string fileName )
        {
            this.fileName = fileName ?? throw new ArgumentNullException ( nameof ( fileName ) );
        }

        public IEnumerable < IResource > Extract ( )
        {
            if ( stream == null )
                stream = new FileStream ( fileName, FileMode.Open, FileAccess.Read, FileShare.Read );

            return Extract ( stream );
        }

        protected abstract IEnumerable < IResource > Extract ( Stream stream );

        public IEnumerable < DictionaryEntry > Read ( )
        {
            if ( stream == null )
                stream = new FileStream ( fileName, FileMode.Open, FileAccess.Read, FileShare.Read );

            return Read ( stream );
        }

        protected virtual IEnumerable < DictionaryEntry > Read ( Stream stream )
        {
            foreach ( var resource in Extract ( ) )
                yield return new DictionaryEntry ( resource.Name, resource.Value );
        }

        public static string GetStreamSource ( Stream stream )
        {
            if ( stream is FileStream file ) return file.Name;

            return stream?.GetType ( ).Name;
        }

        public void Close ( )
        {
            ( (IDisposable) this ).Dispose ( );
        }

        IDictionaryEnumerator IResourceReader.GetEnumerator ( ) => new DictionaryEnumerator ( Read ( ) );
        IEnumerator           IEnumerable    .GetEnumerator ( ) => new DictionaryEnumerator ( Read ( ) );

        protected virtual void Dispose ( bool disposing )
        {
            if ( disposing )
            {
                if ( fileName != null && stream != null )
                {
                    stream.Close ( );
                    stream = null;
                }
            }
        }

        void IDisposable.Dispose ( )
        {
            GC.SuppressFinalize ( this );
            Dispose ( true );
        }

        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator < DictionaryEntry > enumerator;

            public DictionaryEnumerator ( IEnumerable < DictionaryEntry > enumerable )
            {
                enumerator = enumerable.GetEnumerator ( );
            }

            public object          Key     => enumerator.Current.Key;
            public object          Value   => enumerator.Current.Value;
            public DictionaryEntry Entry   => enumerator.Current;
            public object          Current => enumerator.Current;

            public bool MoveNext ( ) => enumerator.MoveNext ( );
            public void Reset    ( ) => enumerator.Reset    ( );
        }
    }
}