using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Client.Web.Models.Dto.Auth
{
    public class RolePathDto : BusinessBaseEntity
    {
         
        public long RoleId { get; set; }

        public string Path { get; set; }

        public bool IsEnabled { get; set; }
    }
}
