using Mair.DS.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mair.DS.Client.Web.Models.Dto.Auth
{
    public class UserDto : EntityBaseWithDates
    {
         
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
