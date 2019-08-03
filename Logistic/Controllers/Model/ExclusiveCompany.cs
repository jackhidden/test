using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistic.Controllers.Model
{
    public class Exclusive_List
    {
        public int ExclusiveId { get; set; }
        public String Company { get; set; }

        public Exclusive_List() { }
        public Exclusive_List(ExclusiveCompany exclusive) {

            this.ExclusiveId = exclusive.ExclusiveId;
            this.Company = exclusive.Company;
        }
    }

    public class Req_Exclusive_Add {
        public String Company { get; set; }
    }

    public class Req_Exclusive_Edit {
        public int ExclusiveId { get; set; }
        public String Company {get;set;}
    }

    public class Req_Exclusive_Remove
    {
        public int ExclusiveId { get; set; }
    }
}