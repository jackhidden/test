//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Logistic.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RatePrice
    {
        public int RateId { get; set; }
        public Nullable<int> No { get; set; }
        public Nullable<int> ExclusiveId { get; set; }
        public string Exclusive { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public string Province { get; set; }
        public Nullable<int> DistrictId { get; set; }
        public string District { get; set; }
        public Nullable<decimal> RatePrice1 { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    
        public virtual RatePrice RatePrice11 { get; set; }
        public virtual RatePrice RatePrice2 { get; set; }
    }
}
