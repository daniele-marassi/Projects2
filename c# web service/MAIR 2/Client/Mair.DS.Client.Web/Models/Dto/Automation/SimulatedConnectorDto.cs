using Mair.DS.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mair.DS.Client.Web.Models.Dto.Automation
{
    public class SimulatedConnectorDto : EntityBaseWithDates
    {
        public long TagId { get; set; }
        public virtual TagDto Tag { get; set; }
        public string Value { get; set; }
    }
}
