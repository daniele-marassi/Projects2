using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Site.Models
{
    public class WebSpeechTypes
    {
        public static IEnumerable<WebSpeechType> Get()
        {
            var list = new List<WebSpeechType>() { };
            list.Add(new WebSpeechType() { Id = "SystemRunExe" , Type = "SystemRunExe"    });
            list.Add(new WebSpeechType() { Id = "SystemRequest", Type = "SystemRequest"   });
            list.Add(new WebSpeechType() { Id = "RunExe"       , Type = "RunExe"          });
            list.Add(new WebSpeechType() { Id = "Request"      , Type = "Request"         });
            list.Add(new WebSpeechType() { Id = "Link"         , Type = "Link"            });
            list.Add(new WebSpeechType() { Id = "Firefox"      , Type = "Firefox"         });
            list.Add(new WebSpeechType() { Id = "Chrome"       , Type = "Chrome" });

            return list;
        }

        public class WebSpeechType
        {
            public string Id { get; set; }
            public string Type { get; set; }
        }
    }
}