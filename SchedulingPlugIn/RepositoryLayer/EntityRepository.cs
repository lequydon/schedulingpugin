using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using SchedulingPlugIn.RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.RepositoryLayer
{
    public class PageRequestRepository : Repository<PageRequest>, IPageRequestRepository
    {
        public PageRequestRepository(ApplicationDbContext context) : base(context) { }
    }
    public class PageQuestionRepository : Repository<PageQuestion>, IPageQuestionRepository
    {
        public PageQuestionRepository(ApplicationDbContext context) : base(context) { }

        public object LastOrDefault()
        {
            throw new NotImplementedException();
        }
    }

    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context) { }
    }
    public class CalendarRepository : Repository<Calendar>, ICalendarRepository
    {
        public CalendarRepository(ApplicationDbContext context) : base(context) { }

        public object Where(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        object ICalendarRepository.Filter()
        {
            throw new NotImplementedException();
        }

        object ICalendarRepository.FirstOrDefault()
        {
            throw new NotImplementedException();
        }
    }
    public class CalendarPoolingRepository : Repository<CalendarPooling>, ICalendarPoolingRepository
    {
        public CalendarPoolingRepository(ApplicationDbContext context) : base(context) { }
    }
    public class TimeSlotRepository : Repository<TimeSlot>, ITimeSlotRepository
    {
        public TimeSlotRepository(ApplicationDbContext context) : base(context) { }
    } public class SessionTimeSlotRepository : Repository<SessionTimeSlot>, ISessionTimeSlotRepository
    {
        public SessionTimeSlotRepository(ApplicationDbContext context) : base(context) { }
    }

}