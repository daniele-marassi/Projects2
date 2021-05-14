using Mair.DS.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mair.DS.Client.Web.Models.Dto.Automation
{
    public class TagDto : BusinessBaseEntity
    {
        public string Address { get; set; }

        public bool IsEnabled { get; set; }

        public long NodeId { get; set; }
        public virtual NodeDto Node { get; set; }

    }
}
