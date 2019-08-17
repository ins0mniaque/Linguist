using Linguist.Resources;

namespace Linguist.Generator
{
    public class ResourceMapping
    {
        public ResourceMapping ( IResource resource )
        {
            Resource = resource;
        }

        public IResource Resource          { get; }
        public string    Property          { get; set; }
        public string    FormatMethod      { get; set; }
        public int       NumberOfArguments { get; set; }
    }
}