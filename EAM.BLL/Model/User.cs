﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EAM.BLL.Model
{
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserRoleID { get; set; }
    }
}
