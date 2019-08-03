using Logistic.Controllers.Api.DataTables;
using Logistic.Controllers.Model;
using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace Logistic.Controllers
{
    public class SettingsController : Controller
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

        // GET: Settings
        public ActionResult Index(int id)
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

        public JsonResult List([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestParams, int ExclusiveId = 0)
        {
            using (var context = new masterEntities())
            {

                IQueryable<RatePrice> query = context.RatePrice as IQueryable<RatePrice>;

                if (ExclusiveId != 0)
                {
                    query = query.Where(o => o.ExclusiveId == ExclusiveId);
                }

                int totalRecords = query.Count();

                if (false == String.IsNullOrEmpty(requestParams.Search.Value))
                {
                    query = query.Where(o => o.Province.Contains(requestParams.Search.Value) || o.District.Contains(requestParams.Search.Value));
                }

                // Sorting
                query = query.OrderByDescending(o => o.RateId);

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

                List<RatePrice> ratePrices = new List<RatePrice>();
                ratePrices = query.ToList<RatePrice>();

                List<RatePrice_List> ratePricelocals = new List<RatePrice_List>();
                foreach (RatePrice ratePrice in ratePrices)
                {
                    RatePrice_List ratePricelocal = new RatePrice_List(ratePrice);
                    ratePricelocals.Add(ratePricelocal);
                }

                DataTablesResponse data = new DataTablesResponse(requestParams.Draw, ratePricelocals, recordsFiltered, totalRecords);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(Req_RatePrice_Add req)
        {
            using (var context = new masterEntities())
            {
                String Province = req.Province.Trim();
                String District = req.District.Trim();
                RatePrice checksame = context.RatePrice.Where(o => o.Province == Province && o.District == District && o.ExclusiveId == req.ExclusiveId).FirstOrDefault();
                if (checksame != null)
                {
                    Status = "fail";
                }
                else
                {
                    RatePrice checkNo = context.RatePrice.OrderByDescending(o => o.No).FirstOrDefault();
                    int No = 1;
                    if (checkNo != null) No = (int)checkNo.No + 1;
                    RatePrice rate = new RatePrice();
                    rate.ExclusiveId = req.ExclusiveId;
                    rate.Exclusive = req.Exclusive.Trim();
                    rate.No = No;
                    rate.ProvinceId = req.ProvinceId;
                    rate.Province = Province;
                    rate.DistrictId = req.DistrictId;
                    rate.District = District;
                    rate.RatePrice1 = req.RatePrice;

                    context.RatePrice.Add(rate);
                    context.SaveChanges();

                    Status = "success";
                }
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(Req_RatePrice_Edit req)
        {
            using (var context = new masterEntities())
            {
                try {

                    RatePrice rate = context.RatePrice.Where(o => o.RateId == req.RateId).FirstOrDefault();
                    rate.ExclusiveId = req.ExclusiveId;
                    rate.Exclusive = req.Exclusive.Trim();
                    rate.ProvinceId = req.ProvinceId;
                    rate.Province = req.Province.Trim();
                    rate.DistrictId = req.DistrictId;
                    rate.District = req.District.Trim();
                    rate.RatePrice1 = req.RatePrice;

                    context.SaveChanges();

                    Status = "success";
                }
                catch (Exception ex)
                {
                    Status = "fail";
                }
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Remove(Req_RatePrice_Remove req)
        {
            using (var context = new masterEntities())
            {
                RatePrice rate = context.RatePrice.Where(o => o.RateId == req.RateId).FirstOrDefault();

                context.RatePrice.Remove(rate);
                context.SaveChanges();

                Status = "success";
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }
    }
}