using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SuppModels
{
    [Table("GoogleAuths", Schema = "dbo")]
    public class GoogleAuth
    {
        public long Id { get; set; }
        public string Client_id { get; set; }
        public string Project_id { get; set; }
        public string Client_secret { get; set; }
        public string TokenFileInJson { get; set; }
        public string GooglePublicKey { get; set; }        
        public System.DateTime InsDateTime { get; set;}
    }
}
