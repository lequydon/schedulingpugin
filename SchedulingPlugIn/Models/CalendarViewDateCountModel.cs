using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Models
{
    public class CalendarViewDateCountModel
    {
        public DateTime date { get; set; }
       public string time { get; set; }
        public int value  { get; set; }
    }
}