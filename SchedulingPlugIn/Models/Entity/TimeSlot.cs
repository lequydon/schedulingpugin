using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SchedulingPlugIn.Helper;
namespace SchedulingPlugIn.Models.Entity
{
    public class TimeSlot
    {
        public string ID { get; set; }
        [DisplayName("Date Start")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime2")]
        public DateTime DateStart { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateEnd { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Duration { get; set; }
        public string Note { get; set; }
        public string IDAppUser { get; set; }
        public string ID_SessionTimeSlot { get; set; }
        [ForeignKey("ID_SessionTimeSlot")]
        public virtual SessionTimeSlot SessionTimeSlot { get; set; }
        public TimeSlotStatus Status { get; set; }
        public virtual ICollection<Calendar> ListCalendar { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Start
        {
            get
            {
                var date = DateStart;
                return date;
            }
        }
        [Column(TypeName = "datetime2")]
        public DateTime End
        {
            get
            {
                var date = DateStart.AddMinutes(Duration);
                return date;
            }
        }
    }
    public enum TimeSlotStatus
    {
        NotYetUsed = 0,
        Used = 1
    }
}