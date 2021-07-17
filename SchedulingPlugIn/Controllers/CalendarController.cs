using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.Service;
using SchedulingPlugIn.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchedulingPlugIn.Helper;

namespace SchedulingPlugIn.Controllers
{
    [HandleError]
    public partial class CalendarController : BaseController
    {
        private ApplicationDbContext db;
        CalendarPoolingService calendarPoolingSv = new CalendarPoolingService();
        CalendarService calendarSv = new CalendarService();
        public CalendarController()
        {
            db = new ApplicationDbContext();
        }
        [Authorize]
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Submited()
        {
            return View();
        }

        public ActionResult Create(Calendar t)
        {
            calendarSv.Create(t);
            return View();
        }
        public ActionResult delete(Calendar t)
        {
            calendarSv.Delete(t);
            return View();
        }
        public ActionResult update(Calendar t)
        {
            calendarSv.Update(t);
            return View();
        }
        
        public JsonResult List(string id)
        {
            var get = calendarPoolingSv.Get(id);
            return Json(get, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList()
        {
            var getList = calendarSv.GetListCalendarViewModelsT();
                DateTime date = Util.ConvertUTC0ToFlorida();
            foreach (var i in getList)
            {
                
                if (i.Status == CalendarStatus.Confirmed)
                {
                    if (date >= i.DateEnd)
                    {
                        i.makeColor = 1;
                    }
                    else if (date >= i.Date && date <= i.DateEnd)
                    {
                        i.makeColor = 3;
                    }
                    else
                    {
                        i.makeColor = 2;
                    }
                }else
                {
                    i.makeColor = 4;
                }
                i.Date = i.Date.ConvertTimeZoneFromFloridaToUTC0();

            }

            return Json(getList, JsonRequestBehavior.AllowGet);
        }
    }
}