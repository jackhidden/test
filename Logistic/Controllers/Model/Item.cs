using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistic.Controllers.Model
{
    public class Item_List
    {
        public int ItemId { get; set; }
        public int? ExclusiveId { get; set; }
        public String Name { get; set; }

        public Item_List() { }
        public Item_List(Item item) {
            this.ItemId = item.ItemId;
            this.ExclusiveId = item.ExclusiveId;
            this.Name = item.Name;
        }
    }

    public class Req_Item_Add
    {
        public int ExclusiveId { get; set; }
        public String Name { get; set; }
    }

    public class Req_Item_Edit
    {
        public int ItemId { get; set; }
        public int ExclusiveId { get; set; }
        public String Name { get;set;}
    }

    public class Req_Item_Remove
    {
        public int ItemId { get; set; }
        public int ExclusiveId { get; set; }
    }
}