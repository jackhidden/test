using Logistic.Controllers.Model;
using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Logistic.Controllers
{
    public class AccountController : Controller
    {
        private String Status = "fail";
        private List<Object> Data = new List<Object>();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

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

        public ActionResult Login(Req_User_Login req)
        {
            using (var context = new masterEntities())
            {
                UserProfile user = context.UserProfile.Where(o => o.Username == req.Username && o.Password == req.Password && o.IsDelete == false).FirstOrDefault();
                if (user == null)
                {
                    Status = "fail";
                    //return Json(buildResponse(), JsonRequestBehavior.DenyGet);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { id = user.UserId });
                }

                
            }
        }
    }
}