using Mair.DigitalSuite.ServiceHost.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Models.Entities.Automation
{
    [Table("Nodes", Schema = "tag")]
    public class Node : BusinessBaseEntity
    {

        public string Driver { get; set; }
        public string ConnectionString { get; set; }
    }
}
