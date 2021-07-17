using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Models.Entity
{
    public class PageQuestion
    {
        public string ID { get; set; }
        [Required]
        public string Question { get; set; }
        public int OrderQuestion { get; set; }
        public string PageRequestID { get; set; }
        [ForeignKey("PageRequestID")]
        public virtual PageRequest PageRequest { get; set; }
        public virtual ICollection<CalendarPooling> CalendarPoolingList { get; set; }
    }
}