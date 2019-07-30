using System;
using System.Diagnostics;

namespace Linguist
{
    [ DebuggerTypeProxy ( typeof ( CacheDebugView < , > ) ) ]
    [ DebuggerDisplay   ( "Size = { Size ( ) }, LastUsedEntry = { lastUsedEntry.Key }" ) ]
    public partial class Cache < TKey, TValue >
    {
        internal class DebugView
        {
            private readonly Cache < TKey, TValue > cache;

            public DebugView ( Cache < TKey, TValue > cache )
            {
                this.cache = cache ?? throw new ArgumentNullException ( nameof ( cache ) );
            }

            [ DebuggerBrowsable ( DebuggerBrowsableState.RootHidden ) ]
            public Entry [ ] Entries
            {
                get
                {
                    cache.cacheLock.EnterReadLock ( );

                    try
                    {
                        var entries = new Entry [ cache.cache.Count ];
                        var index   = 0;

                        foreach ( var entry in cache.cache )
                            entries [ index++ ] = new Entry ( entry.Key, entry.Value );

                        return entries;
                    }
                    finally
                    {
                        cache.cacheLock.ExitReadLock ( );
                    }
                }
            }

            [ DebuggerDisplay ( "{value}", Name = "{key}" ) ]
            internal sealed class Entry
            {
                private object key;
                private object value;

                public Entry ( object key, object value )
                {
                    this.key   = key;
                    this.value = value;
                }
            }
        }
    }

    internal sealed class CacheDebugView < K, V > : Cache < K, V >.DebugView
    {
        public CacheDebugView ( Cache < K, V > cache ) : base ( cache ) { }
    }
}