using System;
using System.Collections.Generic;

namespace Mair.DigitalSuite.Dashboard.Models.Result
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