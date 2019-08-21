namespace Linguist.Resources
{
    public interface IResource
    {
        string   Name    { get; }
        TypeName Type    { get; }
        object   Value   { get; }
        string   Comment { get; }
        string   Source  { get; }
        int?     Line    { get; }
        int?     Column  { get; }
    }
}