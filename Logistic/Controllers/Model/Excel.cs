using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logistic.Controllers.Model
{
    public class Req_Excel_Download
    {
        String DateFrom { get; set; }
        String DateTo { get; set; }
    }

    public class Req_Excel_Remove
    {
        public int ExcelId { get; set; }
    }


    public class ExcelFile_List
    {
        public int ExcelId { get; set; }
        public String NameFile { get; set; }
        public String UploadTime { get; set; }
        public int UserId { get; set; }
        public String Employee { get; set; }
        public bool IsDelete { get; set; }

        public ExcelFile_List() { }
        public ExcelFile_List(ExcelFile excel) {
            this.ExcelId = excel.ExcelId;
            this.NameFile = excel.NameFile;
            this.UploadTime = excel.UploadTime.ToString("dd-MM-yyyy HH:mm");
            this.UserId = excel.UserId;
            this.Employee = excel.Employee;
        }
    }
    public class ExcelData_List
    {
        public int DataId { get; set; }
        public int? ExcelId { get; set; }
        public int? No { get; set; }
        public String Crossdock { get; set; }
        public String ProductName { get; set; }
        public String Po_No { get; set; }
        public String InvoiceNo { get; set; }
        public String APC_Order { get; set; }
        public String DeliverId { get; set; }
        public String StoreId { get; set; }
        public String DateTime { get; set; }
        public String Province { get; set; }
        public String District { get; set; }
        public String StoreName { get; set; }
        public String ProductId { get; set; }
        public String ProductDetail { get; set; }
        public int? Box { get; set; }
        public int? Piece { get; set; }
        public decimal? CBM { get; set;}
        public decimal? Weight { get; set; }
        public String Category { get; set; }
        public String Note { get; set; }
        public String Week { get; set; }
        public decimal? Amount { get; set; }
        public ExcelData_List() { }

        public ExcelData_List(ExcelData data) {
            this.DataId = data.DataId;
            this.ExcelId = data.ExcelId;
            this.No = data.No;
            this.Crossdock = data.Crossdock;
            this.ProductName = data.ProductName;
            this.Po_No = data.PO_NO;
            this.InvoiceNo = data.InvoiceNo;
            this.APC_Order = data.APC_Order;
            this.DeliverId = data.DeliverId;
            this.StoreId = data.StoreId;
            this.DateTime = data.DateTime;
            this.Province = data.Province;
            this.District = data.District;
            this.StoreName = data.StoreName;
            this.ProductId = data.ProductId;
            this.ProductDetail = data.ProductDetail;
            this.Box = data.Box;
            this.Piece = data.Piece;
            this.CBM = data.CBM;
            this.Weight = data.Weight;
            this.Category = data.Category;
            this.Note = data.Note;
            this.Week = data.Week;
            this.Amount = data.Amount;
        }
    }
}