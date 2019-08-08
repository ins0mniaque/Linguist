namespace Linguist
{
    public interface ILocalizerProvider
    {
        ILocalizer Load ( string path );
    }
}