using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    public enum WebSpeechTypes
    {
        SystemRequest = 1,
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
        DeleteNote = 15,
        SystemDeleteNote = 16,
        ReadReminder = 17,
        ReadRemindersToday = 18,
        ReadRemindersTomorrow = 19,
        SystemEditReminder = 20,
        SystemDeleteReminder = 21,
        SystemDialogueRequestNotImplemented = 22,
        SystemDialogueAddToNote = 23,
        SystemDialogueAddToNoteWithName = 24,
        SystemDialogueClearNote = 25,
        SystemDialogueClearNoteWithName = 26,
        SystemDialogueGeneric = 27,
        CreateNote = 28,
        SystemCreateNote = 29,
        SystemDialogueCreateNote = 30,
        SystemDialogueCreateNoteWithName = 31,
        SystemDialogueDeleteNote = 32,
        SystemDialogueDeleteNoteWithName = 33,
        WebSearch = 34,
        SystemDialogueWebSearch = 35,
        SystemDialogueRunExe = 36,
        SystemRunExe = 37,
        RunExe = 38,
        SystemRunExeWithNumericParameter = 39,
        RunExeWithNumericParameter = 40,
        SystemRunExeWithNotNumericParameter = 41,
        RunExeWithNotNumericParameter = 42,
        SystemDialogueCreateReminder = 43,
        SystemDialogueDeleteReminder = 44,
        CreateReminder = 45,
        DeleteReminder = 46,
        SystemCreateReminder = 47,
        SetTimer = 48,
        SystemSetTimer = 49,
        SystemDialogueSetTimer = 50,
        ReadThermostat = 51,
        SystemReadThermostat = 52,
        SetThermostat = 53,
        SystemSetThermostat = 54
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

            webSpeechTypes = webSpeechTypes.OrderBy(_ => _.Name).ToList();

            return webSpeechTypes;
        }
    }

    public class WebSpeechType
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}