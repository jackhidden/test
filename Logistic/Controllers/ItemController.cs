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
    public class ItemController : Controller
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

        // GET: Item
        public ActionResult Index(int id = 0, int ExcelId = 0)
        {
            using (var context = new masterEntities())
            {
                UserProfile user = context.UserProfile.Where(o => o.UserId == id && o.IsDelete == false).FirstOrDefault();
                if (user == null)
                {
                    //return RedirectToAction("Index", "Account");
                }

                ViewBag.User = user;

                ExclusiveCompany company = context.ExclusiveCompany.Where(o => o.ExclusiveId == ExcelId).FirstOrDefault();

                if (company == null)
                {
                    //return RedirectToAction("Index", "Account");
                }

                ViewBag.Company = company;
            }
            return View();
        }

        public JsonResult List([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestParams, int ExclusiveId = 0)
        {
            using (var context = new masterEntities())
            {

                IQueryable<Item> query = context.Item.Where(o => o.ExclusiveId == ExclusiveId) as IQueryable<Item>;

                query = query.Where(o => o.IsDeleted == false);

                int totalRecords = query.Count();

                if (false == String.IsNullOrEmpty(requestParams.Search.Value))
                {
                    query = query.Where(o => o.Name.Contains(requestParams.Search.Value));
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

                List<Item> items = new List<Item>();
                items = query.ToList<Item>();

                List<Item_List> itemLocals = new List<Item_List>();
                foreach (Item item in items)
                {
                    Item_List itemLocal = new Item_List(item);
                    itemLocals.Add(itemLocal);
                }

                DataTablesResponse data = new DataTablesResponse(requestParams.Draw, itemLocals, recordsFiltered, totalRecords);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(Req_Item_Add req)
        {
            using (var context = new masterEntities())
            {
                String Name = req.Name.Trim();
                Item checksame = context.Item.Where(o => o.ExclusiveId == req.ExclusiveId && o.Name == Name && o.IsDeleted == false).FirstOrDefault();
                if (checksame != null)
                {
                    Status = "fail";
                }
                else
                {
                    Item item = new Item();
                    item.ExclusiveId = req.ExclusiveId;
                    item.Name = Name;
                    item.IsDeleted = false;

                    context.Item.Add(item);
                    context.SaveChanges();

                    Status = "success";
                }
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(Req_Item_Edit req)
        {
            using (var context = new masterEntities())
            {
                try
                {
                    String Name = req.Name.Trim();
                    Item checksame = context.Item.Where(o => o.ItemId == req.ItemId && o.ExclusiveId == req.ExclusiveId && o.Name == Name && o.IsDeleted == false).FirstOrDefault();
                    if (checksame != null)
                    {
                        Status = "fail";
                    }
                    else
                    {
                        Item item = context.Item.Where(o => o.ItemId == req.ItemId && o.ExclusiveId == req.ExclusiveId && o.IsDeleted == false).FirstOrDefault();
                        if (item != null)
                        {
                            item.Name = Name;

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

        public JsonResult Remove(Req_Item_Remove req)
        {
            using (var context = new masterEntities())
            {
                Item item = context.Item.Where(o => o.ItemId == req.ItemId && o.ExclusiveId == req.ExclusiveId && o.IsDeleted == false).FirstOrDefault();

                if(item != null)
                {
                    item.IsDeleted = true;
                    context.SaveChanges();

                    Status = "success";
                }

            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

    }
}