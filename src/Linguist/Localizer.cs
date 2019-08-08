using System.Collections.Generic;

using Linguist.Resources;

namespace Linguist
{
    public static partial class Localizer
    {
        private static readonly Cache < string, ILocalizer > cache = new Cache < string, ILocalizer > ( );

        static Localizer ( )
        {
            Providers = new List < ILocalizerProvider > ( ) { new ResourceManagerLocalizerProvider ( ) };
        }

        public static IList < ILocalizerProvider > Providers { get; }

        public static ILocalizer Load ( string path )
        {
            if ( ! cache.TryGet ( path, out var localizer ) )
            {
                foreach ( var provider in Providers )
                {
                    localizer = provider.Load ( path );

                    if ( localizer != null )
                    {
                        cache.Add ( path, localizer );
                        break;
                    }
                }
            }

            return localizer;
        }

        public static void Load ( string path, ILocalizer localizer )
        {
            cache.Add ( path, localizer );
        }

        public static void Unload ( string path )
        {
            cache.Remove ( path );
        }
    }
}