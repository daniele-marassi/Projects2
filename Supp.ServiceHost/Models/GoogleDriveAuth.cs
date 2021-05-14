using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Models
{
    [Table("GoogleDriveAuths", Schema = "dbo")]
    public class GoogleDriveAuth
    {
        public long Id { get; set; }
        public string Client_id { get; set; }

        public string Project_id { get; set; }

        public string Client_secret { get; set; }

        public System.DateTime InsDateTime { get; set;}
    }
}
