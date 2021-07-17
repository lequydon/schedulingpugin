using SchedulingPlugIn.Models;
using SchedulingPlugIn.RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.RepositoryLayer
{
    public class DALContext : IDALContext
    {
        private ApplicationDbContext dbContext;
        private IPageRequestRepository pageRequests;
        private IPageQuestionRepository pageQuestions;
        private ICustomerRepository customers;
        private ICalendarRepository calendars;
        private ICalendarPoolingRepository calendarPoolings;
        private ITimeSlotRepository timeSlots;
        private ISessionTimeSlotRepository sessionTimeSlots;
        public DALContext(bool proxy=false)
        {
            if (!proxy)
                dbContext = new ApplicationDbContext(true);
            else
                dbContext = new ApplicationDbContext();
        }


        public IPageRequestRepository PageRequests
        {
            get
            {
                if (pageRequests == null)
                    pageRequests = new PageRequestRepository(dbContext);
                return pageRequests;
            }
        }
        public IPageQuestionRepository PageQuestions
        {
            get
            {
                if (pageQuestions == null)
                    pageQuestions = new PageQuestionRepository(dbContext);
                return pageQuestions;
            }
        }
        public ICustomerRepository Customers
        {
            get
            {
                if (customers == null)
                    customers = new CustomerRepository(dbContext);
                return customers;
            }
        }
        public ICalendarRepository Calendars
        {
            get
            {
                if (calendars == null)
                    calendars = new CalendarRepository(dbContext);
                return calendars;
            }
        }
        public ICalendarPoolingRepository CalendarPoolings
        {
            get
            {
                if (calendarPoolings == null)
                    calendarPoolings = new CalendarPoolingRepository(dbContext);
                return calendarPoolings;
            }
        }
        public ITimeSlotRepository TimeSlots
        {
            get
            {
                if (timeSlots == null)
                    timeSlots = new TimeSlotRepository(dbContext);
                return timeSlots;
            }
        }
        public ISessionTimeSlotRepository SessionTimeSlots
        {
            get
            {
                if (sessionTimeSlots == null)
                    sessionTimeSlots = new SessionTimeSlotRepository(dbContext);
                return sessionTimeSlots;
            }
        }
        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (pageRequests != null)
                pageRequests.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}