﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsSender
{
    public enum MessageType
    {
        All_Candidates = 1,
        Scheduled_Candidates = 2,
        Unscheduled_Candidates = 3,
        Tested_Candidates = 4
    }
}