using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Client.Web.Models.Dto.Auth
{
    public class RoleDto : EntityBaseWithDates
    {
         
        public string Type { get; set; }
    }
}
