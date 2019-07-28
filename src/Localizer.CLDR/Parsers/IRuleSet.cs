namespace Localizer.CLDR
{
    public interface IRuleSet
    {
        string     Name    { get; }
        string [ ] Locales { get; }
    }
}