using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedulingPlugIn.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            throw new Exception("Server500"); ;
        }
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult NotFound403()
        {
            return View();
        }
        public ActionResult Server500()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}