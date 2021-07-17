﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Models.Entity
{
    public class Customer
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<Calendar> CalendarList { get; set; }
    }
}