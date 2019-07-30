namespace Linguist.CLDR
{
    public interface IRuleSet
    {
        string     Name    { get; }
        string [ ] Locales { get; }
    }
}