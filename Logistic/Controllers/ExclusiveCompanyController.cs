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
    public class ExclusiveCompanyController : Controller
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

        // GET: ExclusiveCompany
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

        public JsonResult List([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestParams)
        {
            using (var context = new masterEntities())
            {

                IQueryable<ExclusiveCompany> query = context.ExclusiveCompany as IQueryable<ExclusiveCompany>;

                query = query.Where(o => o.IsDeleted == false);

                int totalRecords = query.Count();

                if (false == String.IsNullOrEmpty(requestParams.Search.Value))
                {
                    query = query.Where(o => o.Company.Contains(requestParams.Search.Value));
                }

                // Sorting
                query = query.OrderByDescending(o => o.ExclusiveId);

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

                List<ExclusiveCompany> exclusives = new List<ExclusiveCompany>();
                exclusives = query.ToList<ExclusiveCompany>();

                List<Exclusive_List> exclusiveLocals = new List<Exclusive_List>();
                foreach (ExclusiveCompany exclusive in exclusives)
                {
                    Exclusive_List exclusiveLocal = new Exclusive_List(exclusive);
                    exclusiveLocals.Add(exclusiveLocal);
                }

                DataTablesResponse data = new DataTablesResponse(requestParams.Draw, exclusiveLocals, recordsFiltered, totalRecords);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(Req_Exclusive_Add req)
        {
            using (var context = new masterEntities())
            {
                String Company = req.Company.Trim();
                ExclusiveCompany checksame = context.ExclusiveCompany.Where(o => o.Company == Company && o.IsDeleted == false).FirstOrDefault();
                if (checksame != null)
                {
                    Status = "fail";
                }
                else
                {
                    ExclusiveCompany company = new ExclusiveCompany();
                    company.Company = Company;
                    company.IsDeleted = false;

                    context.ExclusiveCompany.Add(company);
                    context.SaveChanges();

                    Status = "success";
                }
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(Req_Exclusive_Edit req)
        {
            using (var context = new masterEntities())
            {
                try
                {
                    String Company = req.Company.Trim();
                    ExclusiveCompany checksame = context.ExclusiveCompany.Where(o => o.Company == Company && o.IsDeleted == false).FirstOrDefault();
                    if (checksame != null)
                    {
                        Status = "fail";
                    }
                    else
                    {
                        ExclusiveCompany company = context.ExclusiveCompany.Where(o => o.ExclusiveId == req.ExclusiveId && o.IsDeleted == false).FirstOrDefault();
                        if(company != null) { 
                        company.Company = Company;

                        context.SaveChanges();

                        Status = "success";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Status = "fail";
                }
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Remove(Req_Exclusive_Remove req)
        {
            using (var context = new masterEntities())
            {
                ExclusiveCompany company = context.ExclusiveCompany.Where(o => o.ExclusiveId == req.ExclusiveId).FirstOrDefault();

                if(company != null)
                {
                    company.IsDeleted = true;
                    context.SaveChanges();

                    Status = "success";
                }

            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }
    }
}