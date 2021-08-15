using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Site.Models
{
    public class WebSpeechTypes
    {
        /// <summary>
        /// Get WebSpeechTypes
        /// </summary>
        /// <returns></returns>
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
            list.Add(new WebSpeechType() { Id = "SystemWebSearch", Type = "SystemWebSearch" });
            list.Add(new WebSpeechType() { Id = "Meteo", Type = "Meteo" });
            list.Add(new WebSpeechType() { Id = "Time", Type = "Time" });
            list.Add(new WebSpeechType() { Id = "SongsPlayer", Type = "SongsPlayer" });

            return list;
        }

        public class WebSpeechType
        {
            public string Id { get; set; }
            public string Type { get; set; }
        }
    }
}