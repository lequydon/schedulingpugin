using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Models.Entity
{
    public class SessionTimeSlot
    {
        internal readonly object DateStart;

        public string ID { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime SessionStart { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime SessionEnd { get; set; }
        public string TimeZone { get; set; }
        public int Duration { get; set; }
        public virtual ICollection<TimeSlot> ListTimeSlot { get; set; }
    }
}