using SchedulingPlugIn.Helper;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SchedulingPlugIn.Controllers
{

    [HandleError]
    public class AdminController : BaseController
    {
        
        // GET: Admin
        CalendarService service = new CalendarService();
        [Authorize(Roles = "Administrator")]
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListAswer(string id)
        {
            var ls = service.Answer(id);
            return Json(ls, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PopUpContact(string id)
        {
            var ls = service.ContactPopup(id);
            return Json(ls, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListContact()
        {
            var ls = service.ListContact();
            return Json(ls, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListTimeCalendar()
        {
            var ls = service.GetTimeCalendar();
            return Json(ls, JsonRequestBehavior.AllowGet);
        }
        public ActionResult gettimeforanalysis()
        {
            var ls = service.gettimeforanalysis();
            return Json(ls, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(int number)
        {

            if (number == 0)
            {
                
                var ls = service.GetListDashBoard(Models.Entity.CalendarStatus.WaitConfirm);
                return Json(ls, JsonRequestBehavior.AllowGet);
            }
            if(number== 3)
            {
                var ls = service.GetListDashBoardexpired();
                return Json(ls, JsonRequestBehavior.AllowGet);
            }
            var ls1 = service.GetListDashBoard(Models.Entity.CalendarStatus.Confirmed);
            return Json(ls1, JsonRequestBehavior.AllowGet);
        }
        static List<Calendar> lscalendarcoincidence = new List<Calendar>();
        public void send(string idCalendar, int status, string reason)
        {
            var customer = service.getCustomerFromIDCalendar(idCalendar);
            if ((CalendarStatus)status == CalendarStatus.Confirmed)
            {
               // List<Calendar> lscalendarcoincidence = new List<Calendar>();
              //  lscalendarcoincidence = service.IdenticalCheckTimesend(idCalendar);
                foreach (var i in lscalendarcoincidence)
                {
                    if (i.ID != idCalendar)
                    {
                        
                        //var pageRequest = service.getPageCodeFromIDCalendar(idCalendar);
                       
                        //Uri requestUrl = HttpContext.Request.Url;
                        //var protocol = Uri.UriSchemeHttp;
                        //var hostName = requestUrl.Host;
                        //var url = Url.Action("Index", "Prospect", null, protocol, hostName);
                        //url += "/Index/" + pageRequest.PageCode;
                        //sendMailDecline(i, customercoincidence, url);
                        var customercoincidence = service.getCustomerFromIDCalendar(i.ID);
                        var calendar = service.GetCalendar(idCalendar);
                        Uri requestUrl = HttpContext.Request.Url;
                        var protocol = Uri.UriSchemeHttp;
                        var hostName = requestUrl.Host;
                        var urlRedirect = Url.Action("RedirectLink", "Admin", new RouteValueDictionary() { { "iDpage", calendar.IDPageRequest } }, protocol, hostName);
                        sendMailDecline(calendar, customer, urlRedirect);
                    }
                }
                // service.UpdateStatusActive(idCalendar);
                var calendarsend = service.GetCalendar(idCalendar);
                sendMail(customer, calendarsend);
            }

            if ((CalendarStatus)status == CalendarStatus.Decline)
            {
                
                //var pageRequest = service.getPageCodeFromIDCalendar(idCalendar);
                var calendar = service.GetCalendar(idCalendar);
               // Uri requestUrl = HttpContext.Request.Url;
                //var protocol = Uri.UriSchemeHttp;
                //var hostName = requestUrl.Host;
                //var url = Url.Action("Index", "Prospect", null, protocol, hostName);
                //url += "/Index/" + pageRequest.PageCode;
                //url += "/Index";
                Uri requestUrl = HttpContext.Request.Url;
                var protocol = Uri.UriSchemeHttp;
                var hostName = requestUrl.Host;
                var urlRedirect = Url.Action("RedirectLink", "Admin", new RouteValueDictionary() { { "iDpage", calendar.IDPageRequest } }, protocol, hostName);
                sendMailDecline(calendar, customer, urlRedirect);
            }
        }
        public ActionResult RedirectLink(string iDpage)
        {
            var page = service.GetPageCode(iDpage);
            //Uri requestUrl = HttpContext.Request.Url;
            //var protocol = Uri.UriSchemeHttp;
            //var hostName = requestUrl.Host;
            //var url = Url.Action("Index", "Prospect", null, protocol, hostName);
            //url += "/Index/" + page.PageCode; //"Prospect/Index/" + page.PageCode
            return RedirectToAction("Index/"+ page.PageCode + "", "Prospect");
        }
        public  int UpdateResson(string idCalendar, int status, string reason)
        {

            if (service.UpdateReason(idCalendar, status, reason) == Service.Interface.CODE_RESULT_RETURN.ThanhCong)
            {
                lscalendarcoincidence = null;
                if ((CalendarStatus)status == CalendarStatus.Confirmed)
                    lscalendarcoincidence=service.IdenticalCheckTime(idCalendar);
                return 1;
            }
            else
                return 0;
        }
        
        //public List<Calendar> update(string id)
        //{
        //    return service.IdenticalCheckTime(id);
        //}
      
        void sendMail(Customer customer,Calendar calendar)
        {
            PageRequest pageRequest = service.GetInfoPageRequest(calendar.IDPageRequest);
            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string from = section.From;
            string host = section.Network.Host;
            int port = section.Network.Port;
            bool enableSsl = section.Network.EnableSsl;
            string user = section.Network.UserName;
            string password = section.Network.Password;
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from, pageRequest.Name + "Contact Center");
            msg.To.Add(new MailAddress(customer.Email));
           
            msg.Subject = "Consultation Request Approved - " + pageRequest.Name;
            DateTime date_start = Helper.Util.ConvertTampaFloridaToTimeZone(calendar.Date, calendar.TimeZone);
            DateTime date_end = date_start.AddMinutes(calendar.Duration);
            
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("", null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(getContentEmail(customer, calendar), null, MediaTypeNames.Text.Html));
            //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(Util.CreateICSFile(from, date_start, date_end, null), "event.ics", "text/calendar");
            //message.Attachments.Add(attachment);
            //msg.Attachments.Add(attachment);
            //////////////////////////////////////////
            System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType("text/calendar");
            contype.Parameters.Add("method", "REQUEST");
            contype.Parameters.Add("name", "Meeting.ics");
            AlternateView avCal = AlternateView.CreateAlternateViewFromString(Util.CreateICSFile(from, date_start, date_end, null), contype);
             msg.AlternateViews.Add(avCal);
            /////////////////////////////////////////
            SmtpClient smtpClient = new SmtpClient(host, port);
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(from, password);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = enableSsl;
            smtpClient.Send(msg);
        }
        string getContentEmail(Customer customer,Calendar calendar)
        {
            PageRequest pageRequest = service.GetInfoPageRequest(calendar.IDPageRequest);

            string url = null;
            string backgroundimg = null;
            string colorbackground = null;
            if (calendar.IDPageRequest == "JouleBroker")
            {
                url = "https://i.ibb.co/8Mqx0YM/Joule-Broker-Mail-header.png";
                backgroundimg = "https://i.ibb.co/y0PKkmF/Joule-Backround.png";
                colorbackground = "#242425";
            }

            if (calendar.IDPageRequest == "MGISolutions")
            {
                url = "https://i.ibb.co/j4MxMdb/MGI-Solutions-header.png";
                backgroundimg = "https://i.ibb.co/31J0RnN/MGIBackground.png";
                colorbackground = "#208dec";
            }
            // StreamReader sr = File.OpenText(Server.MapPath("~/TemplateEmail/EmailCustomer"));
            string strContents = System.IO.File.ReadAllText(Server.MapPath(@"~/Views/Email_Calendar/EmailCalendar.html"));
            //strContents = strContents.Replace("{Date}", calendar.Date.ToString("MM/dd/yyyy HH:mm"));
            DateTime dateTimeUTC = Helper.Util.ConvertTimeZoneFromFloridaToUTC0(calendar.Date);
            DateTime dateTime = Helper.Util.ConvertUTC0ToTimeZone(dateTimeUTC, calendar.TimeZone);
            strContents = strContents.Replace("{logo}", url);
            strContents = strContents.Replace("{background}", backgroundimg);
            strContents = strContents.Replace("{datemeeting}", dateTime.ToString("dddd, MMMM dd yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
            strContents = strContents.Replace("{timemeeting}", dateTime.ToString("HH:mm") + " - " + dateTime.AddMinutes(calendar.Duration).ToString("HH:mm"));
            strContents = strContents.Replace("{namepagerequest}", pageRequest.Name);
            strContents = strContents.Replace("{reason}", calendar.Reason);
            strContents = strContents.Replace("{email}", customer.Email);
            strContents = strContents.Replace("{firstName}", customer.FirstName);
            strContents = strContents.Replace("{lastName}", customer.LastName);
            strContents = strContents.Replace("{colorbackground}", colorbackground);
            //sr.Close();
            return strContents;
        }
        public DateTime GetTimeNowUTC()
        {
            DateTime now = Helper.Util.ConvertUTC0ToFlorida();
            return now;
        }
        void sendMailDecline(Calendar calendar, Customer customer, string linkProspect)
        {
            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string from = section.From;
            string host = section.Network.Host;
            int port = section.Network.Port;
            bool enableSsl = section.Network.EnableSsl;
            string user = section.Network.UserName;
            string password = section.Network.Password;
            PageRequest pageRequest = service.GetInfoPageRequest(calendar.IDPageRequest);
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

        string getEmailDecline(Calendar calendar, Customer customer, string linkProspect)
        {
            // return Hostname
            
            PageRequest pageRequest = service.GetInfoPageRequest(calendar.IDPageRequest);

            string url=null;
            string backgroundimg = null;
            string colorbackground = null;
            if (calendar.IDPageRequest== "JouleBroker")
            {
                url = "https://i.ibb.co/8Mqx0YM/Joule-Broker-Mail-header.png";
                backgroundimg = "https://i.ibb.co/y0PKkmF/Joule-Backround.png";
                colorbackground = "#242425";
            }
            
            if (calendar.IDPageRequest == "MGISolutions")
            {
                url = "https://i.ibb.co/j4MxMdb/MGI-Solutions-header.png";
                backgroundimg = "https://i.ibb.co/31J0RnN/MGIBackground.png";
                colorbackground = "#208dec";
            }
                
            string strDecline = System.IO.File.ReadAllText(Server.MapPath(@"~/Views/Email_Calendar/EmailDecline.html"));
            //strContents = strContents.Replace("{Date}", calendar.Date.ToString("MM/dd/yyyy HH:mm"));
            DateTime dateTime = Helper.Util.ConvertTampaFloridaToTimeZone(calendar.Date, calendar.TimeZone);
            strDecline = strDecline.Replace("{logo}",url);
            strDecline = strDecline.Replace("{background}", backgroundimg);
            strDecline = strDecline.Replace("{datemeeting}", dateTime.ToString("dddd, MMMM dd yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
            strDecline = strDecline.Replace("{timemeeting}", dateTime.ToString("HH:mm") + " - " + dateTime.AddMinutes(calendar.Duration).ToString("HH:mm"));
            strDecline = strDecline.Replace("{namepagerequest}",pageRequest.Name);
            strDecline = strDecline.Replace("{reason}", calendar.Reason);
            strDecline = strDecline.Replace("{email}", customer.Email);
            strDecline = strDecline.Replace("{firstName}", customer.FirstName);
            strDecline = strDecline.Replace("{lastName}", customer.LastName);
            strDecline = strDecline.Replace("{linkProspect}", linkProspect);
            strDecline = strDecline.Replace("{colorbackground}", colorbackground);
            return strDecline;
        }
    }
}