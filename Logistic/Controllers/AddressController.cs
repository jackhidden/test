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
    public class AddressController : Controller
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

        public JsonResult ProvinceList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestParams)
        {
            using (var context = new masterEntities())
            {

                IQueryable<Provinces> query = context.Provinces as IQueryable<Provinces>;

                int totalRecords = query.Count();

                if (false == String.IsNullOrEmpty(requestParams.Search.Value))
                {
                    query = query.Where(o => o.NameInThai.Contains(requestParams.Search.Value));
                }

                // Sorting
                query = query.OrderBy(o => o.Id);

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

                List<Provinces> Provinces = new List<Provinces>();
                Provinces = query.ToList<Provinces>();

                List<Province_List> Provinceslocals = new List<Province_List>();
                foreach (Provinces Province in Provinces)
                {
                    Province_List Provinceslocal = new Province_List(Province);
                    Provinceslocals.Add(Provinceslocal);
                }

                DataTablesResponse data = new DataTablesResponse(requestParams.Draw, Provinceslocals, recordsFiltered, totalRecords);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DistrictList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestParams, int ProvinceId = 0)
        {
            using (var context = new masterEntities())
            {

                IQueryable<Districts> query = context.Districts as IQueryable<Districts>;

                if (ProvinceId != 0)
                {
                    query = query.Where(o => o.ProvinceId == ProvinceId);
                }

                int totalRecords = query.Count();

                if (false == String.IsNullOrEmpty(requestParams.Search.Value))
                {
                    query = query.Where(o => o.NameInThai.Contains(requestParams.Search.Value));
                }

                // Sorting
                query = query.OrderBy(o => o.Id);

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

                List<Districts> Districts = new List<Districts>();
                Districts = query.ToList<Districts>();

                List<District_List> DistrictLocals = new List<District_List>();
                foreach (Districts District in Districts)
                {
                    District_List DistrictLocal = new District_List(District);
                    DistrictLocals.Add(DistrictLocal);
                }

                DataTablesResponse data = new DataTablesResponse(requestParams.Draw, DistrictLocals, recordsFiltered, totalRecords);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(buildResponse(), JsonRequestBehavior.AllowGet);
        }
    }
}