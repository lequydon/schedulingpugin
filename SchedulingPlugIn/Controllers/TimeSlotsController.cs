using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.Service;

namespace SchedulingPlugIn.Controllers
{
    [HandleError]
    public class TimeSlotsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        TimeSlotService TimeslotSV = new TimeSlotService();
        CalendarService service = new CalendarService();
        AdminController adminMail = new AdminController();
        // GET: TimeSlots
        //[Authorize]
        public ActionResult Index()
        {
            return View(db.TimeSlotDbSet.ToList());
        }

        // GET: TimeSlots/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeSlot timeSlot = db.TimeSlotDbSet.Find(id);
            if (timeSlot == null)
            {
                return HttpNotFound();
            }
            return View(timeSlot);
        }
        public JsonResult GetListDetail(string ID)
        {
            return Json(TimeslotSV.GetListDetailSession(ID), JsonRequestBehavior.AllowGet);
        }
        // GET: TimeSlots/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Available()
        {
            return View();
        }

        // POST: TimeSlots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateStart,Hour,Minute,Duration,Note,IDAppUser")] TimeSlot timeSlot)
        {
            if (ModelState.IsValid)
            {
                timeSlot.ID = Guid.NewGuid().ToString();
                db.TimeSlotDbSet.Add(timeSlot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(timeSlot);
        }

        // GET: TimeSlots/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeSlot timeSlot = db.TimeSlotDbSet.Find(id);
            if (timeSlot == null)
            {
                return HttpNotFound();
            }
            return View(timeSlot);
        }

        // POST: TimeSlots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTimeSlot([Bind(Include = "ID,DateStart,Hour,Minute,Duration,Note,IDAppUser")] TimeSlot timeSlot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeSlot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timeSlot);
        }
        [HttpPost]
        public int EditTimeSlotHieu(string id, DateTime date, DateTime time, int duration, string note)
        {
            var ts = TimeslotSV.UpdateHieu(id, date, time, duration, note);
            return (int)ts;
        }

        // GET: TimeSlots/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeSlot timeSlot = db.TimeSlotDbSet.Find(id);
            if (timeSlot == null)
            {
                return HttpNotFound();
            }
            return View(timeSlot);
        }

        // POST: TimeSlots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TimeSlot timeSlot = db.TimeSlotDbSet.Find(id);
            db.TimeSlotDbSet.Remove(timeSlot);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        #region Getlist all sessiontimeslot manage page Thinh
        public ActionResult GetListSesionTimeSlot()
        {
            var getList = TimeslotSV.GetListSessionTimeSlot();

            return Json(getList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete SessionTimeSlot Thinh
        public ActionResult DeleteSessionTimeSlot(string ID, int check)
        {
            SessionTimeSlot sessionTimeSlot = db.SessionTimeSlotDbSet.Find(ID);
            List<TimeSlot> timeSlot = db.TimeSlotDbSet.Where(e => e.ID_SessionTimeSlot == ID).ToList();

            List<Calendar> calendars = new List<Calendar>();
            List<String> distinct = null;
            db.SessionTimeSlotDbSet.Remove(sessionTimeSlot);
            string urlRedirect = null;

            foreach (var i in timeSlot)
            {
                var tempCalendars = db.CalendarDbSet.Where(e => e.IDTimeSlot == i.ID).ToList();
                // add List to list use AddRange
                calendars.AddRange(tempCalendars);
                db.TimeSlotDbSet.Remove(i);
            }

            if (calendars.Count > 1)
            {
                distinct = calendars.Select(e => e.IDCustomer).Distinct().ToList(); ;
            }
            if (calendars.Count == 1)
            {
                distinct = calendars.Select(e => e.IDCustomer).ToList();
            }
            if (distinct != null)
            {
                foreach (var i in distinct)
                {
                    Customer customers = db.CustomerDbSet.Where(e => e.ID == i).FirstOrDefault();
                    List<Calendar> calendarFromCustomer = calendars.Where(e => e.IDCustomer == i).ToList();
                    List<Calendar> calendarFromCustomerConfirm = calendars.Where(e => e.IDCustomer == i && e.Status != CalendarStatus.Confirmed).ToList();
                    List<CalendarPooling> findpooling = new List<CalendarPooling>();
                    Uri requestUrl = HttpContext.Request.Url;
                    var protocol = Uri.UriSchemeHttp;
                    var hostName = requestUrl.Host;
                    urlRedirect = Url.Action("RedirectLink", "Admin", new RouteValueDictionary() { { "iDpage", calendarFromCustomer[0].IDPageRequest } }, protocol, hostName);
                    if (check == 1)
                    {
                        sendMailDecline(calendarFromCustomer, customers, urlRedirect);
                        foreach (var j in calendarFromCustomer)
                        {
                            findpooling.AddRange(db.CalendarPoolingDbSet.Where(e => e.IDCalendar == j.ID).ToList());

                            db.CalendarDbSet.Remove(j);
                        }
                        foreach (var j in findpooling)
                        {
                            db.CalendarPoolingDbSet.Remove(j);
                        }
                    }
                    else
                    {
                        sendMailDecline(calendarFromCustomerConfirm, customers, urlRedirect);
                        foreach (var j in calendarFromCustomerConfirm)
                        {
                            findpooling.AddRange(db.CalendarPoolingDbSet.Where(e => e.IDCalendar == j.ID).ToList());

                            db.CalendarDbSet.Remove(j);
                        }
                        foreach (var j in findpooling)
                        {
                            db.CalendarPoolingDbSet.Remove(j);
                        }

                    }
                }
            }
            else
            {
                foreach (var i in timeSlot)
                {
                    calendars = db.CalendarDbSet.Where(e => e.IDTimeSlot == i.ID).ToList();
                    foreach (var cal in calendars)
                    {
                        db.CalendarDbSet.Remove(cal);
                    }
                }
            }
            //foreach (var i in timeSlot)
            //{
            //    
            //    {

            //        if (check == 1 || (check == 0 && cal.Status != CalendarStatus.Confirmed))
            //        {
            //            db.CalendarDbSet.Remove(cal);
            //        }
            //    }
            //    //

            //}

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        #region UpdateSessionTimeSlot Thinh
        public int UpdateSessionTimeSlot(SessionTimeSlotViewModel sessionTimeSlot, string ID_session,int check)
        {
            var Update = TimeslotSV.UpdateSessionandTimeSlot(sessionTimeSlot, ID_session,check);
            return (int)Update;
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


        #region Send Mail when delete session 
        string getEmailDecline(List<Calendar> calendar, Customer customer, string linkProspect)
        {
            // return Hostname

            PageRequest pageRequest = service.GetInfoPageRequest(calendar[0].IDPageRequest);

            string url = null;
            string backgroundimg = null;
            string colorbackground = null;
            if (calendar[0].IDPageRequest == "JouleBroker")
            {
                url = "https://i.ibb.co/8Mqx0YM/Joule-Broker-Mail-header.png";
                backgroundimg = "https://i.ibb.co/y0PKkmF/Joule-Backround.png";
                colorbackground = "#242425";
            }

            if (calendar[0].IDPageRequest == "MGISolutions")
            {
                url = "https://i.ibb.co/j4MxMdb/MGI-Solutions-header.png";
                backgroundimg = "https://i.ibb.co/31J0RnN/MGIBackground.png";
                colorbackground = "#208dec";
            }

            string strDecline = System.IO.File.ReadAllText(Server.MapPath(@"~/Views/Email_Calendar/EmailDecline.html"));
            //strContents = strContents.Replace("{Date}", calendar.Date.ToString("MM/dd/yyyy HH:mm"));
            List<DateTime> dateTime = new List<DateTime>();
            List<DateTime> dateTimeEnd = new List<DateTime>();

            foreach (var i in calendar)
            {
                dateTime.Add(Helper.Util.ConvertUTC0ToTimeZone(i.Date, i.TimeZone));
                dateTimeEnd.Add(Helper.Util.ConvertUTC0ToTimeZone(i.DateEnd, i.TimeZone));
                strDecline = strDecline.Replace("{reason}", i.Reason);
            }
            string htmlForDateTime = null;
            for (var i = 0; i <= dateTime.Count; i++)
            {
                htmlForDateTime = htmlForDateTime + "<br>" + dateTime[i].ToString("ddd, MMMM dd yyyy ", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) + " " + dateTime[i].ToString("HH:mm") + " - " + dateTimeEnd[i].ToString("HH:mm");
                //strDecline = strDecline.Replace("{datemeeting}", dateTime[i].ToString("dddd, MMMM dd yyyy HH:mm", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
            }
            strDecline = strDecline.Replace("{timemeeting}", htmlForDateTime);
            strDecline = strDecline.Replace("{logo}", url);
            strDecline = strDecline.Replace("{background}", backgroundimg);
            strDecline = strDecline.Replace("{namepagerequest}", pageRequest.Name);
            strDecline = strDecline.Replace("{email}", customer.Email);
            strDecline = strDecline.Replace("{firstName}", customer.FirstName);
            strDecline = strDecline.Replace("{lastName}", customer.LastName);
            strDecline = strDecline.Replace("{linkProspect}", linkProspect);
            strDecline = strDecline.Replace("{colorbackground}", colorbackground);
            return strDecline;
        }
        void sendMailDecline(List<Calendar> calendar, Customer customer, string linkProspect)
        {
            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string from = section.From;
            string host = section.Network.Host;
            int port = section.Network.Port;
            bool enableSsl = section.Network.EnableSsl;
            string user = section.Network.UserName;
            string password = section.Network.Password;
            PageRequest pageRequest = service.GetInfoPageRequest(calendar[0].IDPageRequest);
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from, pageRequest.Name + " Contact Center");
            msg.To.Add(new MailAddress(customer.Email));
            msg.Subject = "Consultation Cancellation - " + pageRequest.Name;
            //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("", null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(getEmailDecline(calendar, customer, linkProspect), null, MediaTypeNames.Text.Html));
            SmtpClient smtpClient = new SmtpClient(host, port);
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(from, password);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = enableSsl;
            smtpClient.Send(msg);
        }
        #endregion


        #region CustomerCalendarViewModel
        public class CustomerCalendarViewModel
        {
            public string IDCalendar { get; set; }
            public string IDCustomer { get; set; }
            public List<Calendar> ListCalendar { get; set; }
            public Customer CustomerVM { get; set; }
        }
        #endregion
    }
}
