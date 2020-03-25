using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAM.API.Model
{
    public class RFIDPartial: RFID
    {
        public string DateTimeUTC { get; set; }
    }
}
