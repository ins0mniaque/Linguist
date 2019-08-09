namespace Linguist.Resources
{
    public delegate object ResourceLoader ( IExternalResource resource );

    public class ExternalResource : ResourceMetadata, IExternalResource
    {
        private readonly ResourceLoader load;

        public ExternalResource ( ResourceLoader loader )
        {
            load = loader;
        }

        public object Reference { get; set; }
        public object Value     { get => Load ( ); }

        private bool   loaded;
        private object value;

        private object Load ( )
        {
            if ( ! loaded )
            {
                value  = load ( this );
                loaded = true;
            }

            return value;
        }
    }
}