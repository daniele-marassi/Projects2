using System;

namespace Tools.MessagesPopUp
{
    [Serializable]
    public class Argument
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string Type { get; set; }

        public int TimeToClosePopUpInMilliseconds { get; set; }
        
    }
}
