using Mair.DS.Client.Web.Models.Enums;
using Mair.DS.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mair.DS.Client.Web.Models.Dto.Automation
{
    public class EventDto : BusinessBaseEntity
    {
        public TriggerType TriggerType { get; set; }

        public virtual TimerDto Timer { get; set; }

        public virtual TagDto StartTag { get; set; }

        public virtual TagDto EndTag { get; set; }

        public virtual TagDto AcknowledgeTag { get; set; }
    }
}
