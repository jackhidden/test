using Logistic.Controllers.Api.DataTables;
using Logistic.Controllers.Model;
using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using System.Globalization;

namespace Logistic.Controllers
{
    public class ExcelDownloadController : Controller
    {
        private String Status = "fail";
        private List<Object> Data = new List<Object>();
        protected Object buildResponse()
        {
            Object response = null;

            if (Status == "success")
            {
                response = new
                {
                    status = Status,
                    data = Data
                };
            }
            else
            {
                response = new
                {
                    status = Status,
                    data = Data
                };
            }

            return response;
        }

        // GET: Excel Download
        public ActionResult Download(int id = 0, int ExcelId = 0)
        {
            using (var context = new masterEntities())
            {
                UserProfile user = context.UserProfile.Where(o => o.UserId == id && o.IsDelete == false).FirstOrDefault();
                if (user == null)
                {
                    return RedirectToAction("Index", "Account");
                }

                ViewBag.User = user;

                ExcelFile excelFile = context.ExcelFile.Where(o => o.ExcelId == ExcelId).FirstOrDefault();

                if (excelFile == null)
                {
                    return RedirectToAction("Index", "Account");
                }

                ViewBag.ExcelFile = excelFile;
            }
            return View();
        }

        public JsonResult DownloadList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestParams, int ExcelId = 0)
        {
            using (var context = new masterEntities())
            {
                IQueryable<ExcelData> query = context.ExcelData as IQueryable<ExcelData>;

                query = query.Where(o => o.ExcelId == ExcelId);

                int totalRecords = query.Count();

                if (false == String.IsNullOrEmpty(requestParams.Search.Value))
                {
                    query = query.Where(o => o.ProductName.Contains(requestParams.Search.Value) || o.Crossdock.Contains(requestParams.Search.Value)
                    || o.PO_NO.Contains(requestParams.Search.Value) || o.InvoiceNo.Contains(requestParams.Search.Value) || o.APC_Order.Contains(requestParams.Search.Value)
                    || o.DeliverId.Contains(requestParams.Search.Value) || o.StoreId.Contains(requestParams.Search.Value) || o.Province.Contains(requestParams.Search.Value)
                    || o.District.Contains(requestParams.Search.Value) || o.ProductId.Contains(requestParams.Search.Value) || o.ProductDetail.Contains(requestParams.Search.Value));
                }

                // Sorting
                query = query.OrderBy(o => o.No);

                IOrderedEnumerable<Column> orderedColumns = requestParams.Columns.GetSortedColumns();
                orderedColumns = orderedColumns.OrderBy(o => o.OrderNumber);
                foreach (Column column in orderedColumns)
                {
                    if (false == column.Orderable || false == column.IsOrdered) continue;

                    if (column.SortDirection == Column.OrderDirection.Ascendant)
                    {
                        query = query.OrderBy(column.Data + " Ascending");
                    }
                    else
                    {
                        query = query.OrderBy(column.Data + " Descending");
                    }
                }

                int recordsFiltered = query.Count();

                // Pagination                
                if (requestParams.Length != -1)
                {
                    query = query.Skip(requestParams.Start).Take(requestParams.Length);
                }

                List<ExcelData> excels = new List<ExcelData>();
                excels = query.ToList<ExcelData>();
                int i = 1;
                List<ExcelData_List> excellocals = new List<ExcelData_List>();
                foreach (ExcelData excel in excels)
                {
                    if (excel.No != i) {
                        excel.No = i;
                        context.SaveChanges();
                    }

                    excel.Amount = CalAmount(excel.Province,excel.District,excel.ProductName,(decimal)excel.Weight);
                    excel.Week = CalWeek(excel.DateTime);

                    ExcelData_List excellocal = new ExcelData_List(excel);
                    excellocals.Add(excellocal);
                    i++;
                }

                DataTablesResponse data = new DataTablesResponse(requestParams.Draw, excellocals, recordsFiltered, totalRecords);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public decimal CalAmount(String Province, String District ,String Product, decimal Weight)
        {
            decimal Amount = 0;
            Province = Province.Trim();
            Province = CheckString(Province, "P");
            District = District.Trim();
            District = CheckString(District, "D");
            Product = Product.Trim();
           
            using (var context = new masterEntities())
            {
                Item item = context.Item.Where(o => o.Name == Product).FirstOrDefault<Item>();
                if (item == null) return 0;
 
                ExclusiveCompany company = context.ExclusiveCompany.Where(o => o.ExclusiveId == item.ExclusiveId).FirstOrDefault<ExclusiveCompany>();
                if (company == null) return 0;

                RatePrice ratePrice = context.RatePrice.Where(o => o.ExclusiveId == company.ExclusiveId && o.Province == Province && o.District == District).FirstOrDefault<RatePrice>();
                if (ratePrice == null) return 0;
                
                    
                Amount = (decimal)ratePrice.RatePrice1 * Weight;
            }
            return Amount;
        }

        public String CheckString(String Data,String Type)
        {
            if(Type == "P")
            {
                bool check = Data.Contains("จังหวัด");
                if (check)
                {
                    Data = Data.Substring(0, 7);
                }
            }
            else if (Type == "D")
            {
                bool check1 = Data.Contains("อำเภอ");

                bool check2 = Data.Contains("กิ่งอำเภอ");
                if (check2 == true) check1 = false;
                bool check3 = Data.Contains("อำเภอกิ่งอำเภอ");
                if (check3 == true)
                {
                    check1 = false;
                    check2 = false;
                }

                if (check1)
                {
                    Data = Data.Substring(5);
                }
                if (check2)
                {
                    Data = Data.Substring(9);
                }
                if (check3)
                {
                    Data = Data.Substring(14);
                }
            }
            return Data;
        } 

        public String CalWeek(String Week)
        {        
            int WeekNo = 1;
            String WeekCal = "Week" + WeekNo;
            DateTime date = DateTime.ParseExact(Week, "dd/MM/yyyy HH:mm", CultureInfo.CurrentUICulture.DateTimeFormat);
            DateTime dateDay1;
            for (int i = 1; i <= date.Day; i++)
            {
				String Month = date.Month.ToString();
				if (Month.Length == 1)
				{
					Month = "0" + Month;
				}
                String day1 = "01/" + Month + "/" + date.Year;
                dateDay1 = DateTime.ParseExact(day1, "dd/MM/yyyy", CultureInfo.CurrentUICulture.DateTimeFormat);
                dateDay1 = dateDay1.AddDays(i);
                if(dateDay1.DayOfWeek.ToString() == "Saturday")
                {
                    WeekNo++;
                    WeekCal = "Week" + WeekNo;
                }
            }
            return WeekCal;
        }
    }
}