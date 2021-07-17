using SchedulingPlugIn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.RepositoryLayer.Interface
{
    public interface IPageRequestRepository : IRepository<PageRequest>
    {

    }
    public interface IPageQuestionRepository : IRepository<PageQuestion>
    {
        object LastOrDefault();
    }
    public interface ICustomerRepository : IRepository<Customer>
    {

    }
    public interface ICalendarRepository : IRepository<Calendar>
    {
        object FirstOrDefault();
        object Filter();
        object Where(Func<object, object> p);
    }
    public interface ICalendarPoolingRepository : IRepository<CalendarPooling>
    {

    }
    public interface ITimeSlotRepository : IRepository<TimeSlot>
    {

    }
    public interface ISessionTimeSlotRepository : IRepository<SessionTimeSlot>
    {

    }
}