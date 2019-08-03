using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistic.Controllers.Model
{
    public class Req_RatePrice_Add
    {
        public int ExclusiveId { get; set; }
        public String Exclusive { get; set; }
        public int ProvinceId { get; set; }
        public String Province { get; set; }
        public int DistrictId { get; set; }
        public String District { get; set; }
        public Decimal RatePrice { get; set; }
    }

    public class Req_RatePrice_Edit
    {
        public int RateId { get; set; }
        public String Exclusive { get; set; }
        public int ExclusiveId { get; set; }
        public int ProvinceId { get; set; }
        public String Province { get; set; }
        public int DistrictId { get; set; }
        public String District { get; set; }
        public Decimal RatePrice { get; set; }
    }

    public class Req_RatePrice_Remove
    {
        public int RateId { get; set; }
    }

    public class RatePrice_List
    {
        public int RateId { get; set; }
        public int? ExclusiveId { get; set; }
        public String Exclusive { get; set; }
        public int? ProvinceId { get; set; }
        public String Province { get; set; }
        public int? DistrictId { get; set; }
        public String District { get; set; }
        public Decimal? RatePrice { get; set; }
        public RatePrice_List() { }

        public RatePrice_List(RatePrice rate)
        {
            this.RateId = rate.RateId;
            this.ExclusiveId = rate.ExclusiveId;
            this.Exclusive = rate.Exclusive;
            this.ProvinceId = rate.ProvinceId;
            this.Province = rate.Province;
            this.DistrictId = rate.DistrictId;
            this.District = rate.District;
            this.RatePrice = rate.RatePrice1;
        }
    }
}