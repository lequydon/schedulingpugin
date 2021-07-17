using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedulingPlugIn.Controllers
{
    [Authorize]
    [HandleError]
    public class CustomerController : BaseController
    {
       
        CustomerService customerservice = new CustomerService();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListCustomer()
        {
            var lscustomer = customerservice.GetList();
            return Json(lscustomer, JsonRequestBehavior.AllowGet);
        }
        public ActionResult listcustomerid( string id)
        {
            var customer = customerservice.Get(id);
            return Json(customer, JsonRequestBehavior.AllowGet);
        }
        public int create( Customer t )
        {
            var create = customerservice.Create(t);
            return 1;
        }
        public int update( Customer t)
        {
            var up = customerservice.Update(t);
            return 1;
        }
        public int delete(Customer dele)
        {
            var delete = customerservice.Delete(dele);
            return 1;
        }
    }
}