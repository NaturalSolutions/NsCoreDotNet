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
        Consumed = 8,
        Sent = 16,
        StoredExternally = 32,
        Exit = 64,
        Deleted = 1024,
        NoEdit = 2048,
        NoDelete = 4096
    }

    // TODO, I DONT REALLY KNOW WHY, WHEN I TRY TO REFERENCE THIS NEW ENUM, NOTHING WILL COMPILE !!!
    // SO ITS PRETTY BAD ACTUALLY BUT ILL PUT IT ALL ON THE "STATUS" ABOVE, TEMPORARILY ...

    public enum TypeStatus
    {
        Validated = 2,
        NoEdit = 4,
        NoDelete = 8,
        Deleted = 1024
    }


}


