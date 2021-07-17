using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.RepositoryLayer;
using SchedulingPlugIn.RepositoryLayer.Interface;
using SchedulingPlugIn.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using SchedulingPlugIn.Helper;

namespace SchedulingPlugIn.Service
{
    public class CalendarService : ICalendarService
    {
        IDALContext context;

        public object Interface { get; internal set; }

        public CalendarService()
        {
            context = new DALContext();
        }
        public CODE_RESULT_RETURN Create(Calendar t)
        {
            var calendar = new Calendar
            {
                ID = t.ID,
                IDPageRequest = t.IDPageRequest,
                IDCustomer = t.IDCustomer,
                Date = t.Date,
                HourFrom = t.HourFrom,
                MinuteFrom = t.MinuteFrom,
                Duration = t.Duration,
                Status = t.Status,
                Reason = t.Reason,
                IDTimeSlot = t.IDTimeSlot,
                DateCreated = t.DateCreated,
                TimeZone = t.TimeZone
            };
            context.Calendars.Create(calendar);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }

        public CODE_RESULT_RETURN Delete(Calendar t)
        {
            throw new NotImplementedException();
        }
       
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Calendar Get(string id)
        {
            throw new NotImplementedException();
        }
        public List<CalendarViewModels> Answer(string id)
        {
            var ListQuestion = context.CalendarPoolings.Filter(e => e.IDCalendar == id).Select(e => new CalendarViewModels
            {
                Answer = e.Answer,
                Question = e.Question,
                ID = e.IDCalendar,
                OrderQuestion = e.OrderQuestion
            }).OrderBy(e => e.OrderQuestion);
            //var lst = context.Calendars.Filter(e => e.ID == id).FirstOrDefault();
            //var ls = context.PageQuestions.Filter(e => e.PageRequestID == lst.IDPageRequest).Select(e => new DetaillsQuestionViewModels
            //{
            //    ID=e.ID,
            //    Question = e.Question
            //}).ToList();
            //var lspoling = context.CalendarPoolings.Filter(e => e.IDCalendar == id).Select(e => new DetaillsQuestionViewModels
            //{
            //    ID = e.IDPageQuestion,
            //    Answer = e.Answer
            //}).ToList();
            //foreach (var i in ls)
            //    foreach (var y in lspoling)
            //    {
            //        if (i.ID == y.ID)
            //            i.Answer = y.Answer;
            //    }
            return ListQuestion.ToList();
        }
        public List<CalendarViewModels> ContactPopup(string id)
        {
            var contaclist = context.Calendars.Filter(e => e.ID == id).Select(e => new CalendarViewModels
            {
                Date = e.Date,
                FirstName = e.Customer.FirstName + " " + e.Customer.LastName,
                PageRequestName = e.PageRequest.Name,
                Email = e.Customer.Email,
                Phone = e.Customer.Phone,
                Duration = e.Duration,
                ID = e.ID
            });

            return contaclist.ToList();
        }
        public List<CalendarViewModels> GetListDashBoard(CalendarStatus calendarStatus)
        {

            if ( calendarStatus == CalendarStatus.WaitConfirm) {
                DateTime DateTimeZone = Helper.Util.ConvertUTC0ToFlorida();
                var ls = context.Calendars.Filter(e => (e.Status == calendarStatus && e.Date >= DateTimeZone)/* || (e.Status == CalendarStatus.Decline && e.Date >= DateTimeZone)*/).Select(e => new CalendarViewModels
                {
                    ID = e.ID,
                    Email = e.Customer.Email,
                    FirstName = e.Customer.FirstName + " " + e.Customer.LastName,
                    Date = e.Date,
                    PageRequestName = e.PageRequest.Name,
                    Duration = e.Duration

                }).OrderBy(e => e.Date).Take(10);
                return ls.ToList();
            }
            else if(calendarStatus==CalendarStatus.Confirmed)
            {
                DateTime DateTimeZone = Helper.Util.ConvertUTC0ToFlorida();
                var ls = context.Calendars.Filter(e => e.Status == calendarStatus && e.Date >= DateTimeZone ).Select(e => new CalendarViewModels
                {
                    ID = e.ID,
                    Email = e.Customer.Email,
                    FirstName = e.Customer.FirstName + " " + e.Customer.LastName,
                    Date = e.Date,
                    PageRequestName = e.PageRequest.Name,
                    Duration = e.Duration

                }).OrderBy(e => e.Date).Take(10);
                return ls.ToList();
            }
            return null;
        }
        public List<CalendarViewModels> GetListDashBoardexpired()
        {
            DateTime now = Helper.Util.ConvertUTC0ToFlorida();
            TimeSpan TimeLimit = TimeSpan.Parse("12:00:00");
            var ls = context.Calendars.All().Select(e => new CalendarViewModels {
                ID = e.ID,
                Email = e.Customer.Email,
                FirstName = e.Customer.FirstName + " " + e.Customer.LastName,
                Date = e.Date,
                PageRequestName = e.PageRequest.Name,
                Duration = e.Duration,
                datecreated=e.DateCreated,
                Status=e.Status
            }).ToList();
            var lst = ls.Where(e => e.Date <now  && e.Status==CalendarStatus.WaitConfirm).OrderBy(e=>e.Date).ToList();
            //var lstist = lst.Where(e => e.Status != CalendarStatus.Decline).ToList();
            //var listTimeExpired1 = listTimeExpired.Where(e => now - CovertTimeZone(e.DateCreated) > TimeLimit).ToList();
            return lst.ToList();
        }
        public List<CalendarViewModels> GetDashBoardexpired()
        {
            DateTime now = Helper.Util.ConvertUTC0ToFlorida();
            TimeSpan TimeLimit = TimeSpan.Parse("12:00:00");
            var ls = context.Calendars.All().Select(e => new CalendarViewModels
            {
                ID = e.ID,
                Email = e.Customer.Email,
                FirstName = e.Customer.FirstName,
                LastName = e.Customer.LastName,
                Phone = e.Customer.Phone,
                Date = e.Date,
                PageName = e.PageRequest.Name,
                Duration = e.Duration,
                datecreated = e.DateCreated,
                Status = e.Status
            }).ToList();
            var lst = ls.Where(e =>e.Status == CalendarStatus.WaitConfirm && e.Date < now).OrderBy(e => e.Date).ToList();
            return lst;
        }
        //public List<Calendar> IdenticalCheckTimesend(string IdCalendar)
        //{
        //    Calendar TimeUse = context.Calendars.Filter(e => e.ID == IdCalendar).FirstOrDefault();
        //    var ListIdentical = context.Calendars.Filter(e =>e.Status != CalendarStatus.Decline && e.Date == TimeUse.Date && e.Duration == TimeUse.Duration);
        //    //var dateend = TimeUse.Date.AddMinutes(TimeUse.Duration);
        //    //var ListIdentical = context.Calendars.Filter(e => (e.Date >= TimeUse.Date && e.Date < dateend) || (e.Date.AddMinutes(e.Duration) >= TimeUse.Date && e.Date.AddMinutes(e.Duration) < dateend));            
        //    return ListIdentical.ToList();
        //}
        public List<Calendar> IdenticalCheckTime(string IdCalendar)
        {
            Calendar TimeUse = context.Calendars.Filter(e => e.ID == IdCalendar).FirstOrDefault();
            var ListIdentical = context.Calendars.Filter(e => e.Status == CalendarStatus.WaitConfirm && e.Date == TimeUse.Date && e.Duration == TimeUse.Duration);
            //var dateend = TimeUse.Date.AddMinutes(TimeUse.Duration);
            //var ListIdentical = context.Calendars.Filter(e => (e.Date >= TimeUse.Date && e.Date < dateend) || (e.Date.AddMinutes(e.Duration) >= TimeUse.Date && e.Date.AddMinutes(e.Duration) < dateend));
            var ls = ListIdentical.ToList();
            foreach (var i in ls)
            {
                if (i.ID != IdCalendar)
                {
                    i.Status = CalendarStatus.Decline;
                    i.Reason = "Your consulation is out of date";
                }

            }
            context.SaveChanges();
            return ls.ToList();
            
        }
        public List<CalendarViewModels> ListContact()
        {
            DateTime now =  Helper.Util.ConvertUTC0ToFlorida();

            var contaclist = context.Calendars.Filter(e => e.Status != CalendarStatus.Decline && e.Status == CalendarStatus.Confirmed).Select(e => new CalendarViewModels
            {
                Date = e.Date,
                FirstName = e.Customer.FirstName + " " + e.Customer.LastName,
                PageRequestName = e.PageRequest.Name,
                Email = e.Customer.Email,
                Phone = e.Customer.Phone,
                Duration = e.Duration,
                ID = e.ID,
                Status = e.Status
            });
            List<CalendarViewModels> Contact = new List<CalendarViewModels>();
            var flag = 0;
            var flag1 = 0;
            foreach (var i in contaclist)
            {
                if (flag == 0)
                {
                    Contact.Add(i);
                    flag++;
                }
                if (flag1 == 0)
                    if (DateTime.Compare(now, i.Date) <= 0)
                    {
                        Contact.Clear();
                        Contact.Add(i);
                        flag1++;
                    }
                if (DateTime.Compare(now, i.Date) <= 0 && DateTime.Compare(Contact[0].Date, i.Date) > 0)
                {
                    Contact.Clear();
                    Contact.Add(i);
                }
            }
            try
            {
                if (DateTime.Compare(Contact[0].Date, now) < 0)
                    Contact.Clear();
            }
            catch
            {

                return Contact.ToList();
            }
            
            return Contact.ToList();
        }
        public List<CalendarViewDateCountModel> GetTimeCalendar()
        {
            DateTime thisnow = Helper.Util.ConvertUTC0ToFlorida();
            var lst = context.Calendars.Filter(e => e.Date.Month == thisnow.Month && e.Date.Year == thisnow.Year && e.Status == CalendarStatus.Confirmed).Select(e => new CalendarViewDateCountModel
            {
                date = e.Date,
                value = 1
            }).ToList();
            var lsr = lst.GroupBy(x => x.date.Day).Select(e => new CalendarViewDateCountModel
            {
                time = e.Key.ToString(),
                value = e.Count()
            }).ToList();
            return lsr;
        }
        public PageRequest GetInfoPageRequest(string pageID)
        {
            return context.PageRequests.Find(e => e.ID == pageID);
        }
        public List<CalendarViewDateCountModel> gettimeforanalysis()
        {
            DateTime thisnow = Helper.Util.ConvertUTC0ToFlorida();
            var ls = context.Calendars.Filter(e => e.Date.Month == thisnow.Month && e.Date.Year == thisnow.Year && e.Status == CalendarStatus.Confirmed).Select(e => new CalendarViewDateCountModel
            {
                date = e.Date
            });
            return ls.ToList();
        }
        public PageRequest GetPageCode(string idpage) { 
                return context.PageRequests.Filter(e => e.ID == idpage).FirstOrDefault();
                
            }
        //public List<string> GetTimeCalendar()
        //{
        //    var ls = context.Calendars.All().Select(e => e.ID);
        //    return ls.ToList();
        //}
        public List<Calendar> GetList()
        {

            throw new NotImplementedException();
        }
        public CODE_RESULT_RETURN UpdateReason(string idCalendar, int status, string reason)
        {

            if ((CalendarStatus)status == CalendarStatus.Confirmed)
            {
                var lt = context.Calendars.Filter(e => e.ID == idCalendar).FirstOrDefault();
                var ls = context.TimeSlots.Filter(e => e.ID == lt.IDTimeSlot).FirstOrDefault();
                if (ls != null)
                {
                    ls.Status = TimeSlotStatus.Used;
                }
            }
            var p = context.Calendars.Filter(e => e.ID == idCalendar).FirstOrDefault();
            if (p != null)
            {
                p.Status = (CalendarStatus)status;
                p.Reason = reason;
            }
            context.SaveChanges();

            return CODE_RESULT_RETURN.ThanhCong;
        }
        public List<CalendarViewModels> GetListCalendarViewModelsT()
        {
            DateTime date = Util.ConvertUTC0ToFlorida();
           var tslot = context.TimeSlots.All().Where(e => e.Status == TimeSlotStatus.NotYetUsed && e.DateStart > date).Select(e => new CalendarViewModels
            {
                ID = e.ID,
                Date = e.DateStart,
                Duration = e.Duration,
                Reason = e.Note,
                Status = 0
            });

            var cal = context.Calendars.All().Where(e => (e.Date > date && e.Status != CalendarStatus.Confirmed) || e.Status == CalendarStatus.Confirmed).Select(e => new CalendarViewModels
            {
                ID = e.IDTimeSlot,
                Date = e.Date,
                Duration = e.Duration,
                Reason = e.Reason,
                Status = e.Status
            });
            cal = cal.Union(tslot);
            var any = cal.GroupBy(e => e.ID).Select(d => d.FirstOrDefault());
            return any.ToList();
        }

        public CODE_RESULT_RETURN Update(Calendar t)
        {
            throw new NotImplementedException();
        }

        public List<ConsultationViewModels> GetListConsultation()
        {
            DateTime now = Helper.Util.ConvertUTC0ToFlorida();
            var rs = context.Calendars.Filter(e => e.Status != CalendarStatus.Decline  && e.Status != CalendarStatus.Completed && e.Status!=CalendarStatus.EmailSended && e.Date >= now).OrderBy(e => e.Date).OrderBy(e => e.Date).Select(e => new ConsultationViewModels
            {
                ID = e.ID,
                Date = e.Date,
                HourFrom = e.HourFrom,
                MinuteFrom = e.MinuteFrom,
                Duration = e.Duration,
                FirstName = e.Customer.FirstName,
                LastName = e.Customer.LastName,
                Email = e.Customer.Email,
                PageName = e.PageRequest.Name,
                Phone = e.Customer.Phone,
                Status = e.Status,
            });
            return rs.ToList();
        }


        public CODE_RESULT_RETURN DeleteConsultation(string id)
        {
            context.Calendars.Delete(e => e.ID == id);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }

        public List<ConsultationViewModels> GetListApproved(CalendarStatus status)
        {
            DateTime now = Helper.Util.ConvertUTC0ToFlorida();
            var statu = context.Calendars.Filter(e => e.Status == status && e.Date>=now).OrderBy(e => e.Date).Select(e => new ConsultationViewModels
            {
                ID = e.ID,
                Date = e.Date,
                HourFrom = e.HourFrom,
                MinuteFrom = e.MinuteFrom,
                Duration = e.Duration,
                FirstName = e.Customer.FirstName,
                LastName = e.Customer.LastName,
                Email = e.Customer.Email,
                PageName = e.PageRequest.Name,
                Phone = e.Customer.Phone,
                Status = e.Status
            });
            return statu.ToList();
        }
        
        public CODE_RESULT_RETURN Approvedd(string id, CalendarStatus status)
        {
            var save = context.Calendars.Filter(e => e.ID == id).FirstOrDefault();
            save.Status = status;
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }
        public CODE_RESULT_RETURN UpdateStatusPendding(string id, CalendarStatus status)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var cal = db.CalendarDbSet.Where(e => e.ID == id).FirstOrDefault();
            //var save = context.Calendars.Find(e => e.ID == id);
            if (cal != null && cal.Status == CalendarStatus.EmailSended)
            {
                cal.Status = status;
                db.SaveChanges();
                return CODE_RESULT_RETURN.ThanhCong;
            }
            return CODE_RESULT_RETURN.ThatBai;

        }
        public CODE_RESULT_RETURN UpdateStatusDecline(string id, CalendarStatus status)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var calendar = db.CalendarDbSet.Where(e => e.ID == id).FirstOrDefault();
            if(calendar != null && calendar.Status == CalendarStatus.EmailSended)
            {
                calendar.Status = status;
                db.SaveChanges();
                return CODE_RESULT_RETURN.ThanhCong;
            }
            return CODE_RESULT_RETURN.ThatBai;
        }
        public CODE_RESULT_RETURN CheckIDTimeSlot(string IDTimeSlot, string IDCustomer)
        {
            var calendar = context.Calendars.Find(e => e.IDTimeSlot == IDTimeSlot && e.IDCustomer == IDCustomer );
            if (calendar == null) return CODE_RESULT_RETURN.ThanhCong;
            else return CODE_RESULT_RETURN.IDTimeSlotTrung;
        }
        
        public CODE_RESULT_RETURN UpdateStatusActive(string IDCalendar)
        {
            var calendar = context.Calendars.Find(e => e.ID == IDCalendar);
            if (calendar != null)
            {
                calendar.Status = CalendarStatus.Confirmed;
                context.SaveChanges();
                return CODE_RESULT_RETURN.ThanhCong;
            }
            return CODE_RESULT_RETURN.ThatBai;
        }
        public CODE_RESULT_RETURN UpdateStatusActiveDecline(string IDCalendar, string reason)
        {
            var calendar = context.Calendars.Find(e => e.ID == IDCalendar);
            if (calendar != null)
            {
                calendar.Status = CalendarStatus.Decline;
                calendar.Reason = reason;
                context.SaveChanges();
                return CODE_RESULT_RETURN.ThanhCong;
            }
            return CODE_RESULT_RETURN.ThatBai;
        }

        public Customer getCustomerFromIDCalendar(string ID)
        {
            var calendar = context.Calendars.Find(e => e.ID == ID);
            var customer = context.Customers.Find(e => e.ID == calendar.IDCustomer);
            return customer;
        }
        public PageRequest getPageCodeFromIDCalendar(string id)
        {
            var ls = context.Calendars.Find(e => e.ID == id);
            var lst = context.PageRequests.Find(e => e.ID == ls.IDPageRequest);
            return lst;
        }
        public CalendarViewModels GetClendarviewmodel(string ID)
        {
            var calendarview = context.Calendars.Filter(e => e.ID == ID).Select(e => new CalendarViewModels
            {
                ID = e.ID,
                FirstName = e.Customer.FirstName + " " + e.Customer.LastName,
                Email = e.Customer.Email,
                PageRequestName = e.PageRequest.Name,
                IDPageRequest = e.IDPageRequest,
                TimeZone=e.TimeZone,
                Date=e.Date
            }).FirstOrDefault();
            return calendarview;
        }
        public Calendar GetCalendar(string ID)
        {
            var ls = context.Calendars.Find(e => e.ID == ID);
            return ls;
        }
        public CODE_RESULT_RETURN CheckTimeSlotUsed(string IDTimeSlot, CalendarStatus status)
        {
            var cal = context.Calendars.Filter(e => e.IDTimeSlot == IDTimeSlot && e.Status == status).FirstOrDefault();
            if (cal != null) return CODE_RESULT_RETURN.ThatBai;
            return CODE_RESULT_RETURN.ThanhCong;
        }

    }
}