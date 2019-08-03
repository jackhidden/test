using Logistic.Controllers.Api.DataTables;
using Logistic.Controllers.Model;
using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace Logistic.Controllers
{
    public class EmployeeController : Controller
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
        // GET: Employee
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

        public JsonResult Register(Req_Employee_Register req)
        {
            using (var context = new masterEntities())
            {
                UserProfile checkUser = context.UserProfile.Where(o => o.Username == req.Username && o.IsDelete == false).FirstOrDefault();
                if (checkUser != null)
                {
                    Status = "fail";
                }
                else
                {
                    UserProfile user = new UserProfile();
                    user.Username = req.Username;
                    user.Password = req.Password;
                    user.Name = req.Name ?? "";
                    user.Surname = req.Surname ?? "";
                    user.Position = 2;
                    user.IsDelete = false;

                    context.UserProfile.Add(user);
                    context.SaveChanges();

                    Status = "success";
                }          
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(Req_Employee_Edit req)
        {
            using (var context = new masterEntities())
            {
                UserProfile User = context.UserProfile.Where(o => o.UserId == req.UserId && o.IsDelete == false).FirstOrDefault();
                if (User != null)
                {
                    User.Username = req.Username;
                    User.Password = req.Password;
                    User.Name = req.Name ?? "";
                    User.Surname = req.Surname ?? "";

                    context.SaveChanges();

                    Status = "success";                
                }
                else
                {
                    Status = "fail";
                }
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Remove(Req_Employee_Remove req)
        {
            using (var context = new masterEntities())
            {
                UserProfile User = context.UserProfile.Where(o => o.UserId == req.UserId).FirstOrDefault();

                User.IsDelete = true;
                context.SaveChanges();

                    Status = "success";
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult List([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestParams)
        {
            using (var context = new masterEntities())
            {

                IQueryable<UserProfile> query = context.UserProfile as IQueryable<UserProfile>;

                query = query.Where(o => o.IsDelete == false);

                int totalRecords = query.Count();

                if (false == String.IsNullOrEmpty(requestParams.Search.Value))
                {
                    query = query.Where(o => o.Name.Contains(requestParams.Search.Value) || o.Username.Contains(requestParams.Search.Value));
                }

                // Sorting
                query = query.OrderBy(o => o.UserId);

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

                List<UserProfile> users = new List<UserProfile>();
                users = query.ToList<UserProfile>();

                List<Employee_List> empUsers = new List<Employee_List>();
                foreach (UserProfile user in users)
                {
                    Employee_List empUser = new Employee_List(user);
                    empUsers.Add(empUser);                   
                }

                DataTablesResponse data = new DataTablesResponse(requestParams.Draw, empUsers, recordsFiltered, totalRecords);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }
    }
}