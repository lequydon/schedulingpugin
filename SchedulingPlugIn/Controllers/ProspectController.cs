using SchedulingPlugIn.Helper;
using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.Service;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
//using SchedulingPlugIn.Helper;
using System.Net.Configuration;
using System.Net.Mime;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using System.Web.Routing;
using SchedulingPlugIn.Servicex;

namespace SchedulingPlugIn.Controllers
{
    [HandleError]
    public class ProspectController : Controller
    {
        PageQuestionService pagequestionService = new PageQuestionService();

        TimeSlotService TimeSlotService = new TimeSlotService();

        CustomerService CustomerService = new CustomerService();

        CalendarService CalendarService = new CalendarService();

        CalendarPoolingService CalendarPoolingService = new CalendarPoolingService();

        PageRequestService PageReQuestService = new PageRequestService();
        // GET: Prospect
        public ActionResult Index(string id)
        {

            Session["pageCode"] = id;
            return View();
        }
        public ActionResult Index1(string id)
        {

            Session["pageCode"] = id;
            return View();
        }
        public ActionResult GetListQuestionByPageCode()
        {
            var pagecode = Session["pageCode"].ToString();
            var list = pagequestionService.GetListQuestion(pagecode);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList()
            
        {
           
            var list = TimeSlotService.GetList(DateTime.UtcNow.AddDays(1));
            
            //foreach (var i in list)
            //{
                
            //    i.DateStart = i.DateStart.ConvertTimeZoneFromFloridaToUTC0();
                
            //}

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListForProspect_DetalTimeSlot()
        {

            var list = TimeSlotService.GetListForProspect_DetalTimeSlot(DateTime.UtcNow.AddDays(1));
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public int Confirm(string idTimeSlot, Customer customer, List<Calendar> calendar, QuestionByIDViewModels[] arrQuestion, string timeZone)
        {
            // CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            if (CalendarService.CheckTimeSlotUsed(idTimeSlot, CalendarStatus.EmailSended) == Service.Interface.CODE_RESULT_RETURN.ThatBai) return 0;
            if (CalendarService.CheckTimeSlotUsed(idTimeSlot, CalendarStatus.Confirmed) == Service.Interface.CODE_RESULT_RETURN.ThanhCong)
            {
                var pagecode = Session["pageCode"].ToString();
                if (CustomerService.CheckEmail(customer.Email) == Service.Interface.CODE_RESULT_RETURN.ThanhCong)
                {   
                    //edit TimeSlot
                    // TimeSlotService.UpdateStatusUsed(idTimeSlot);
                    //add Customer
                    customer.ID = Guid.NewGuid().ToString(); //(CustomerService.CountID() + 1).ToString();
                    CustomerService.Create(customer);
                }
                else {
                    customer.ID = CustomerService.getIDCustomerFromEmail(customer.Email);
                    CustomerService.Update(customer);
                }
                List<string> arrUrlAccept = new List<string>();
                List<string> arrurlDecline = new List<string>();
                //add Calendar
                List<Calendar> Calendars = new List<Calendar>();
                Calendar calendars = new Calendar();
                foreach (var j in calendar) {
                    if (CalendarService.CheckIDTimeSlot(idTimeSlot, customer.ID) == Service.Interface.CODE_RESULT_RETURN.ThanhCong)
                    {
                        calendars.ID = Guid.NewGuid().ToString();
                        calendars.IDPageRequest = pagequestionService.getIDPageRequest(pagecode);
                        calendars.IDCustomer = customer.ID;
                        calendars.HourFrom = j.Date.Hour;
                        calendars.MinuteFrom = j.Date.Minute;
                        calendars.Status = CalendarStatus.EmailSended;
                        calendars.IDTimeSlot = idTimeSlot;
                        calendars.DateCreated = DateTime.UtcNow;
                        //calendars.Date = Util.ConvertTimeZoneToTampaFlorida(calendars.Date, standartTime);
                        calendars.Date = j.Date;
                        calendars.Duration = j.Duration;
                        calendars.TimeZone = timeZone;
                        Calendars.Add(calendars);
                        CalendarService.Create(calendars);

                        
                        //add CalendarPoolings
                        if (arrQuestion != null)
                        {
                            foreach (QuestionByIDViewModels i in arrQuestion)
                            {
                                CalendarPooling calendarPooling = new CalendarPooling();
                                var question = pagequestionService.Get(i.IDPageQuestion);
                                calendarPooling.ID = Guid.NewGuid().ToString();
                                calendarPooling.IDPageQuestion = i.IDPageQuestion;
                                calendarPooling.IDCalendar = j.ID;
                                calendarPooling.Answer = i.Answer;
                                calendarPooling.Question = question.Question;
                                calendarPooling.OrderQuestion = question.OrderQuestion;
                                CalendarPoolingService.Create(calendarPooling);
                            }
                        }
                        Uri requestUrl = HttpContext.Request.Url;
                        var protocol = Uri.UriSchemeHttp;
                        var hostName = requestUrl.Host;
                        var urlAccept = Url.Action("Accept", "Prospect", new RouteValueDictionary() { { "idCalendar", j.ID } }, protocol, hostName);
                        var urlDecline = Url.Action("Decline", "Prospect", new RouteValueDictionary() { { "idCalendar", j.ID } }, protocol, hostName);
                        //var url = Url.Action("Accept", "Prospect", new RouteValueDictionary(), protocol, hostName);

                        //var urlHelper = new UrlHelper(HttpContext.Request.RequestContext);
                        //var url = urlHelper.Action("Accept", "Prospect", new { idCalendar = calendars.ID });
                        arrUrlAccept.Add(urlAccept);
                        arrurlDecline.Add(urlDecline);
                        //TimeSlotService.UpdateStatusUsed(idTimeSlot);
                       
                    }
                    sendMail(customer, Calendars, arrUrlAccept, arrurlDecline);
                    return 1;
                }
                return -1;
            }
            return 0;

        }
        public ActionResult Accept(string idCalendar)
        {
            ViewData["class"] = "red";
            var calendar = CalendarService.GetCalendar(idCalendar);
            DateTime date = Util.ConvertUTC0ToFlorida();
            var hour = (date - calendar.DateCreated).Hours;
            if (hour > 12)
            {
                ViewData["message"] = "The require your consultation is out of date";
            }
            else
            {
                ViewData["IDPageRequest"] = calendar.IDPageRequest;
                var result = CalendarService.UpdateStatusPendding(idCalendar, CalendarStatus.WaitConfirm);
                if (result == Service.Interface.CODE_RESULT_RETURN.ThanhCong /*&& TimeSlotService.CheckStatusUsedTimeSlot(calendar.IDTimeSlot) == Service.Interface.CODE_RESULT_RETURN.ThatBai*/ )
                {
                    TimeSlotService.UpdateStatusUsedTimeSlot(calendar.IDTimeSlot, TimeSlotStatus.Used);
                    ViewData["message"] = "Your request is confirmed!";
                    ViewData["class"] = "green";
                }
                else ViewData["message"] = "The require your consultation is failed";

            }

            return View();
        }
        public ActionResult Decline(string idCalendar)
        {
            ViewData["class"] = "red";
            var calendar = CalendarService.GetCalendar(idCalendar);
            var result = CalendarService.UpdateStatusDecline(idCalendar, CalendarStatus.Decline);
            if (result == Service.Interface.CODE_RESULT_RETURN.ThanhCong)
            {
                TimeSlotService.UpdateStatusUsedTimeSlot(calendar.IDTimeSlot, TimeSlotStatus.NotYetUsed);
                ViewData["message"] = "Request to Decline your consultation successfully";
                ViewData["class"] = "green";
            }
            else ViewData["message"] = "Request to Decline your consultation failed";
            return View("Accept");
        }
        
        void sendMail(Customer customer, List<Calendar> calendars, List<string> linkAccept, List<string> linkDecline)
        {
            PageRequest page = PageReQuestService.Get(calendars[0].IDPageRequest);
            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string from = section.From;
            string host = section.Network.Host;
            int port = section.Network.Port;
            bool enableSsl = section.Network.EnableSsl; 
            string user = section.Network.UserName;
            string password = section.Network.Password;
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from, "4MGI Contact Center");
            msg.To.Add(new MailAddress(customer.Email));
            msg.Subject = "Consultation Request Confirmation- " + page.Name;

 
            //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("", null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(getContentEmail(customer, linkAccept, linkDecline , calendars), null, MediaTypeNames.Text.Html));
            //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(Util.CreateICSFile(dateStart, dateEnd, timeZone), "event.ics", "text/calendar");
            //message.Attachments.Add(attachment);
            //msg.Attachments.Add(attachment);
            SmtpClient smtpClient = new SmtpClient(host, port);
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(from, password);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = enableSsl;
            smtpClient.Send(msg);
        }
        string getContentEmail(Customer customer, List<string> linkAccept, List<string> linkDecline, List<Calendar> calendars)
        {
            PageRequest page = PageReQuestService.Get(calendars[0].IDPageRequest);
            // StreamReader sr = File.OpenText(Server.MapPath("~/TemplateEmail/EmailCustomer"));
            string strContents = System.IO.File.ReadAllText(Server.MapPath(@"~/TemplateEmail/EmailCustomer.html"));
            strContents = strContents.Replace("{firstName}", customer.FirstName);
            strContents = strContents.Replace("{lastName}", customer.LastName);
            //strContents = strContents.Replace("{startDate}", dateStart.ToString("MM/dd/yyyy HH:mm"));
            //strContents = strContents.Replace("{endDate}", dateEnd.ToString("MM/dd/yyyy HH:mm"));
            //strContents = strContents.Replace("{location}", "MGI Company");
            //strContents = strContents.Replace("{detail}", "Meeting to consultation");
            //strContents = strContents.Replace("{linkGoogleCalendar}", linkCalendar);
            strContents = strContents.Replace("{email}", customer.Email);
            foreach (var i in linkAccept) { 
                strContents = strContents.Replace("{linkAccept}", i);
            }
            foreach (var i in linkDecline)
            {
                strContents = strContents.Replace("{linkDecline}", i);
            }
            strContents = strContents.Replace("{pagerequest}",page.Name);
            // string header;
            string logo=null;
            string background=null;
            string colorbackground = null;
            if (calendars[0].IDPageRequest == "MGISolutions")
            {
                logo = "https://i.ibb.co/j4MxMdb/MGI-Solutions-header.png";
                background = "https://i.ibb.co/FKp0bb9/MGIBackground.png";
                colorbackground = "#208dec";
            }
            if (calendars[0].IDPageRequest == "JouleBroker")
            {
                logo = "https://i.ibb.co/8Mqx0YM/Joule-Broker-Mail-header.png";
                background = "https://i.ibb.co/y0PKkmF/Joule-Backround.png";
                colorbackground = "#242425";
            }
            strContents = strContents.Replace("{logo}", logo);
            strContents = strContents.Replace("{background}", background);
            strContents = strContents.Replace("{colorbackground}", colorbackground);
             return strContents;
        } 

        public class QuestionByIDViewModels
        {
            public string IDPageQuestion { get; set; }
            public string Answer { get; set; }
        }
        public static CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = System.Web.Configuration.WebConfigurationManager.AppSettings["recaptchaPrivateKey"];
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }
       
    }
}