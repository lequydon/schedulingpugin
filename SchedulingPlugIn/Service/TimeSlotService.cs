using Microsoft.Ajax.Utilities;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.RepositoryLayer;
using SchedulingPlugIn.RepositoryLayer.Interface;
using SchedulingPlugIn.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using SchedulingPlugIn.Helper;
using System.Data.Entity;
using SchedulingPlugIn.Models;
using SchedulingPlugIn.Controllers;
using System.Web.Mvc;
using System.Security.Policy;
using System.Web.Routing;
using System.Net.Mime;
using System.Net.Mail;
using System.Net.Configuration;
using System.Configuration;

namespace SchedulingPlugIn.Service
{
    public class TimeSlotService :  Controller, ITimeSlotService
    {
        IDALContext context;
        
        //Controller Controller = new Controller();
        public TimeSlotService(IDALContext sl)
        {
            context = sl;
        }

        public TimeSlotService()
        {
            context = new DALContext();
        }

        public CODE_RESULT_RETURN Create(TimeSlot t)
        {
            var timeSlot = new TimeSlot { DateStart = t.DateStart, Hour = t.Hour, Minute = t.Minute, Duration = t.Duration, Note = t.Note, IDAppUser = t.IDAppUser };
            context.TimeSlots.Create(t);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }
        public CODE_RESULT_RETURN Delete(TimeSlot t)
        {
            context.TimeSlots.Delete(e => e.ID == t.ID);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }
        #region Delete session and timeslot Thinh
        public CODE_RESULT_RETURN DeleteSessionAndTimeSlot(TimeSlot t)
        {
            context.TimeSlots.Delete(e => e.ID == t.ID);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }
        #endregion
        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        public TimeSlot Get(string id)
        {
            return context.TimeSlots.Filter(e => e.ID == id).FirstOrDefault();
        }

        public List<TimeSlot> GetList()
        {
            DateTime day = Helper.Util.ConvertUTC0ToFlorida();

            var timeSlot = context.TimeSlots.All().Where(e => (e.DateStart >= day) && (e.Duration != 0) && (e.Status == TimeSlotStatus.NotYetUsed)).OrderBy(e => e.DateStart).ToList();

            return timeSlot;
        }
        public List<SessionTimeSlot> GetListSessionTimeSlot()
        {
            DateTime day = DateTime.UtcNow;
            var sessionTimeSlot = context.SessionTimeSlots.All().Where(e => e.SessionEnd > day).OrderBy(e => e.SessionStart).ToList();
            foreach (var i in sessionTimeSlot)
            {
                i.SessionStart = i.SessionStart.ConvertUTC0ToTimeZone(i.TimeZone);
                i.SessionEnd = i.SessionEnd.ConvertUTC0ToTimeZone(i.TimeZone);
            }
            return sessionTimeSlot;
        }
        public List<SessionTimeSlotViewModel> GetListDetailSession(string ID)
        {
            DateTime day = DateTime.UtcNow;
            var sessionTemp = context.SessionTimeSlots.Find(ID);
            //create array timeslot from database
            var timeSlot = context.TimeSlots.Filter(x => x.ID_SessionTimeSlot == ID).Select(e => new TimeSlotOfDayViewModel
            {
                ID_DetailTimeSlot = e.ID_SessionTimeSlot,
                DateEnd = e.DateEnd,
                DateStart = e.DateStart,
                Duration = e.Duration
            }).ToArray();
            foreach (var i in timeSlot)
            {
                i.DateEnd = i.DateEnd.ConvertUTC0ToTimeZone(sessionTemp.TimeZone);
                i.DateStart = i.DateStart.ConvertUTC0ToTimeZone(sessionTemp.TimeZone);
            }
            //create list SessionTimesLosts from database
            var SessionTimesLosts = context.SessionTimeSlots.Filter(x => x.ID == ID).Select(e => new SessionTimeSlotViewModel
            {
                ID = e.ID,
                SessionEnd = e.SessionEnd,
                SessionStart = e.SessionStart,
                SessionDuration = e.Duration,
                TimeZone = e.TimeZone
            }).ToList();
            foreach (var i in SessionTimesLosts)
            {
                i.SessionEnd = i.SessionEnd.ConvertUTC0ToTimeZone(sessionTemp.TimeZone);
                i.SessionStart = i.SessionStart.ConvertUTC0ToTimeZone(sessionTemp.TimeZone);
            }
            // Create list dateofweek type List
            List<DayOfWeekSessionTimeSlotViewModel> dateofweekList = new List<DayOfWeekSessionTimeSlotViewModel>();
            //insert timeslot to SessionTimesLosts
            for (int i = 0; i < 7; i++)
            {
                DayOfWeekSessionTimeSlotViewModel dateofweekSessionTimeSlot = new DayOfWeekSessionTimeSlotViewModel();
                var CheckExit = 0;
                List<TimeSlotOfDayViewModel> TimeSlostInDate = new List<TimeSlotOfDayViewModel>();
                //find timeSlot in dayofweek "i" 
                foreach (var l in timeSlot)
                {
                    if ((l.DateStart.DayOfWeek == (DayOfWeek)i) && CheckExit == 1)
                    {
                        TimeSlostInDate.Add(l);
                    }
                    if ((l.DateStart.DayOfWeek == (DayOfWeek)i) && CheckExit == 0)
                    {
                        CheckExit = 1;
                        dateofweekSessionTimeSlot.DayOfWeek = l.DateStart.DayOfWeek;
                        TimeSlostInDate.Add(l);
                    }

                }

                if (TimeSlostInDate.Count != 0)
                {
                    dateofweekSessionTimeSlot.TimeSlotOfDay = TimeSlostInDate.OrderBy(e=>e.DateStart).ToArray();
                    dateofweekList.Add(dateofweekSessionTimeSlot);
                }

            }

            foreach (var l in SessionTimesLosts)
            {
                l.DaysOfWeek = dateofweekList;
            }
            return SessionTimesLosts;
        }

        public CODE_RESULT_RETURN UpdateHieu(string id, DateTime date, DateTime time, int duration, string note)
        {

            DateTime dateStart = date.Date.AddHours(time.Hour).AddMinutes(time.Minute);
            DateTime dateEnd = dateStart.AddMinutes(duration);
            DateTime dateNow = DateTime.UtcNow;
            var timeslots = context.TimeSlots.All();
            var q = timeslots.AsEnumerable().Where(e => e.ID != id && ((dateStart >= e.DateStart && dateStart < e.DateEnd) || (dateEnd > e.DateStart && dateEnd <= e.DateEnd) || (e.DateStart > dateStart && e.DateEnd < dateEnd))).ToList();
            if (dateStart > dateNow.AddDays(1))
            {
                if (q == null || q.Count == 0)
                {
                    var p = context.TimeSlots.Find(id);
                    if (p != null)
                    {
                        p.DateStart = dateStart;
                        p.Duration = duration;
                        p.Note = note;
                        context.SaveChanges();
                        return CODE_RESULT_RETURN.ThanhCong;
                    }
                }
            }
            return CODE_RESULT_RETURN.ThatBai;
        }
        public CODE_RESULT_RETURN Update(TimeSlot t)
        {
            throw new NotImplementedException();
        }


        public DateTime getDateFromIDTimeSlot(string ID)
        {
            TimeSlot timeSlot = context.TimeSlots.Find(ID);
            return timeSlot.DateStart;
        }
        public List<TimeSlot> GetListNull(DateTime dateTime)
        {
            return null;
        }
        public List<TimeSlot> GetList(DateTime dateTime)
        {
            var List = context.TimeSlots.Filter(e => (e.DateStart > dateTime)/* && e.Status == TimeSlotStatus.NotYetUsed*/).OrderBy(e => e.DateStart);
            return List.ToList();
        }
        public CODE_RESULT_RETURN UpdateStatusUsed(string ID)
        {
            TimeSlot timeSlot = context.TimeSlots.Find(ID);
            timeSlot.Status = TimeSlotStatus.Used;
            context.SaveChanges();
            return Interface.CODE_RESULT_RETURN.ThanhCong;
        }
        public List<CalendarUnionTimeSlotViewModel> GetListForProspect_DetalTimeSlot(DateTime dateTime)
        {
            var listTimeslot = context.TimeSlots.All().Where(e => e.DateStart > dateTime).Select(e => new CalendarUnionTimeSlotViewModel
            {
                ID = e.ID,
                DateStart = e.DateStart,
                Duration = e.Duration,
                Note = e.Note,
            });
           
            //var listCalendar = context.Calendars.All().Where(e => (e.Date > dateTime) && e.Status == CalendarStatus.Decline  ).Select(e => new CalendarUnionTimeSlotViewModel
            //{
            //    ID = e.IDTimeSlot,
            //    DateStart = e.Date,
            //    Duration = e.Duration,
            //    Note = e.Reason,
            //});
            //listTimeslot.Union(listCalendar).GroupBy(e => e.ID).Select(d => d.FirstOrDefault());
            //listTimeslot = listTimeslot.OrderBy(e => e.DateStart );
            return listTimeslot.ToList();
        }
        public List<CalendarUnionTimeSlotViewModel> GetListForProspect_DetalTimeSlot()
        {
            DateTime day = DateTime.UtcNow;

            var listTimeslot = context.TimeSlots.All().Where(e => (e.DateStart > day)).Select(e => new CalendarUnionTimeSlotViewModel
            {
                ID = e.ID,
                DateStart = e.DateStart,
                Duration = e.Duration,
                Note = e.Note,
            });
            var listCalendar = context.Calendars.All().Where(e => (e.Date > day) && e.Status == CalendarStatus.Decline).Select(e => new CalendarUnionTimeSlotViewModel
            {
                ID = e.IDTimeSlot,
                DateStart = e.Date,
                Duration = e.Duration,
                Note = e.Reason,
            });
            listTimeslot.Union(listCalendar).GroupBy(e => e.ID).Select(d => d.FirstOrDefault());
            listTimeslot = listTimeslot.OrderBy(e => e.DateStart);
            return listTimeslot.ToList();
        }
        public CODE_RESULT_RETURN UpdateStatusUsedTimeSlot(string id, TimeSlotStatus status)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var ts = db.TimeSlotDbSet.Where(e => e.ID == id).FirstOrDefault();
            //var save = context.Calendars.Find(e => e.ID == id);
            if (ts != null && ts.Status == TimeSlotStatus.NotYetUsed)
            {
                ts.Status = status;
                db.SaveChanges();
                return CODE_RESULT_RETURN.ThanhCong;
            }
            return CODE_RESULT_RETURN.ThatBai;
        }
        public CODE_RESULT_RETURN CheckStatusUsedTimeSlot(string id)
        {
            TimeSlot timeSlot = context.TimeSlots.Find(id);
            if (timeSlot.Status == TimeSlotStatus.Used) return CODE_RESULT_RETURN.ThanhCong;
            return CODE_RESULT_RETURN.ThatBai;
        }
        public CODE_RESULT_RETURN CheckTimeCreateTimeSlot(DateTime dateStart, DateTime dateEnd)
        {
            DateTime dateNow = DateTime.UtcNow;
            if (dateStart > dateNow.AddDays(1))
            {

                var timeslots = context.TimeSlots.All();
                var q = timeslots.Where(e => (dateStart >= e.DateStart && dateStart < e.DateEnd) || (dateEnd > e.DateStart && dateEnd <= e.DateEnd) || (e.DateStart > dateStart && e.DateEnd < dateEnd)).ToList();

                if (q == null || q.Count == 0)
                {
                    return CODE_RESULT_RETURN.ThanhCong;
                }
            }
            return CODE_RESULT_RETURN.ThatBai;
        }
        public CODE_RESULT_RETURN UpdateSessionandTimeSlot(SessionTimeSlotViewModel sessionTimeSlot, string ID_session, int Checked)
        {
            DateTime Now = DateTime.UtcNow;
            ApplicationDbContext db = new ApplicationDbContext();
            SessionTimeSlot sTimeSlot = new SessionTimeSlot();
            sTimeSlot = context.SessionTimeSlots.Find(ID_session);
            sTimeSlot.Duration = sessionTimeSlot.SessionDuration;
            List<Calendar> calendars = new List<Calendar>();
            string urlRedirect = null;
            List<TimeSlot> timeSlots = db.TimeSlotDbSet.Where(e => e.ID_SessionTimeSlot == sTimeSlot.ID).ToList();
            List<TimeSlot> Temp = new List<TimeSlot>().ToList();
            List<String> distinct = null;
            //So sanh DB voi obj timeslot truyen vao
            List<TimeSlot> timeSlotsTemp = new List<TimeSlot>();

            foreach (var k in timeSlots)
            {
                foreach (var i in sessionTimeSlot.DaysOfWeek)
                {
                    bool check = false;
                    foreach (var j in i.TimeSlotOfDay)
                    {
                        if (k.ID == j.ID_DetailTimeSlot)
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        var item = timeSlots.Where(x => x.ID == k.ID).FirstOrDefault();
                        timeSlotsTemp.Add(item);
                    }
                }
            }
            foreach (var j in timeSlotsTemp)
            {
                var calendar = db.CalendarDbSet.Where(e => e.IDTimeSlot == j.ID).ToList();
                calendars.AddRange(calendar);
                if (calendars.Count > 1)
                {
                    distinct = calendars.Select(e => e.IDCustomer).Distinct().ToList();
                }
                if (calendars.Count > 1)
                {
                    distinct = calendars.Select(e => e.IDCustomer).ToList();
                }
                timeSlots.Remove(j);
            };
            if (distinct != null)
            {
                foreach(var i in distinct)
                {
                    Customer customers = db.CustomerDbSet.Where(e => e.ID == i).FirstOrDefault();
                    List<Calendar> calendarFromCustomer = calendars.Where(e => e.IDCustomer == i).ToList();
                    List<Calendar> calendarFromCustomerConfirm = calendars.Where(e => e.IDCustomer == i && e.Status != CalendarStatus.Confirmed).ToList();
                    List<CalendarPooling> findpooling = new List<CalendarPooling>();
                    Uri requestUrl = HttpContext.Request.Url;
                    var protocol = Uri.UriSchemeHttp;
                    var hostName = requestUrl.Host;
                    urlRedirect = Url.Action("RedirectLink", "Admin", new RouteValueDictionary() { { "iDpage", calendarFromCustomer[0].IDPageRequest } }, protocol, hostName);
                    if (Checked == 1)
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
                foreach (var i in timeSlotsTemp)
                {
                    calendars = db.CalendarDbSet.Where(e => e.IDTimeSlot == i.ID).ToList();
                    foreach (var cal in calendars)
                    {
                        db.CalendarDbSet.Remove(cal);
                    }
                }
            }
            // Duyet obj timeslot tu tham so truyen vao so sanh voi db
            // Create New timeslot
            foreach (var i in sessionTimeSlot.DaysOfWeek)
            {
                foreach (var j in i.TimeSlotOfDay)
                {
                    for (DateTime d = sTimeSlot.SessionStart; d <= sTimeSlot.SessionEnd; d = d.AddDays(1))
                    {
                        if (i.DayOfWeek == d.DayOfWeek) //sai thi sua doan nay
                        {
                            var check = timeSlots.AsEnumerable().Where(e => e.ID == j.ID_DetailTimeSlot).ToList();
                            if (check == null || check.Count == 0)
                            {
                                TimeSlot timeSlot = new TimeSlot();
                                timeSlot.ID = Guid.NewGuid().ToString();
                                timeSlot.ID_SessionTimeSlot = sTimeSlot.ID;
                                timeSlot.DateStart = j.DateStart.ConvertTimeZoneToUTC0(sessionTimeSlot.TimeZone);
                                timeSlot.DateEnd = j.DateEnd.ConvertTimeZoneToUTC0(sessionTimeSlot.TimeZone);
                                timeSlot.Duration = j.Duration == -1 ? sessionTimeSlot.SessionDuration : j.Duration;
                                if (CheckTimeCreateTimeSlot(timeSlot.DateStart, timeSlot.DateEnd) == Service.Interface.CODE_RESULT_RETURN.ThatBai) return CODE_RESULT_RETURN.ThatBai;
                                db.TimeSlotDbSet.Add(timeSlot);
                            }
                            else
                            {
                                var search = context.TimeSlots.Find(j.ID_DetailTimeSlot);
                                search.DateEnd = j.DateEnd.ConvertTimeZoneToUTC0(sessionTimeSlot.TimeZone);
                                search.DateStart = j.DateStart.ConvertTimeZoneToUTC0(sessionTimeSlot.TimeZone);
                                search.Duration = j.Duration == -1 ? sessionTimeSlot.SessionDuration : j.Duration;
                                if (CheckTimeCreateTimeSlot(search.DateStart, search.DateEnd) == Service.Interface.CODE_RESULT_RETURN.ThatBai) return CODE_RESULT_RETURN.ThatBai;
                            }
                        }
                    }
                }
            }
            context.SaveChanges();
            db.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }
        #region Send Mail when delete session 
        CalendarService service = new CalendarService();
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
    }
}