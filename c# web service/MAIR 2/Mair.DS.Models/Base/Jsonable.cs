using Mair.DS.Common.JsonDeserializers;
using System;

namespace Mair.DS.Models.Base
{
    public class Jsonable : EntityBase
    {
        public Enum Type { get; set; }
        public string Json { get; set; }
        public object JsonObj { get; set; }

    }
}
