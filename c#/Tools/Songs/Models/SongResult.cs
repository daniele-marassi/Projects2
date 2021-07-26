using System;
using System.Collections.Generic;

namespace Tools.Songs.Models
{
    public class SongResult
    {
        public List<SongDto> Data { get; set; }
        public ResultType ResultState { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
        public Exception OriginalException { get; set; }
    }
}
