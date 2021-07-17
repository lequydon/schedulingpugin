using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Models.Entity
{
    public class Calendar
    {
        public string ID { get; set; }
        public string IDPageRequest { get; set; }
        [ForeignKey("IDPageRequest")]
        public PageRequest PageRequest { get; set; }
        public string IDCustomer { get; set; }
        [ForeignKey("IDCustomer")]
        public Customer Customer { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }
        public int HourFrom { get; set; }
        public int MinuteFrom { get; set; }
        public int Duration { get; set; }
        public CalendarStatus Status { get; set; }
        public string Reason { get; set; }
        public virtual ICollection<CalendarPooling> CalendarPoolingList { get; set; }
        public string TimeZone { get; set; }
        public string IDTimeSlot { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateEnd {
            get
            {
                var date = Date.AddMinutes(Duration);
                return date;
            }
        }
    }
    public enum CalendarStatus
    {
        WaitConfirm = 0,
        Confirmed = 1,
        Decline = -1,
        EmailSended = 2,
        Completed = 3
    }
}