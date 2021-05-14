using System;

namespace Mair.DS.Models.Base
{
    public class EntityBaseWithDates : EntityBase
    {
        public DateTime Created { get; set; }
        public DateTime LastUpdated{ get; set; }

    }
}
