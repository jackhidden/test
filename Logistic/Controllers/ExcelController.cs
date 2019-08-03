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
    public class ExcelController : Controller
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
        // GET: Excel
        public ActionResult Index()
        {
            return View();
        }

        // GET: Excel Import
        public ActionResult List(int id = 0)
        {
            using (var context = new masterEntities())
            {
                UserProfile user = context.UserProfile.Where(o => o.UserId == id && o.IsDelete == false).FirstOrDefault();
                if (user == null)
                {
                    return RedirectToAction("Index", "Account");
                }

                ViewBag.User = user;
            }
            return View();
        }

        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase excelfile, int UserId)
        {
            using (var context = new masterEntities())
            {
                UserProfile user = context.UserProfile.Where(o => o.UserId == UserId && o.IsDelete == false).FirstOrDefault();
                if (user == null)
                {
                    return RedirectToAction("Index", "Account");
                }

                DateTime dateTimeNow = DateTime.Now;
                if (excelfile == null ||excelfile.ContentLength == 0) {
                    Status = "fail";
                }
                else
                {
                    string data = "";
                    if (excelfile != null)
                    {
                        if (excelfile.ContentType == "application/vnd.ms-excel" || excelfile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            string filename = excelfile.FileName;
                            if (filename.EndsWith(".xlsx") || filename.EndsWith(".xls"))
                            {
                                string targetpath = Server.MapPath("~/ExcelFile/");
                                excelfile.SaveAs(targetpath + filename);
                                string pathToExcelFile = targetpath + filename;

                                string sheetName = "Sheet1";

                                ExcelFile excel = new ExcelFile();
                                excel.NameFile = filename.Trim();
                                excel.CreateTime = dateTimeNow.Date;
                                excel.UploadTime = dateTimeNow;
                                excel.UserId = user.UserId;
                                excel.Employee = user.Name + ' ' + user.Surname;
                                context.ExcelFile.Add(excel);
                                context.SaveChanges();


                                int no = 1;
                                var excelFile = new ExcelQueryFactory(pathToExcelFile);
                                var empDetails = from a in excelFile.Worksheet(sheetName) select a;
                                foreach (var a in empDetails)
                                {
                                    
                                    String Crossdock = a[0];
                                    String ProductName = a[1];
                                    String PO_Number = a[2];
                                    String Inovice_Number = a[3];
                                    String APC_Order = a[4];
                                    String DeliverId = a[5];
                                    String StoreId = a[6];
                                    String DateTimes = a[7];
									String AllDate = "";
									if (DateTimes != "" || DateTimes != null) {
										String[] Data = DateTimes.Split('/');
										
										if (Data[0].Length == 1)
										{
											Data[0] = "0" + Data[0];
										}
										if (Data[1].Length == 1)
										{
											Data[1] = "0" + Data[1];
										}
										AllDate = Data[0] + '/' + Data[1] + '/' + Data[2];
									}
									
									String Province = a[8];
                                    String District = a[9];
                                    String StoreName = a[10];
                                    String ProductId = a[11];
                                    String ProductDetail = a[12];
                                    int Box = Convert.ToInt32(a[13]);
                                    int Piece = Convert.ToInt32(a[14]);
                                    Decimal CBM = Convert.ToDecimal(a[15]);
                                    Decimal Weight = Convert.ToDecimal(a[16]);
                                    String Category = a[17];
                                    String Note = a[18];
                                    
                                    String Status = PostExcelData(excel.ExcelId , no , Crossdock , ProductName , PO_Number , Inovice_Number , APC_Order , DeliverId , StoreId , AllDate, Province , District 
                                        , StoreName , ProductId , ProductDetail , Box, Piece , CBM , Weight , Category , Note);

                                    no++;
                                }
                            }
                        }
                            Status = "success";
                    }
                    else
                    {
                        Status = "error";
                    }
                }
                
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public String PostExcelData(int ExcelId, int No ,String Crossdock, String ProductName ,String PO_Number,String Inovice_Number,String APC_Order,String DeliverId,String StoreId , String DateTime ,String Province , String District , String StoreName ,String ProductId , String ProductDetail , int Box , int Piece , Decimal CBM , Decimal Weight , String Category , String Note)
        {
            using (var context = new masterEntities())
            {
                

                try
                {
                    ExcelData data = new ExcelData();
                    data.ExcelId = ExcelId;
                    data.No = No;
                    data.Crossdock = Crossdock.Trim();
                    data.ProductName = ProductName.Trim();
                    data.PO_NO = PO_Number.Trim();
                    data.InvoiceNo = Inovice_Number.Trim();
                    data.APC_Order = APC_Order.Trim();
                    data.DeliverId = DeliverId.Trim();
                    data.StoreId = StoreId.Trim();
                    data.DateTime = DateTime.Trim();
                    data.Province = Province.Trim();
                    data.District = District.Trim();
                    data.StoreName = StoreName.Trim();
                    data.ProductId = ProductId.Trim();
                    data.ProductDetail = ProductDetail.Trim();
                    data.Box = Box;
                    data.Piece = Piece;
                    data.CBM = CBM;
                    data.Weight = Weight;
                    data.Category = Category.Trim();
                    data.Note = Note.Trim();
                    context.ExcelData.Add(data);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }

            return Status = "success";
        }

        public JsonResult Remove(Req_Excel_Remove req)
        {
            using (var context = new masterEntities())
            {
                ExcelFile excelFile = context.ExcelFile.Where(o => o.ExcelId == req.ExcelId).FirstOrDefault();
                if (excelFile == null) {
                    Status = "fail";
                }
                else
                {
                    List<ExcelData> excelDatas = context.ExcelData.Where(o => o.ExcelId == excelFile.ExcelId).ToList();
                    if (excelDatas.Count != 0)
                    {
                        foreach (ExcelData excelData in excelDatas)
                        {
                            ExcelData excelDataRemove = context.ExcelData.Where(o => o.ExcelId == excelData.ExcelId).FirstOrDefault();
                            context.ExcelData.Remove(excelDataRemove);
                            context.SaveChanges();
                        }
                    }
                    context.ExcelFile.Remove(excelFile);
                    context.SaveChanges();
                    Status = "success";
                }         
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ImportList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestParams, String Date = "",int UserId = 0)
        {
            using (var context = new masterEntities())
            {
                IQueryable<ExcelFile> query = context.ExcelFile as IQueryable<ExcelFile>;

                if (Date != "")
                {
                    DateTime date = DateTime.ParseExact(Date, "yyyy/MM/dd", CultureInfo.InvariantCulture);

                    query = query.Where(o => o.CreateTime == date);
                }

                if (UserId != 0)
                {
                    query = query.Where(o => o.UserId == UserId);
                }

                int totalRecords = query.Count();

                if (false == String.IsNullOrEmpty(requestParams.Search.Value))
                {
                    query = query.Where(o => o.NameFile.Contains(requestParams.Search.Value) || o.Employee.Contains(requestParams.Search.Value));
                }

                // Sorting
                query = query.OrderBy(o => o.ExcelId);

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

                List<ExcelFile> excels = new List<ExcelFile>();
                excels = query.ToList<ExcelFile>();

                List<ExcelFile_List> excellocals = new List<ExcelFile_List>();
                foreach (ExcelFile excel in excels)
                {
                    ExcelFile_List excellocal = new ExcelFile_List(excel);
                    excellocals.Add(excellocal);
                }

                DataTablesResponse data = new DataTablesResponse(requestParams.Draw, excellocals, recordsFiltered, totalRecords);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }
    }
}