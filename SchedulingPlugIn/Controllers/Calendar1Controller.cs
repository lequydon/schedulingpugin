using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchedulingPlugIn.Helper;
using System.Dynamic;
using SchedulingPlugIn.Service.Interface;

namespace SchedulingPlugIn.Controllers
{
    [HandleError]
    public partial class CalendarController : BaseController
    {
        // GET: Calendar
        TimeSlotService tss = new TimeSlotService();
        [Authorize]
        public ActionResult Detail()
        {
            ViewBag.Action = "Detail";
            return View();
        }
        [Authorize]
        [HttpGet]
        [Authorize]
        public ActionResult CreateTimeSlot()
        {
            return View();
        }

        //[Authorize]
        public ActionResult CreateAvailableTime()
        {
            return View();
        }
        #region CreateTimeSLot_Hieu Closed
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public ActionResult CreateTimeSlot([Bind(Include = "DateStart,Duration,Note")] TimeSlot timeSlot, [Bind(Include = "DateStart,DateEnd,Note")] SessionTimeSlot sessionTimeSlot)
        //{
        //    //Get input SessionTimeSlot
        //    DateTime dateSessionStart = DateTime.Parse(Request.Form["SessionStart"]);
        //    DateTime dateSessionEnd = DateTime.Parse(Request.Form["SessionEnd"]);
        //    int durationSession = int.Parse(Request.Form["DurationSession"]);
        //    TimeSpan timeSpanSession = dateSessionEnd - dateSessionStart;
        //    int spaceDays = timeSpanSession.Days;
        //    for (var i = 0; i<=spaceDays;i++)
        //    {

        //    }
        //    DateTime date = DateTime.Parse(Request.Form["dateStart"]);
        //    DateTime timeStart = DateTime.Parse(Request.Form["time"]);
        //    int duration = int.Parse(Request.Form["duration"]);
        //    string note = Request.Form["note"];
        //    string timezone = Request.Form["timeZone"];
        //    ViewBag.Success = "0";
        //    DateTime dateStart = date.Date.AddHours(timeStart.Hour).AddMinutes(timeStart.Minute);
        //    //dateStart = dateStart.ConvertTimeZoneToUTC0(timezone).ConvertUTC0ToTimeZone("Eastern Standard Time");
        //    if (ModelState.IsValid)
        //    {
        //        DateTime dateNow = Util.ConvertUTC0ToFlorida();
        //        if(dateStart > dateNow.AddDays(1))
        //        {
        //            DateTime dateEnd = dateStart.AddMinutes(duration);
        //            var timeslots = db.TimeSlotDbSet.ToList();
        //            var q = timeslots.Where(e => (dateStart >= e.DateStart && dateStart < e.DateEnd) || (dateEnd > e.DateStart && dateEnd <= e.DateEnd) || (e.DateStart > dateStart && e.DateEnd < dateEnd)).ToList();

        //            if (q == null || q.Count == 0)
        //            {
        //                timeSlot.ID = Guid.NewGuid().ToString();
        //                timeSlot.DateStart = dateStart;
        //                timeSlot.Duration = duration;
        //                timeSlot.Note = note;
        //                db.TimeSlotDbSet.Add(timeSlot);
        //                db.SaveChanges();
        //                //ViewBag.Success = "1";
        //                return RedirectToAction("Detail");/* View();*/
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "The time is in conflict with the time previous event!");
        //                ViewBag.Success = "0";
        //            }
        //        }
        //        else
        //            ModelState.AddModelError("", "Please, You only can create a free time slot more than 24 hours from this moment");

        //    }
        //    return View(timeSlot);
        //}
        #endregion


        #region CreateAvailableTimeSlot_Thinh New
        [HttpPost]
        public int CreateAvailableTimeSlot(SessionTimeSlotViewModel sessionTimeSlot)
        {
            //create session
            SessionTimeSlot sTimeSlot = new SessionTimeSlot();
            TimeZone timezone = TimeZone.CurrentTimeZone;
            TimeSpan currentOffset = timezone.GetUtcOffset(sessionTimeSlot.SessionStart);
            string timeZone = "UTC" + currentOffset.Hours;
            //sessionTimeSlot.TimeZone = timeZone;
            sTimeSlot.SessionStart = sessionTimeSlot.SessionStart;
            sTimeSlot.SessionEnd = sessionTimeSlot.SessionEnd;

            sTimeSlot.ID = Guid.NewGuid().ToString();
            sTimeSlot.TimeZone = sessionTimeSlot.TimeZone;
            //sTimeSlot.SessionStart = sessionTimeSlot.SessionStart;
            //sTimeSlot.SessionEnd = sessionTimeSlot.SessionEnd;
            TimeSpan timeSpan = sTimeSlot.SessionEnd - sTimeSlot.SessionStart;
            int rangeTime = timeSpan.Days;
            sTimeSlot.Duration = sessionTimeSlot.SessionDuration;
            


            if (sessionTimeSlot.DaysOfWeek != null) { 
            //create TimeSlot from DayofWeek
                foreach (var i in sessionTimeSlot.DaysOfWeek)
                {
                    foreach(var j in i.TimeSlotOfDay)
                    {
                        DateTime d = sTimeSlot.SessionStart;
                        for (; d <= sTimeSlot.SessionEnd; d=d.AddDays(1))
                        {
                            if(i.DayOfWeek == d.DayOfWeek) //sai thi sua doan nay
                            {
                                
                                sTimeSlot.SessionStart = sessionTimeSlot.SessionStart.ConvertTimeZoneToUTC0(sessionTimeSlot.TimeZone);
                                sTimeSlot.SessionEnd = sessionTimeSlot.SessionEnd.ConvertTimeZoneToUTC0(sessionTimeSlot.TimeZone);
                                TimeSlot timeSlot = new TimeSlot();
                                timeSlot.ID = Guid.NewGuid().ToString();
                                timeSlot.ID_SessionTimeSlot = sTimeSlot.ID;
                                timeSlot.DateStart = j.DateStart.ConvertTimeZoneToUTC0(sessionTimeSlot.TimeZone);
                                timeSlot.DateStart = new DateTime(d.Year, d.Month, d.Day, timeSlot.DateStart.Hour, timeSlot.DateStart.Minute, 0);
                                timeSlot.DateEnd = j.DateEnd.ConvertTimeZoneToUTC0(sessionTimeSlot.TimeZone);
                                timeSlot.DateEnd = new DateTime(d.Year, d.Month, d.Day, timeSlot.DateEnd.Hour, timeSlot.DateEnd.Minute, 0);
                                timeSlot.Duration = j.Duration == -1?sessionTimeSlot.SessionDuration : j.Duration ;
                                if (tss.CheckTimeCreateTimeSlot(timeSlot.DateStart, timeSlot.DateEnd) == Service.Interface.CODE_RESULT_RETURN.ThatBai) return (int)CODE_RESULT_RETURN.ThatBai;
                                db.TimeSlotDbSet.Add(timeSlot);
                            
                            }
                        }
                    }
                }
            }else
            {
                return (int)CODE_RESULT_RETURN.ThatBai;
            }
            db.SessionTimeSlotDbSet.Add(sTimeSlot);
            db.SaveChanges();
            return (int)CODE_RESULT_RETURN.ThanhCong;
        }

        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public int Create(TimeSlot t)
        {
            tss.Create(t);
            return 1;
        }
        
        [HttpPost]
        public int Delete(TimeSlot t)
        {
            var delete = tss.Delete(t);
            return (int)delete;
        }

        public JsonResult ListEvent(string id)
        {
            var get = tss.Get(id);
            return Json(get, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetListEvent()
        {
            var getList = tss.GetList();
            foreach (var i in getList)
            {
                i.DateStart = i.DateStart.ConvertTimeZoneFromFloridaToUTC0();
            }
            return Json(getList, JsonRequestBehavior.AllowGet);
        }   
        public JsonResult getData(DateTime date)
        {
            //date = Util.ConvertTimeZoneToTampaFlorida(date, TimeZoneInfo.Utc.StandardName);
            var dates = db.TimeSlotDbSet.Where(e => (DbFunctions.TruncateTime(e.DateStart) == DbFunctions.TruncateTime(date)) && e.Status==TimeSlotStatus.NotYetUsed).OrderBy(e=>e.DateStart).ToList();
            foreach (var i in dates)
            {
                i.DateStart = i.DateStart.ConvertTimeZoneFromFloridaToUTC0();
            }
            return Json(dates, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListForProspect_DetalTimeSlot()
        {
            var getList = tss.GetListForProspect_DetalTimeSlot();
            foreach (var i in getList)
            {
                i.DateStart = i.DateStart.ConvertTimeZoneFromFloridaToUTC0();
            }
            return Json(getList, JsonRequestBehavior.AllowGet);
        }
        
    }
    #region ViewModelSessionTimeSlotViewModel
    public class SessionTimeSlotViewModel
    {
        public string ID { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }
        public int SessionDuration { get; set; }
        public string TimeZone { get; set; }
        public List<DayOfWeekSessionTimeSlotViewModel> DaysOfWeek { get; set; }
    }
    public class DayOfWeekSessionTimeSlotViewModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSlotOfDayViewModel[] TimeSlotOfDay { get; set; }
    }
    public class TimeSlotOfDayViewModel
    {
        public string ID_DetailTimeSlot { get; set; }
        public int Duration { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
    #endregion
}