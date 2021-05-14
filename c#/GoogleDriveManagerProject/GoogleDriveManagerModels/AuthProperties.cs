using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveManagerModels
{
    public class AuthProperties
    {
        public string Client_id { get; set; }

        public string Project_id { get; set; }

        public string Auth_uri { get; set; }

        public string Token_uri { get; set; }

        public string Auth_provider_x509_cert_url { get; set; }

        public string Client_secret { get; set; }

        public string[] Redirect_uris { get; set; }
    }
}
