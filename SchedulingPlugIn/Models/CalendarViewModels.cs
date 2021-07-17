using SchedulingPlugIn.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Models
{
    public class CalendarViewModels
    {
        public string ID { get; set; }
        public string IDTimeSlot { get; set; }
        public string IDPageQuestion { get; set; }
        public string IDCalendar { get; set; }
        public string IDPageRequest { get; set; }
        public Calendar Calendar { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public DateTime Date { get; set; }
        public int HourFrom { get; set; }
        public int MinuteFrom { get; set; }
        public int Duration { get; set; }
        public string Answer { get; set; }
        public string Question { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PageRequestName { get; set; }
        public string Reason { get; set; }
        public CalendarStatus  Status { get; set; }
        public TimeSlotStatus StatusSlot { get; set; }
        public int makeColor { get; set; }
        public string Phone { get; set; }
        public int OrderQuestion { get; set; }
        public DateTime datecreated { get; set; }
        public string TimeZone { get; set; }
        public string PageName { get; set; }
        public DateTime DateEnd {
            get
            {
                return Date.AddMinutes(Duration);
            }
        }


        public List<PageQuestion> ListQuestion { get; set; }
        public string Time
        {
            get
            {
                string date = Date.ToString("t");
                string date_end = DateEnd.ToString("t");
                return date + " - " + date_end;
            }
        }
        
        public string DateNotStamp
        {
            get
            {
                return Date.ToString("d");
                
            }
        }
        
    }
}