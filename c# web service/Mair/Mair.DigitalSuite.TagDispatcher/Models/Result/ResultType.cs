using System;
using System.Collections.Generic;
using System.Text;

namespace Mair.DigitalSuite.TagDispatcher.Models.Result
{
    public enum ResultType
    {
        Error = 0,
        Created = 1,
        Updated = 2,
        Deleted = 3,
        NotFound = 4,
        Found = 5,
        Failed = 6
    }
}
