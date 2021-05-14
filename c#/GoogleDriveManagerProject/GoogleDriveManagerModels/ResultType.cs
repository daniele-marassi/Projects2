using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleDriveManagerModels
{
    public enum ResultType
    {
        Error = 0,
        FoundWithError = 1,
        NotFound = 2,
        Found = 3,
        None = 4,
        Executed = 5
    }
}
