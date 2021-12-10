using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuppModels
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
        EditNote = 12,
        DeleteNote = 13,
        ReadRemindersToday = 14,
        ReadRemindersTomorrow = 15,
        EditReminder = 16,
        DeleteReminder = 17,
        RequestNotImplemented = 18
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
                webSpeechTypes.Add(new WebSpeechType() { Id = item.ToString(), Type = item.ToString() });
            }
            return webSpeechTypes;
        }
    }

    public class WebSpeechType
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }
}