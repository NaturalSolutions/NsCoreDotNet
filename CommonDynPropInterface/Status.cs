using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDynPropInterface
{
    public enum Status
    {
        Pending = 1,
        Validated = 2,
        Scrapped = 4,
        Consummed = 8,
        Sent = 16,
        StoredExternally = 32,
        Exit = 64
    }




}


