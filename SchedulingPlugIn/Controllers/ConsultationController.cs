using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchedulingPlugIn.Controllers
{
    [HandleError]
    public class ConsultationController : BaseController
    {
        CalendarService calendarservice = new CalendarService();
        CustomerService customerservice = new CustomerService();
        [Authorize]
        public ActionResult Index()
        {
            
            return View();
        }
        //public ActionResult ListConsultation()
        //{
        //    var lsConsultation = ConsultationViewModels.GetList();

        //    return Json(lsConsultation, JsonRequestBehavior.AllowGet);
        //} 

        public ActionResult Listconsultation()
        {
            var lsconsultation = calendarservice.GetListConsultation();
            return Json(lsconsultation, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detailid( string id)
        {
            var detail = customerservice.Get(id);
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        public int Delete( string id)
        {
            var delete = calendarservice.DeleteConsultation(id);
            return 1;
        }
        public ActionResult ListApproved( int status)
        {
            var lsApproved = calendarservice.GetListApproved((CalendarStatus)status);
            return Json(lsApproved, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Expired()
        {
            var lsExpired = calendarservice.GetDashBoardexpired();
            return Json(lsExpired, JsonRequestBehavior.AllowGet);
        }

        public void Status( string id , int status)
        {
            var St = calendarservice.Approvedd(id, (CalendarStatus)status);
        }
    }
}