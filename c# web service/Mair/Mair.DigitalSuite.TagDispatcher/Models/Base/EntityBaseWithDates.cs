using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.TagDispatcher.Models.Base
{
    public class EntityBaseWithDates: EntityBase
    {
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
