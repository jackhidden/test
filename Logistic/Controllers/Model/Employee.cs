using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistic.Controllers.Model
{
    public class Req_Employee_Register
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
    }

    public class Req_Employee_Edit
    {
        public int UserId { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
    }

    public class Req_Employee_Remove
    {
        public int UserId { get; set; }
    }

    public class Employee_List
    {
        public int UserId { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public int Seq { get; set; }
        public Employee_List() { }

        public Employee_List(UserProfile user)
        {
            this.UserId = user.UserId;
            this.Username = user.Username;
            this.Password = user.Password;
            this.Name = user.Name;
            this.Surname = user.Surname;
        }
    }
}