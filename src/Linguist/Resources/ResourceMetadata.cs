namespace Linguist.Resources
{
    public class ResourceMetadata
    {
        public string Name    { get; set; }
        public string Type    { get; set; }
        public string Comment { get; set; }
        public string Source  { get; set; }
        public int?   Line    { get; set; }
        public int?   Column  { get; set; }
    }
}