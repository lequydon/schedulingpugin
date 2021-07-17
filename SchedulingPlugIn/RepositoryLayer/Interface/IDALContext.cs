using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.RepositoryLayer.Interface
{
    public interface IDALContext : IUnitOfWork
    {
        IPageRequestRepository PageRequests { get; }
        IPageQuestionRepository PageQuestions { get; }
        ICustomerRepository Customers { get; }
        ICalendarRepository Calendars { get; }
        ICalendarPoolingRepository CalendarPoolings { get; }
        ITimeSlotRepository TimeSlots { get; }
        ISessionTimeSlotRepository SessionTimeSlots { get; }
    }
}