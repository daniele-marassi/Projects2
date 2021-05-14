using System;
using System.Collections.Generic;
using System.Text;

namespace Mair.DS.Models.Base
{
    public class BusinessBaseEntity : EntityBaseWithDates
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
