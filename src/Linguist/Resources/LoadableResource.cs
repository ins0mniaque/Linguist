using System;

namespace Linguist.Resources
{
    public class LoadableResource : Resource, ILoadableResource
    {
        private readonly ResourceLoader load;

        public LoadableResource ( ResourceLoader loader )
        {
            load = loader ?? throw new ArgumentNullException ( nameof ( loader ) );
        }

        public object Data { get; set; }

        public override object Value
        {
            get => Load ( );
            set => base.Value = value;
        }

        private bool loaded;

        private object Load ( )
        {
            if   ( ! loaded )
            lock ( load )
            if   ( ! loaded )
            {
                base.Value = load ( this );
                loaded     = true;
            }

            return base.Value;
        }
    }
}