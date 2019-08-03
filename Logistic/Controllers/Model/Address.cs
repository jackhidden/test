using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistic.Controllers.Model
{

    public class Province_List
    {
        public int ProvinceId { get; set; }
        public String Province { get; set; }
        public Province_List() { }

        public Province_List(Provinces provice)
        {
            this.ProvinceId = provice.Id;
            this.Province = provice.NameInThai;
        }
    }

    public class District_List
    {
        public int DistrictId { get; set; }
        public String District { get; set; }
        public District_List() { }

        public District_List(Districts districts)
        {
            this.DistrictId = districts.Id;
            this.District = districts.NameInThai;
        }
    }
}