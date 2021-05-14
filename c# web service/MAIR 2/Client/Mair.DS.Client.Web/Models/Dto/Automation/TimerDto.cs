using Mair.DS.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mair.DS.Client.Web.Models.Dto.Automation
{
    public class TimerDto : BusinessBaseEntity
    {
        public int Interval { get; set; }
    }
}
