using SchedulingPlugIn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingPlugIn.Models
{
    public class QuestionViewModels
    {
        public string PageID { get; set; }
        public string Name { get; set; }
        public string PageCode { get; set; }
        public string UrlPage { get; set; }
        public string pagerequest { get; set; }
        public string ID { get; set; }
        public string Question { get; set; }
        public int OrderQuestion { get; set; }
    }
}