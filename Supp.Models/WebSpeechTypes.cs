using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    public enum WebSpeechTypes
    {
        SystemRunExe = 0,
        SystemRequest = 1,
        RunExe = 2,
        Request = 3,
        Link = 4,
        Firefox = 5,
        Chrome = 6,
        SystemWebSearch = 7,
        Meteo = 8,
        Time = 9,
        SongsPlayer = 10,
        ReadNotes = 11,
        SystemReadNotes = 12,
        EditNote = 13,
        SystemEditNote = 14,
        SystemDeleteNote = 15,
        ReadReminder = 16,
        ReadRemindersToday = 17,
        ReadRemindersTomorrow = 18,
        SystemEditReminder = 19,
        SystemDeleteReminder = 20,
        SystemRequestNotImplemented = 21
    }

    public class WebSpeechTypesUtility
    {
        /// <summary>
        /// Get WebSpeechTypes
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<WebSpeechType> Get()
        {
            var webSpeechTypes = new List<WebSpeechType>();
            var values = Enum.GetValues(typeof(WebSpeechTypes));
            foreach (var item in values)
            {
                webSpeechTypes.Add(new WebSpeechType() { Id = item.ToString(), Name = item.ToString() });
            }
            return webSpeechTypes;
        }
    }

    public class WebSpeechType
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}