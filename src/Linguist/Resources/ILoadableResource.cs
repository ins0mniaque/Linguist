namespace Linguist.Resources
{
    public delegate object ResourceLoader ( ILoadableResource resource );

    public interface ILoadableResource : IResource
    {
        object Data { get; }
    }
}