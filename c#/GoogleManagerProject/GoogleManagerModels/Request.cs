﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleManagerModels
{
    public class Request
    {
        public string Account { get; set; }
        public string Action { get; set; }
        public Auth Auth { get; set; }
        public List<FileProperties> Files { get; set; }
    }
}
