using Newtonsoft.Json;
using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SchedulingPlugIn.Helper
{
    public static class Util
    {
        public static string GenerateCode(int length)
        {
            Random rnd = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[rnd.Next(characters.Length)]);
            }
            return result.ToString();
        }
        /// <summary>
        /// Thực hiện khi lấy dữ liệu hiển thị lên trình duyệt
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timezoneOffset"></param>
        /// <returns></returns>
        public static DateTime ConvertTampaFloridaToTimeZone(this DateTime time, string standartTime)
        {
            TimeZoneInfo tsF = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            TimeZoneInfo tsT = TimeZoneInfo.FindSystemTimeZoneById(standartTime);
            time = DateTime.SpecifyKind(time, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(time, tsF, tsT);
        }
        /// <summary>
        /// Thực hiện khi lấy dữ liệu từ Database ra web
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timezoneOffset"></param>
        /// <returns></returns>
        public static DateTime ConvertLocalServerToTimeZone(this DateTime time, string standartTime)
        {
            string timeZoneFlorida = "Eastern Standard Time";
            // TimeZoneInfo tsT = TimeZoneInfo.FindSystemTimeZoneById(standartTime);
            //DateTime now = DateTime.SpecifyKind(time, DateTimeKind.Utc);
            //var utc = TimeZoneInfo.ConvertTimeToUtc(now, tsT);
            // TimeZoneInfo tsF = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault();

            //return TimeZoneInfo.ConvertTime(time, tsF, tsT);

            DateTime cstTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, standartTime, timeZoneFlorida);
            return cstTime;
        }
        /// <summary>
        /// Thực hiện khi lưu dữ liệu vào Databxase
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timezoneOffset"></param>Eastern Standard Time
        /// <returns></returns>
        public static DateTime ConvertTimeZoneToTampaFlorida(this DateTime time, string standartTime)
        {

            //time = new DateTime(1970, 1, 1, 12, 1, 0, DateTimeKind.Utc);
            //TimeZoneInfo tsT = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            //TimeZoneInfo tsF = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");// standartTime);
            //return TimeZoneInfo.ConvertTime(time, tsF, tsT);

            //TimeZoneInfo tsT = TimeZoneInfo.FindSystemTimeZoneById(standartTime);
            ////DateTime now = DateTime.SpecifyKind(time, DateTimeKind.Utc);
            ////var utc = TimeZoneInfo.ConvertTimeToUtc(now, tsT);
            //TimeZoneInfo tsF = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault();

            //DateTime dateProspectToLocalServer = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, standartTime, TimeZoneInfo.Local.Id);

            //return TimeZoneInfo.ConvertTime(time, tsF, tsT);
            string timeZoneFlorida = "Eastern Standard Time";
            //DateTime cstTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateProspectToLocalServer, TimeZoneInfo.Local.Id, timeZoneFlorida);
            //return cstTime;


            time = new DateTime(time.Ticks, DateTimeKind.Unspecified);
            //var timelocal = time.ToLocalTime();
            //var timezonelocal = TimeZoneInfo.l
            DateTime timetime = DateTime.SpecifyKind(time, DateTimeKind.Unspecified);
            DateTime cstTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(timetime, standartTime, timeZoneFlorida);

            return cstTime;


        }
        public static DateTime ConvertTimeZoneToTampaFloridaNew1(this DateTime time, string standartTime)
        {
            //string timeZoneFlorida = "Eastern Standard Time";

            try
            {
                var sourceTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, standartTime);

                return sourceTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DateTime ConvertTimeZoneToTampaFloridaNew2(this DateTime time, string standartTime, string timeZoneFlorida)
        {
            //string timeZoneFlorida = "Eastern Standard Time";

            try
            {
                var sourceTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, standartTime);
                var desTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, timeZoneFlorida);

                return desTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Chuyển TimeZone từ máy tính thành giờ của SV
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timezoneOffset"></param>
        /// <returns></returns>
        /// 
       
        public static DateTime ConvertUTC0ToFlorida()
        {
            DateTime time = DateTime.UtcNow;
            string timeZoneFlorida = "Eastern Standard Time";
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneFlorida);
            var dateFlorida = TimeZoneInfo.ConvertTimeFromUtc(time, tz);
            return dateFlorida;
        }

        public static DateTime ConvertTimeZoneFromFloridaToUTC0(this DateTime time)
        {
            string timeZoneFlorida = "Eastern Standard Time";
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneFlorida);
            var dateUTC0 = TimeZoneInfo.ConvertTimeToUtc(time, tz);
            return dateUTC0;
        }
        public static DateTime ConvertUTC0ToTimeZone(this DateTime time, string timeZoneString)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneString);
            var dateTimeZone = TimeZoneInfo.ConvertTimeFromUtc(time, tz);
            return dateTimeZone;
        }
        public static DateTime ConvertTimeZoneToUTC0(this DateTime time, string timeZoneString)
        {
            //var tz1 = TimeZoneInfo.Utc;
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneString);
            var times = DateTime.ParseExact(time.ToString("MM/dd/yyyy HH:mm:ss"), "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            var dateTimeZone = TimeZoneInfo.ConvertTimeToUtc(times, tz);
            return dateTimeZone;
        }
        public static DateTime ConvertTimeZoneToLocalServer(this DateTime time, int timezoneOffset)
        {
            TimeZoneInfo tsT = TimeZoneInfo.Local;
            TimeZoneInfo tsF = GetTimeZoneNameByGMT(timezoneOffset);
            return TimeZoneInfo.ConvertTime(time, tsF, tsT);
        }

        internal static DateTime LinkEmail(Customer customer)
        {
            throw new NotImplementedException();
        }

        public static List<string> GetListTimeZoneName()
        {
            List<string> rs = new List<string>();
            foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
                rs.Add(z.Id);
            return rs;
        }
        public static TimeZoneInfo GetTimeZoneNameByGMT(int gmt)
        {
            List<string> rs = new List<string>();
            foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
                if (z.BaseUtcOffset.Hours == gmt)
                    return z;
            return null;
        }
        public static string LinkEmail(Customer customer, DateTime dateStart, DateTime dateEnd)
        {
            string linkemail = string.Format("http://www.google.com/calendar/event?action=TEMPLATE&dates={0:yyyyMMddTHHmmss}%2F{1:yyyyMMddTHHmmss}&text={2}&location={3}&details={4}", dateStart, dateEnd, "Meeting with MGI", "MGI Company", "Meeting to consultation");
            return linkemail;
        }
        public static CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = System.Web.Configuration.WebConfigurationManager.AppSettings["recaptchaPrivateKey"];
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }
        public static string CreateICSFile(string emailSender, DateTime dateStart, DateTime dateEnd, string standartTimeZone)
        {
            StringBuilder sb = new StringBuilder();
            string DateFormat = "yyyyMMddTHHmmss";
            //DateTime now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            //var utc = TimeZoneInfo.ConvertTimeToUtc(now, zone);
            string now = DateTime.Now.ToString(DateFormat);
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:-//MGI Solution//Scheduling//EN");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:REQUEST");
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("DTSTART:" + dateStart.ToString(DateFormat));
            sb.AppendLine("DTEND:" + dateEnd.ToString(DateFormat));
            sb.AppendLine("DTSTAMP:" + now);
            sb.AppendLine("UID:" + Guid.NewGuid());
            sb.AppendLine("CREATED:" + now);
            sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + "MGI Solutions");
            sb.AppendLine("DESCRIPTION:" + "Meeting to consultation");
            sb.AppendLine("LAST-MODIFIED:" + now);
            sb.AppendLine("CLASS:PUBLIC");
            sb.AppendLine("LOCATION:" + "MGI Solution");
            sb.AppendLine("SEQUENCE:0");
            //sb.AppendLine("X-MICROSOFT-CDO-BUSYSTATUS:BUSY");
            //sb.AppendLine("X-MICROSOFT-CDO-INTENDEDSTATUS:BUSY");
            sb.AppendLine(String.Format("ORGANIZER;CN={0}:mailto:{1}", "MGI Solutions", emailSender));
            sb.AppendLine(String.Format("ATTENDEE;CN=MGI Solutions;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;RSVP=TRUE:mailto:{0}", emailSender));
            sb.AppendLine("STATUS:CONFIRMED");
            sb.AppendLine(String.Format("ATTENDEE;CN=MGI Solutions;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;RSVP=TRUE:mailto:{0} STATUS: CONFIRMED", emailSender));
            sb.AppendLine("STATUS:CONFIRMED");
            sb.AppendLine("SUMMARY:" + "Meeting with MGI Company");
            //sb.AppendLine("TRANSP:OPAQUE");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");
            //sb.AppendLine("ACTION:DISPLAY");
            return sb.ToString();
            //var calendarBytes = Encoding.UTF8.GetBytes(sb.ToString());
            //return new MemoryStream(calendarBytes);
            //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, "event.ics", "text/calendar");
            //message.Attachments.Add(attachment);
        }
    }
}