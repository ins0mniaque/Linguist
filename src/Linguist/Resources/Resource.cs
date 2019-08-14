namespace Linguist.Resources
{
    public class Resource : IResource
    {
        public string Name    { get; set; }
        public string Type    { get; set; }
        public string Comment { get; set; }
        public string Source  { get; set; }
        public int?   Line    { get; set; }
        public int?   Column  { get; set; }

        private object value;

        public virtual object Value
        {
            get { return value; }
            set
            {
                if ( Type == null )
                    Type = value?.GetType ( ).FullName;

                this.value = value;
            }
        }
    }
}