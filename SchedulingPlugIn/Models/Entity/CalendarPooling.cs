using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Models.Entity
{
    public class CalendarPooling
    {
        public string ID { get; set; }
        public string IDPageQuestion { get; set; }
        [ForeignKey("IDPageQuestion")]
        public PageQuestion PageQuestion { get; set; }
        public string IDCalendar { get; set; }
        [ForeignKey("IDCalendar")]
        public Calendar Calendar { get; set; }
        public string Answer { get; set; }
        public string Question { get; set; }
        public int OrderQuestion { get; set; }
    }
}