using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.ExecutionQueue.Models
{
    //
    // Riepilogo:
    //     The state in which an entity is being tracked by a context.
    public enum EntityState
    {
        //
        // Riepilogo:
        //     The entity is not being tracked by the context.
        Detached = 0,
        //
        // Riepilogo:
        //     The entity is being tracked by the context and exists in the database. Its property
        //     values have not changed from the values in the database.
        Unchanged = 1,
        //
        // Riepilogo:
        //     The entity is being tracked by the context and exists in the database. It has
        //     been marked for deletion from the database.
        Deleted = 2,
        //
        // Riepilogo:
        //     The entity is being tracked by the context and exists in the database. Some or
        //     all of its property values have been modified.
        Modified = 3,
        //
        // Riepilogo:
        //     The entity is being tracked by the context but does not yet exist in the database.
        Added = 4
    }
}
