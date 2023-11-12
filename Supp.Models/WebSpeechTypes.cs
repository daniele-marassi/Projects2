using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Models
{
    public enum WebSpeechTypes
    {
        Link,
        Firefox,
        Chrome,
        Meteo,
        Time,
        SongsPlayer,
        Request,
        SystemRequest,
        SystemDialogueRequestNotImplemented,
        ReadNotes,
        SystemReadNotes,
        EditNote,
        SystemEditNote,
        DeleteNote,     
        SystemDeleteNote,
        SystemDialogueDeleteNote,
        SystemDialogueDeleteNoteWithFixedName,
        DialogueAddToNote,
        DialogueAddToNoteWithFixedName,
        DialogueClearNote,
        DialogueClearNoteWithFixedName,
        SystemDialogueAddToNote,
        SystemDialogueAddToNoteWithFixedName,
        SystemDialogueClearNote,
        SystemDialogueClearNoteWithFixedName,       
        CreateNote,
        SystemCreateNote,
        SystemDialogueCreateNote,
        SystemDialogueCreateNoteWithFixedName,
        WebSearch,
        SystemWebSearch,
        SystemDialogueWebSearch,
        RunExe,
        SystemRunExe,
        SystemDialogueRunExe,
        RunExeWithNumericParameter,
        SystemRunExeWithNumericParameter,
        RunExeWithNotNumericParameter,
        SystemRunExeWithNotNumericParameter,
        RunMediaAndPlay,
        SystemRunMediaAndPlay,
        SystemDialogueRunMediaAndPlay,
        RunMediaAndPlayWithNumericParameter,
        SystemRunMediaAndPlayWithNumericParameter,
        RunMediaAndPlayWithNotNumericParameter,
        SystemRunMediaAndPlayWithNotNumericParameter,
        ReadThermostat,
        SystemReadThermostat,
        SetThermostat,
        SystemSetThermostat,
        ReadReminder,
        ReadRemindersToday,
        ReadRemindersTomorrow,
        SystemEditReminder,
        DeleteReminder,
        SystemDeleteReminder,
        SystemDialogueDeleteReminder,
        CreateExtendedReminder,
        SystemCreateExtendedReminder,
        SystemDialogueCreateExtendedReminder,
        CreateReminder,
        SystemCreateReminder,
        SystemDialogueCreateReminder,
        SetTimer,
        SystemSetTimer,
        SystemDialogueSetTimer,
        DialogueSetTimer,
        SystemDialogueSetTimerWithFixedName,
        DialogueSetTimerWithFixedName,
        DeleteTimer,
        SystemDeleteTimer,
        SystemDialogueDeleteTimer,
        SetAlarmClock,
        SystemSetAlarmClock,
        SystemDialogueSetAlarmClock,
        DeleteAlarmClock,
        SystemDeleteAlarmClock,
        SystemDialogueDeleteAlarmClock,
        MediaPlayOrPause,
        MediaNextTrack,
        MediaPreviousTrack
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