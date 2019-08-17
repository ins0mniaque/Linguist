using System;

namespace Linguist.Generator
{
    [ Flags ]
    public enum ResourceTypeOptions
    {
        None                = 0,
        CultureChangedEvent = 1 << 0
    }
}