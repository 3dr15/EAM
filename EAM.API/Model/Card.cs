﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EAM.API.Model
{
    public class Card
    {
        public int CardID { get; set; }
        public string RFID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
