using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mair.DS.Models.Base
{
    public class EntityBaseWithDates : EntityBase
    {
        public DateTime Created { get; set; }
        public DateTime LastUpdated{ get; set; }

    }
}
