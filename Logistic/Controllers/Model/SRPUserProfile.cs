using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistic.Controllers.Model
{
    public class SRPUserProfile
    {
        public int UserId { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
    }
    public class Req_User_Login
    {
        public String Username { get; set; }
        public String Password { get; set; }
    }
}