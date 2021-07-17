using SchedulingPlugIn.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Models
{
    public class CalendarUnionTimeSlotViewModel
    {
        public string ID { get; set; }
        public CalendarStatus Status { get; set; }
        public TimeSlotStatus StatusSlot { get; set; }
        public DateTime DateStart { get; set; }
        public int Duration { get; set; }
        public string TimeZone { get; set; }
        public SessionTimeSlot SessionTimeSlot { get; set; }
        public DateTime Start
        {
            get
            {
                var date = DateStart;
                return date;
            }
        }
        public DateTime DateEnd
        {
            get
            {
                return DateStart.AddMinutes(Duration);
            }
        }
        public Calendar Calendar { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public string IDAppUser { get; set; }
        public string Note { get; set; }
        
    }
}