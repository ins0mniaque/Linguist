using System.Collections.Generic;
using System.Threading;

namespace Linguist
{
    /// <summary>
    /// Fast thread-safe cache based on <see cref="T:System.Collections.Generic.Dictionary" /> and
    /// <see cref="T:System.Threading.ReaderWriterLockSlim" />, with lock-free last used entry retrieval.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the cache.</typeparam>
    /// <typeparam name="TValue">The type of the values in the cache.</typeparam>
    public sealed partial class Cache < TKey, TValue >
    {
        private readonly ReaderWriterLockSlim          cacheLock = new ReaderWriterLockSlim ( );
        private readonly IEqualityComparer < TKey >    comparer;
        private readonly Dictionary   < TKey, TValue > cache;
        private          KeyValuePair < TKey, TValue > lastUsedEntry;

        public Cache ( ) : this ( 0, null ) { }

        public Cache ( int capacity ) : this ( capacity, null ) { }

        public Cache ( IEqualityComparer < TKey > comparer ) : this ( 0, comparer ) { }

        public Cache ( int capacity, IEqualityComparer < TKey > comparer )
        {
            this.comparer = comparer ?? EqualityComparer < TKey >.Default;
            this.cache    = new Dictionary < TKey, TValue > ( capacity, this.comparer );
        }

        public void Add ( TKey key, TValue value )
        {
            lastUsedEntry = new KeyValuePair < TKey, TValue > ( key, value );

            cacheLock.EnterWriteLock ( );
            try     { cache [ key ] = value; }
            finally { cacheLock.ExitWriteLock ( ); }
        }

        public TValue Get ( TKey key )
        {
            var lastUsed = lastUsedEntry;
            if ( comparer.Equals ( key, lastUsed.Key ) )
                return lastUsed.Value;

            cacheLock.EnterReadLock ( );
            try     { return cache [ key ]; }
            finally { cacheLock.ExitReadLock ( ); }
        }

        public bool TryGet ( TKey key, out TValue value )
        {
            var lastUsed = lastUsedEntry;
            if ( comparer.Equals ( key, lastUsed.Key ) )
            {
                value = lastUsed.Value;
                return true;
            }

            cacheLock.EnterReadLock ( );
            try     { return cache.TryGetValue ( key, out value ); }
            finally { cacheLock.ExitReadLock ( ); }
        }

        public void Remove ( TKey key )
        {
            if ( comparer.Equals ( key, lastUsedEntry.Key ) )
                lastUsedEntry = default;

            cacheLock.EnterWriteLock ( );
            try     { cache.Remove ( key ); }
            finally { cacheLock.ExitWriteLock ( ); }
        }

        public void Clear ( )
        {
            lastUsedEntry = default;

            cacheLock.EnterWriteLock ( );
            try     { cache.Clear ( ); }
            finally { cacheLock.ExitWriteLock ( ); }
        }

        public bool Contains ( TKey key )
        {
            if ( comparer.Equals ( key, lastUsedEntry.Key ) )
                return true;

            cacheLock.EnterReadLock ( );
            try     { return cache.ContainsKey ( key ); }
            finally { cacheLock.ExitReadLock ( ); }
        }

        public int Size ( )
        {
            cacheLock.EnterReadLock ( );
            try     { return cache.Count; }
            finally { cacheLock.ExitReadLock ( ); }
        }
    }
}