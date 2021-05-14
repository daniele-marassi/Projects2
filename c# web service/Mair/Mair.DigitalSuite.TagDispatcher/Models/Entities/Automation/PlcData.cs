using Mair.DigitalSuite.TagDispatcher.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mair.DigitalSuite.TagDispatcher.Models.Entities.Automation
{ 
    [Table("PlcData", Schema = "tag")]
    public class PlcData: EntityBaseWithDates
    {
        public string Driver { get; set; }
        public string ConnectionString { get; set; }
        public string TagAddress { get; set; }
        public string TagValue { get; set; }
    }
}
