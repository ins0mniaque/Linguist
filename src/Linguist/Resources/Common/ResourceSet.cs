using System;
using System.Collections;
using System.Resources;

namespace Linguist.Resources.Common
{
    /// <summary>
    /// Stores all the resources localized for one particular culture,
    /// ignoring all other cultures, including any fallback rules.
    /// </summary>
    /// <remarks>
    /// Compatibility type to bridge .NET Framework/Standard ResourceSet implementation.
    /// </remarks>
    public class ResourceSet : System.Resources.ResourceSet
    {
        #if NETSTANDARD2_0
        /// <summary>Indicates the <see cref="T:System.Resources.IResourceReader" /> used to read the resources.</summary>
        [ NonSerialized ]
        protected IResourceReader Reader;

        /// <summary>The <see cref="T:System.Collections.Hashtable" /> in which the resources are stored.</summary>
        protected Hashtable Table;

        private   Hashtable caseInsensitiveTable;
        protected Hashtable CaseInsensitiveTable
        {
            get
            {
                if ( caseInsensitiveTable == null && Table != null )
                    caseInsensitiveTable = new Hashtable ( Table, StringComparer.OrdinalIgnoreCase );

                return caseInsensitiveTable;
            }
        }

        /// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> that can iterate through the <see cref="T:System.Resources.ResourceSet" />.</summary>
        /// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for this <see cref="T:System.Resources.ResourceSet" />.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The resource set has been closed or disposed.</exception>
        public override IDictionaryEnumerator GetEnumerator ( )
        {
            var table = Table;
            if ( table == null )
                throw new ObjectDisposedException ( "ResourceSet" );

            return table.GetEnumerator ( );
        }

        public override object GetObject ( string name ) => GetObject ( name, false );
        public override object GetObject ( string name, bool ignoreCase )
        {
            var table = ignoreCase ? CaseInsensitiveTable : Table;
            if ( table == null )
                throw new ObjectDisposedException ( "ResourceSet" );

            return table [ name ?? throw new ArgumentNullException ( nameof ( name ) ) ];
        }

        public override string GetString ( string name ) => GetString ( name, false );
        public override string GetString ( string name, bool ignoreCase )
        {
            var table = ignoreCase ? CaseInsensitiveTable : Table;
            if ( table == null )
                throw new ObjectDisposedException ( "ResourceSet" );

            try
            {
                return (string) table [ name ?? throw new ArgumentNullException ( nameof ( name ) ) ];
            }
            catch ( InvalidCastException exception )
            {
                throw new InvalidOperationException ( $"Resource { name } is not a string", exception );
            }
        }

        /// <summary>
        /// Reads all the resources and stores them in a <see cref="T:System.Collections.Hashtable" /> indicated
        /// in the <see cref="F:Linguist.Resources.ResourceSet.Table" /> property.
        /// </summary>
        protected override void ReadResources ( )
        {
            var enumerator = Reader.GetEnumerator ( );
            while ( enumerator.MoveNext ( ) )
                Table.Add ( enumerator.Key, enumerator.Value );
        }

        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
            {
                Reader?.Close ( );
                Reader = null;
            }

            Reader = null;
            Table  = null;
            caseInsensitiveTable = null;

            base.Dispose ( disposing );
        }
        #endif
    }
}