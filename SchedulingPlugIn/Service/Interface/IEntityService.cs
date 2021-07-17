using SchedulingPlugIn.Models;
using SchedulingPlugIn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Service.Interface
{
    public interface IPageRequestService : IService<PageRequest>
    {
        PageRequest Create(string pageID, string pageName, string pageCode);
    }
    public interface IPageQuestionService : IService<PageQuestion>
    {
    }
    public interface ICustomerService : IService<Customer>
    {
    }
    public interface ICalendarService : IService<Calendar>
    {
        List<CalendarViewModels> GetListCalendarViewModelsT();
        List<ConsultationViewModels> GetListConsultation();
   
        List<ConsultationViewModels> GetListApproved(CalendarStatus status);


    }
    public interface ICalendarPoolingService : IService<CalendarPooling>
    {
    }
    public interface ITimeSlotService : IService<TimeSlot>
    {
        List<TimeSlot> GetList(DateTime dateTime);
        List<CalendarUnionTimeSlotViewModel> GetListForProspect_DetalTimeSlot(DateTime dateTime);
        List<CalendarUnionTimeSlotViewModel> GetListForProspect_DetalTimeSlot();
        //List<CalendarViewModels> GetList(DateTime dateTime);
    }
}