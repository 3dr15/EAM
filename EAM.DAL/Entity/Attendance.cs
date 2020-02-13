using System;
using System.Collections.Generic;
using System.Text;

namespace EAM.DAL.Entity
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public int CardID { get; set; }
        public Card Card { get; set; }
        public DateTimeOffset DateTime { get; set; }
    }
}
