using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDynPropInterface
{
    public enum Status
    {
        ToBeValidated = 2,
        Validated = 4,
        Archived = 8,
        Deleted = 16,
        Sent = 32,
        Consumed = 64,
        Outside = 128
    }




}


