using Mair.DigitalSuite.TagDispatcher.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mair.DigitalSuite.TagDispatcher.Models.Dto.Automation
{
    public class PlcDataDto : EntityBaseWithDates
    {
        public string Driver { get; set; }
        public string ConnectionString { get; set; }
        public string TagAddress { get; set; }
        public string TagValue { get; set; }
    }
}
