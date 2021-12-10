using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleManagerModels
{
    public class AccessProperties
    {
        public string Access_token { get; set; }

        public string Token_type { get; set; }

        public long Expires_in { get; set; }

        public string Refresh_token { get; set; }

        public string Scope { get; set; }

        public DateTime Issued { get; set; }

        public DateTime IssuedUtc { get; set; }
    }
}
