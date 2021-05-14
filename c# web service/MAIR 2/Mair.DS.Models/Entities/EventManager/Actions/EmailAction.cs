using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.EventManager.Actions
{
    [Table("EmailActions", Schema = "EventManager")]
    public class EmailAction : Action
    {
        public string Subject { get; set; }
        public string MailToAddresses { get; set; }
        public string Message { get; set; }

    }
}
