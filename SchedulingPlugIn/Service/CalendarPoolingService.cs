using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.RepositoryLayer;
using SchedulingPlugIn.RepositoryLayer.Interface;
using SchedulingPlugIn.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Service
{
    public class CalendarPoolingService : ICalendarPoolingService
    {
        IDALContext context;
        public CalendarPoolingService()
        {
            context = new DALContext();
        }
        public CODE_RESULT_RETURN Create(CalendarPooling t)
        {
            var calendarPooling = new CalendarPooling
            {
                ID = t.ID,
                IDPageQuestion = t.IDPageQuestion,
                IDCalendar = t.IDCalendar,
                Answer = t.Answer,
                Question = t.Question,
                OrderQuestion = t.OrderQuestion
            };
            context.CalendarPoolings.Create(calendarPooling);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }

        public CODE_RESULT_RETURN Delete(CalendarPooling t)
        {
            context.CalendarPoolings.Delete(t);
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }

        public CalendarPooling Get(string id)
        {
            var get = context.CalendarPoolings.Filter(e => e.ID == id).FirstOrDefault();
            return get;
        }

        public List<CalendarPooling> GetList()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="t">Câu trả lời của khách hàng trên lịch</param>
        /// <returns></returns>
        public CODE_RESULT_RETURN Update(CalendarPooling t)
        {
            var p = context.CalendarPoolings.Filter(e => e.ID == t.ID).FirstOrDefault();
            p.Answer = t.Answer;
            context.SaveChanges();
            return CODE_RESULT_RETURN.ThanhCong;
        }
        public List<CalendarViewModels> GetListCalendarViewModelsT()
        {
            var rs = context.Calendars.All().Select(e => new CalendarViewModels
            {
                ID = e.ID,
                IDPageRequest = e.PageRequest.ID,
                Email = e.Customer.Email,
                FirstName = e.Customer.FirstName,
                LastName = e.Customer.LastName,
                Date = e.Date,
                HourFrom = e.HourFrom,
                MinuteFrom = e.MinuteFrom,
                PageRequestName = e.PageRequest.Name,
                Duration = e.Duration,
                Reason = e.Reason
            });
            return rs.ToList();
        }
        
    }
}