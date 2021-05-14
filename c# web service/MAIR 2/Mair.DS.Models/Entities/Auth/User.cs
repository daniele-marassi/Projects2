using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Models.Entities.Auth
{
    [Table("Users", Schema = "auth")]
    public class User : EntityBaseWithDates
    {
         
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
